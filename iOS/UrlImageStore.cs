using System;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using Vancura.CSLibs;


namespace Vancura.Utilities.iOS
{


	/// <summary>
	/// UrlImageUpdated interface.
	/// </summary>
	public interface IUrlImageUpdated
	{
		void UrlImageUpdated (string id, UIImage image);
	}


	/// <summary>
	/// URL image store.
	/// Code based on Redth's UrlImageStore, but not using the not very stable
	/// NSOperationQueue under MonoTouch but rather the Parallels taks library.
	/// Original one: https://gist.github.com/405923
	/// </summary>
	public class UrlImageStore : NSObject
	{
		public delegate UIImage ProcessImageDelegate (string id, UIImage img);
		
		readonly static string baseDir;
		string cachedImageDir;
		TaskFactory taskFactory;
		LimitedConcurrencyLevelTaskScheduler lcts;
		const int CONCURRENT_THREADS = 5; // max number of downloading threads


#region Store handling
			

		/// <summary>
		/// Initializes the <see cref="MonoTouch.UrlImageStore.UrlImageStore"/> class.
		/// </summary>
		static UrlImageStore ()
		{
			baseDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "..");
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="MonoTouch.UrlImageStore.UrlImageStore"/> class.
		/// Creates a folder for cached images, if it doesn't exist already.
		/// </summary>
		/// <param name="storeName">Store name.</param>
		/// <param name="processImage">Process image delegate.</param>
		public UrlImageStore (string storeName, ProcessImageDelegate processImage)
		{
			StoreName = storeName;
			ProcessImage = processImage;
			
			lcts = new LimitedConcurrencyLevelTaskScheduler (CONCURRENT_THREADS);
			taskFactory = new TaskFactory (lcts);

			// create cached folder if it doesn't already exist
			if (!Directory.Exists (Path.Combine (baseDir, "Library/Caches/Pictures/" + storeName)))
				Directory.CreateDirectory (Path.Combine (baseDir, "Library/Caches/Pictures/" + storeName));
			
			cachedImageDir = Path.Combine (baseDir, "Library/Caches/Pictures/" + storeName);

			Console.WriteLine ("UrlImageStore: Setting up image store at {0}", cachedImageDir);
		}


		/// <summary>
		/// Deletes all cached files.
		/// </summary>
		public void DeleteCachedFiles ()
		{
			string[] files = new string[]{};
			
			try {
				files = Directory.GetFiles (cachedImageDir);
			} catch (Exception ex) {
				Console.WriteLine ("UrlImageStore: Error getting list of cached images ({0})\n{1}", ex.Message, ex.StackTrace);
				return;
			}
			
			foreach (string file in files) {
				try {
					File.Delete (file);
				} catch (Exception ex) {
					Console.WriteLine ("UrlImageStore: Error deleting cached image ({0})\n{1}", ex.Message, ex.StackTrace);
				}
			}

			Console.WriteLine ("UrlImageStore: Cached images deleted");
		}


#endregion


#region Image handling


		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="id">Identifier.</param>
		public UIImage GetImage (string id)
		{
			string cachedFilename = cachedImageDir + "/" + StringUtils.GenerateMD5 (id) + ".png";
			
			if (File.Exists (cachedFilename)) {
				UIImage img = null;
				
				try {
					img = UIImage.FromFile (cachedFilename);
				} catch (Exception ex) {
					Console.WriteLine ("UrlImageStore: Error loading image from {0} ({1})\n{2}", cachedFilename, ex.Message, ex.StackTrace);
				}
				
				if (img != null) {
					return img;
				}
			}
			
			return null;
		}
		
		
		/// <summary>
		/// Requests the image.
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="url">URL.</param>
		/// <param name="notify">Notify.</param>
		public UIImage RequestImage (string id, string url, IUrlImageUpdated notify)
		{
			if (string.IsNullOrEmpty (url))
				return null; // do not start for empty string
			
			// next check for a saved file, and load it into cache and return it if found
			string cachedFilename = cachedImageDir + "/" + StringUtils.GenerateMD5 (id) + ".png";
			
			if (File.Exists (cachedFilename)) {
				UIImage cachedImage = null;
				
				try {
					cachedImage = UIImage.FromFile (cachedFilename);
				} catch (Exception ex) {
					Console.WriteLine ("UrlImageStore: Error loading image from {0} ({1})\n{2}", cachedFilename, ex.Message, ex.StackTrace);
				}
				
				if (cachedImage != null) {					
					return cachedImage;
				}
			}

			// download the image now
			DownloadImage (id, url, notify);
			
			// return the default while they wait for the queued download
			return null;
		}


		/// <summary>
		/// Downloads the image.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="url">URL.</param>
		/// <param name="notify">Notify.</param>
		public void DownloadImage (string id, string url, IUrlImageUpdated notify) {
			taskFactory.StartNew (delegate {				
				try {
					var data = NSData.FromUrl (NSUrl.FromString (url));
					
					if (data == null) {
						Console.WriteLine ("UrlImageStore: No data for " + url);
						return;
					}
					
					var img = UIImage.LoadFromData (data);
					
					img = ProcessImage (id, img);
					
					AddToCache (id, img);
					
					notify.UrlImageUpdated (id, img);
				} catch (Exception ex) {
					Console.WriteLine ("UrlImageStore: Errorloading image {0} ({1})\n{2}", url, ex.Message, ex.StackTrace);
				}				
			});
		}
		
		
		/// <summary>
		/// Adds to cache.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="img">Image.</param>
		internal void AddToCache (string id, UIImage img)
		{
			string cachedFilename = cachedImageDir + "/" + StringUtils.GenerateMD5 (id) + ".png";
			
			if (!File.Exists (cachedFilename)) {
				// save it to disk
				NSError err = null;

				try { 
					img.AsPNG ().Save (cachedFilename, false, out err); 

					if (err != null)
						Console.WriteLine ("UrlImageStore: Error encoding file {0} to cache ({1} - {2})", cachedFilename, err.Code.ToString (), err.LocalizedDescription);
				} catch (Exception ex) {
					Console.WriteLine ("UrlImageStore: Error encoding file {0} to cache ({1})\n{2}", cachedFilename, ex.Message, ex.StackTrace);
				}
			}
		}		


#endregion


#region Getters & setters


		/// <summary>
		/// Gets the process image.
		/// </summary>
		/// <value>The process image.</value>
		public ProcessImageDelegate ProcessImage {
			get;
			private set;
		}


		/// <summary>
		/// Gets the name of the store.
		/// </summary>
		/// <value>The name of the store.</value>
		public string StoreName {
			get;
			private set;	
		}


#endregion


	}


	/// <summary>
	/// URL image store request.
	/// </summary>
	public class UrlImageStoreRequest
	{

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		public string Url {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the notify.
		/// </summary>
		/// <value>The notify.</value>
		public IUrlImageUpdated Notify {
			get;
			set;
		}


	}


}
