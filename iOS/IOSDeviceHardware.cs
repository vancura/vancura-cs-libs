using System;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;


namespace Vancura.Utilities.Helpers
{


    /// <summary>
    /// IOS device hardware.
    /// Data from http://theiphonewiki.com/wiki/Models
    /// </summary>
    public class IOSDeviceHardware
    {
        public const string HardwareProperty = "hw.machine";


        public enum IOSHardware
        {
            iPhone,
            iPhone3G,
            iPhone3GS,
            iPhone4,
            iPhone4RevA,
            iPhone4CDMA,
            iPhone4S,
            iPhone5GSM,
            iPhone5CDMAGSM,
            iPhone5CGSM,
            iPhone5CGlobal,
            iPhone5SGSM,
            iPhone5SGlobal,
            iPodTouch1G,
            iPodTouch2G,
            iPodTouch3G,
            iPodTouch4G,
            iPodTouch5G,
            iPad,
            iPad3G,
            iPad2,
            iPad2GSM,
            iPad2CDMA,
            iPad2RevA,
            iPadMini,
            iPadMiniGSM,
            iPadMiniCDMAGSM,
            iPad3,
            iPad3CDMA,
            iPad3GSM,
            iPad4,
            iPad4GSM,
            iPad4CDMAGSM,
            iPadAir,
            iPadAirCDMAGSM,
            iPadMini2G,
            iPadMini2GCDMAGSM,
            AppleTV2G,
            AppleTV3G,
            AppleTV3GRevA,
            iPhoneSimulator,
            iPhoneRetinaSimulator,
            iPadSimulator,
            iPadRetinaSimulator,
            Unknown
        }


        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctlbyname ([MarshalAs (UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);


        public static IOSHardware Version {
            get {
                var pLen = Marshal.AllocHGlobal (sizeof(int));
                sysctlbyname (IOSDeviceHardware.HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);

                var length = Marshal.ReadInt32 (pLen);

                if (length == 0) {
                    Marshal.FreeHGlobal (pLen);

                    return IOSHardware.Unknown;
                }

                var pStr = Marshal.AllocHGlobal (length);
                sysctlbyname (IOSDeviceHardware.HardwareProperty, pStr, pLen, IntPtr.Zero, 0);

                var hardwareStr = Marshal.PtrToStringAnsi (pStr);

                Marshal.FreeHGlobal (pLen);
                Marshal.FreeHGlobal (pStr);

                if (hardwareStr == "iPhone1,1")
                    return IOSHardware.iPhone;
                if (hardwareStr == "iPhone1,2")
                    return IOSHardware.iPhone3G;
                if (hardwareStr == "iPhone2,1")
                    return IOSHardware.iPhone3GS;
                if (hardwareStr == "iPhone3,1")
                    return IOSHardware.iPhone4;
                if (hardwareStr == "iPhone3,2")
                    return IOSHardware.iPhone4RevA;
                if (hardwareStr == "iPhone3,3")
                    return IOSHardware.iPhone4CDMA;
                if (hardwareStr == "iPhone4,1")
                    return IOSHardware.iPhone4S;
                if (hardwareStr == "iPhone5,1")
                    return IOSHardware.iPhone5GSM;
                if (hardwareStr == "iPhone5,2")
                    return IOSHardware.iPhone5CDMAGSM;
                if (hardwareStr == "iPhone5,3")
                    return IOSHardware.iPhone5CGSM;
                if (hardwareStr == "iPhone5,4")
                    return IOSHardware.iPhone5CGlobal;
                if (hardwareStr == "iPhone6,1")
                    return IOSHardware.iPhone5SGSM;
                if (hardwareStr == "iPhone6,2")
                    return IOSHardware.iPhone5SGlobal;

                if (hardwareStr == "iPad1,1")
                    return IOSHardware.iPad;
                if (hardwareStr == "iPad1,2")
                    return IOSHardware.iPad3G;
                if (hardwareStr == "iPad2,1")
                    return IOSHardware.iPad2;
                if (hardwareStr == "iPad2,2")
                    return IOSHardware.iPad2GSM;
                if (hardwareStr == "iPad2,3")
                    return IOSHardware.iPad2CDMA;
                if (hardwareStr == "iPad2,4")
                    return IOSHardware.iPad2RevA;
                if (hardwareStr == "iPad2,5")
                    return IOSHardware.iPadMini;
                if (hardwareStr == "iPad2,6")
                    return IOSHardware.iPadMiniGSM;
                if (hardwareStr == "iPad2,7")
                    return IOSHardware.iPadMiniCDMAGSM;
                if (hardwareStr == "iPad3,1")
                    return IOSHardware.iPad3;
                if (hardwareStr == "iPad3,2")
                    return IOSHardware.iPad3CDMA;
                if (hardwareStr == "iPad3,3")
                    return IOSHardware.iPad3GSM;
                if (hardwareStr == "iPad3,4")
                    return IOSHardware.iPad4;
                if (hardwareStr == "iPad3,5")
                    return IOSHardware.iPad4GSM;
                if (hardwareStr == "iPad3,6")
                    return IOSHardware.iPad4CDMAGSM;
                if (hardwareStr == "iPad4,1")
                    return IOSHardware.iPadAir;
                if (hardwareStr == "iPad4,2")
                    return IOSHardware.iPadAirCDMAGSM;
                if (hardwareStr == "iPad4,4")
                    return IOSHardware.iPadMini2G;
                if (hardwareStr == "iPad4,5")
                    return IOSHardware.iPadMini2GCDMAGSM;

                if (hardwareStr == "iPod1,1")
                    return IOSHardware.iPodTouch1G;
                if (hardwareStr == "iPod2,1")
                    return IOSHardware.iPodTouch2G;
                if (hardwareStr == "iPod3,1")
                    return IOSHardware.iPodTouch3G;
                if (hardwareStr == "iPod4,1")
                    return IOSHardware.iPodTouch4G;
                if (hardwareStr == "iPod5,1")
                    return IOSHardware.iPodTouch5G;

                if (hardwareStr == "AppleTV2,1")
                    return IOSHardware.AppleTV2G;
                if (hardwareStr == "AppleTV3,1")
                    return IOSHardware.AppleTV3G;
                if (hardwareStr == "AppleTV3,2")
                    return IOSHardware.AppleTV3GRevA;

                if (hardwareStr == "i386" || hardwareStr == "x86_64") {
                    if (UIDevice.CurrentDevice.Model.Contains ("iPhone")) {
                        if (UIScreen.MainScreen.Scale > 1.5f)
                            return IOSHardware.iPhoneRetinaSimulator;
                        else
                            return IOSHardware.iPhoneSimulator;
                    } else {
                        if (UIScreen.MainScreen.Scale > 1.5f)
                            return IOSHardware.iPadRetinaSimulator;
                        else
                            return IOSHardware.iPadSimulator;
                    }
                }

                return IOSHardware.Unknown;
            }


        }


    }


}
