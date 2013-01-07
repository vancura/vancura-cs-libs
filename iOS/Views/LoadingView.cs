using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Vancura.Utilities.iOS.Views
{


	/// <summary>
	/// Loading view.
	/// </summary>
	public class LoadingView : UIAlertView
	{


		UIActivityIndicatorView _activityView;


		/// <summary>
		/// Show the the view.
		/// </summary>
		/// <param name='title'>
		/// Title.
		/// </param>
		public void Show (string title)
		{
			Title = title;

			Show ();
			
			_activityView = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);
			_activityView.Frame = new RectangleF ((Bounds.Width / 2) - 15, Bounds.Height - 62, 30, 30);
			_activityView.StartAnimating ();

			AddSubview (_activityView);
		}


		/// <summary>
		/// Hide the view.
		/// </summary>
		public void Hide ()
		{
			DismissWithClickedButtonIndex (0, true);
		}


	}


}
