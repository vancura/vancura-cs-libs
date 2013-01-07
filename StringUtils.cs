using System.Security.Cryptography;
using System.Text;

namespace Vancura.Utilities
{


	/// <summary>
	/// String utilities.
	/// </summary>
	public static class StringUtils
	{


		/// <summary>
		/// method to generate a MD5 hash of a string
		/// </summary>
		/// <param name="strToHash">string to hash</param>
		/// <returns>hashed string</returns>
		public static string GenerateMD5 (string str)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
			
			byte[] byteArray = Encoding.ASCII.GetBytes (str);
			
			byteArray = md5.ComputeHash (byteArray);
			
			string hashedValue = "";
			
			foreach (byte b in byteArray) {
				hashedValue += b.ToString ("x2");
			}
			
			return hashedValue;
		}


	}


}
