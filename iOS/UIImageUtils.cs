using System;
using MonoTouch.UIKit;
using System.IO;

namespace Vancura.Utilities.iOS
{


	/// <summary>
	/// UIImage utilities.
	/// </summary>
	public static class UIImageUtils
	{


		/// <summary>
		/// Get the UIImage from the path, regardless if the device is HD or not.
		/// </summary>
		/// <returns>UIImage from the path</returns>
		/// <param name="path">Original path</param>
		public static UIImage FromBundle16x9 (string path)
		{
			if (DetectorUtils.Is16x9) {
				var imagePath = Path.GetDirectoryName (path);
				var imageFile = Path.GetFileNameWithoutExtension (path);
				var imageExt = Path.GetExtension (path);
				
				imageFile = imageFile + "-568h@2x" + imageExt;
				
				return UIImage.FromFile (Path.Combine (imagePath, imageFile));
			}
			
			else {
				return UIImage.FromBundle (path);
			}
		}


	}
}
