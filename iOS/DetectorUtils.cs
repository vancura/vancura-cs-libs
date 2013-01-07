using System;
using MonoTouch.UIKit;
using System.IO;

namespace Vancura.Utilities.iOS
{


	/// <summary>
	/// Detector.
	/// </summary>
	public static class DetectorUtils
	{


		static bool isDetected;
		static bool isIOS5OrBetter;
		static bool isIOS6OrBetter;
		static int versionMajor = 0;
		static int versionMinor = 0;
		static bool is16x9;


#region Getters


		/// <summary>
		/// Gets a value indicating if the iOS system is 5 or better.
		/// </summary>
		/// <value>
		/// <c>true</c> if is the iOS system is 5 or better; otherwise, <c>false</c>.
		/// </value>
		public static bool IsIOS5OrBetter {
			get {
				ProcessDetection ();
				
				return isIOS5OrBetter;
			}
			
		}
		
		
		/// <summary>
		/// Gets a value indicating if the iOS system is 6 or better.
		/// </summary>
		/// <value>
		/// <c>true</c> if is the iOS system is 6 or better; otherwise, <c>false</c>.
		/// </value>
		public static bool IsIOS6OrBetter {
			get {
				ProcessDetection ();
				
				return isIOS6OrBetter;
			}
			
		}


		/// <summary>
		/// Gets the major iOS version number.
		/// </summary>
		/// <value>
		/// The major iOS version number.
		/// </value>
		public static int VersionMajor {
			get {
				ProcessDetection ();
				
				return versionMajor;
			}
		}
		
		
		
		/// <summary>
		/// Gets the minor iOS version number.
		/// </summary>
		/// <value>
		/// The minor iOS version number.
		/// </value>
		public static int VersionMinor {
			get {
				ProcessDetection ();
				
				return versionMinor;
			}
		}


		/// <summary>
		/// Gets the 16x9 HD flag.
		/// </summary>
		/// <value><c>true</c> if is device is 16:9; otherwise, <c>false</c>.</value>
		public static bool Is16x9 {
			get { 
				ProcessDetection ();

				return is16x9;
			}     
		}


#endregion


#region Private
				

		/// <summary>
		/// Runs the detection and caches results.
		/// </summary>
		static void ProcessDetection ()
		{
			if (!isDetected) {
				string version = UIDevice.CurrentDevice.SystemVersion;
				string[] versionElements = version.Split ('.');

				if (versionElements.Length > 0) {
					if (Int32.TryParse (versionElements [0], out versionMajor)) {
						if (Int32.TryParse (versionElements [1], out versionMinor)) {
							isIOS6OrBetter = (versionMajor >= 6);
							isIOS5OrBetter = (versionMajor >= 5);
						}
					}
				}

				is16x9 =  UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= 1136;

				isDetected = true;
			}
		}


#endregion


	}


}
