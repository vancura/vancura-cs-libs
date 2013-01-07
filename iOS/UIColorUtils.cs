using System;
using MonoTouch.UIKit;

namespace Vancura.Utilities.iOS
{


	/// <summary>
	/// UIColor utilities.
	/// </summary>
	public static class UIColorUtils
	{


		/// <summary>
		/// Convert a hex value to a UIColor
		/// </summary>
		/// <param name='color'>
		/// Resulting UIColor.
		/// </param>
		/// <param name='hexValue'>
		/// Hex value.
		/// </param>
		public static UIColor FromHex (this UIColor color, int hexValue)
		{
			return UIColor.FromRGB (
				(((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
				(((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
				(((float)(hexValue & 0xFF)) / 255.0f)
			);
		}


	}


}
