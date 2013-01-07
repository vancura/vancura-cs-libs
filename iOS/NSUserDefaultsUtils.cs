using System;
using MonoTouch.Foundation;

namespace Vancura.Utilities.iOS
{


	/// <summary>
	/// User defaults.
	/// </summary>
	public static class NSUserDefaultsUtils
	{


		/// <summary>
		/// Loads the default settings from the Settings.bundle/Root.plist
		/// file. Also calls nested settings (referred to in child pane
		/// items) recursively, to load those defaults.
		/// </summary>
		public static void LoadDefaultSettings ()
		{
			// check to see if they've already been loaded for the first time
			if (!NSUserDefaults.StandardUserDefaults.BoolForKey ("__DefaultsLoaded")) {
				string rootSettingsFilePath = NSBundle.MainBundle.BundlePath + "/Settings.bundle/Root.plist";

				// check to see if there is event a settings file
				if (System.IO.File.Exists (rootSettingsFilePath)) {
					// load the settings
					NSDictionary settings = NSDictionary.FromFile (rootSettingsFilePath);
					LoadSettingsFile (settings);
				}

				// mark them as loaded so this doesn't run again
				NSUserDefaults.StandardUserDefaults.SetBool (true, "__DefaultsLoaded");
			}
		}


		/// <summary>
		/// Recursive version of LoadDefautSetings
		/// </summary>
		private static void LoadSettingsFile (NSDictionary settings)
		{
			bool foundTypeKey;
			bool foundDefaultValue;
			string prefKeyName;
			NSObject prefDefaultValue;
			NSObject key;

			// get the preference specifiers node
			NSArray prefs = settings.ObjectForKey (new NSString ("PreferenceSpecifiers")) as NSArray;

			// loop through the settings
			for (uint i = 0; i < prefs.Count; i++) {
				// reset for each setting
				foundTypeKey = false;
				foundDefaultValue = false;
				prefKeyName = string.Empty;
				prefDefaultValue = new NSObject ();

				NSDictionary pref = new NSDictionary (prefs.ValueAt (i));

				// loop through the dictionary of any particular setting
				for (uint keyCount = 0; keyCount < pref.Keys.Length; keyCount++) {
					// shortcut reference
					key = pref.Keys [keyCount];

					// get the key name and default value
					if (key.ToString () == "Key") {
						foundTypeKey = true;
						prefKeyName = pref [key].ToString ();
					} else if (key.ToString () == "DefaultValue") {
						foundDefaultValue = true;
						prefDefaultValue = pref [key];
					} else if (key.ToString () == "File") {
						NSDictionary nestedSettings = NSDictionary.FromFile (NSBundle.MainBundle.BundlePath + "/Settings.bundle/" + pref [key].ToString () + ".plist");
						LoadSettingsFile (nestedSettings);
					}

					// if we've found both, set it in our user preferences
					if (foundTypeKey && foundDefaultValue) {
						NSUserDefaults.StandardUserDefaults [prefKeyName] = prefDefaultValue;
					}
				}
			}
		}


	}


}

