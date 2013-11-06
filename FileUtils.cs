using System;
using System.IO;


namespace Vancura.CSLibs {


    /// <summary>
    /// File utilities.
    /// </summary>
    public static class FileUtils {


        /// <summary>
        /// Get the crossplatform file path.
        /// </summary>
        /// <returns>
        /// The crossplatform file path.
        /// </returns>
        /// <param name='filename'>
        /// Original filename.
        /// </param>
        public static string FilePath(string filename) {
            #if SILVERLIGHT
			    var path = filename;
            #else
                #if __ANDROID__
                    string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
                #else
                    // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                    // (they don't want non-user-generated data in Documents)
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                    string libraryPath = Path.Combine(documentsPath, "..", "Library");
                #endif
                var path = Path.Combine(libraryPath, filename);
            #endif

            return path;
        }
    }
}
