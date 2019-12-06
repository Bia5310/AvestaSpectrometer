using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvestaSpectrometr
{
    class UsbCCD
    {
        public const string dllname = "CCDUSBDCOM01.dll";

        //The function CCD_Init should start before all another function.
        //This function performs the search of all CCD-cameras and sets the initial parameters
        //ahAppWnd may be 0. Prm and ID not used;
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_Init")]
        public static extern bool CCD_Init(IntPtr ahAppWnd, StringBuilder prm, ref int ID);

        //The function CCD_HitTest is used for hit test of CCD-cameras
        //If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        //If one device is used then ID = 0.
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_HitTest")]
        public static extern bool CCD_HitTest(int ID);

        // Cause function CCD_CameraReset when there was a mistake or it is necessary to interrupt registration.
        //If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        //If one device is used then ID = 0.
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_CameraReset")]
        public static extern bool CCD_CameraReset(int ID);

        //The function CCD_SetParameters is used for CCD-camera parameters' setting.
        //The parameter Prms is structure of type TCCDUSBParams. The declaration of structure
        //TCCDUSBParams defines above.
        //It is allowed to set only following parameters:
        //  - exposure time
        //  - number of readouts
        //  - synchronization mode
        //The remaining parameters is set automatically
        //If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        //If one device is used then ID = 0.
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_SetParameters")]
        public static extern bool CCD_SetParameters(int ID, ref TCCDUSBParams Prms);

        //The function CCD_SetExtendParameters is used for CCD-camera parameters' setting.
        //The parameter Prms is structure of type TCCDUSBExtendParams. The declaration of structure
        //TCCDUSBExtendParams defines above.
        //It is allowed to set only following parameters:
        //  - exposure time
        //  - number of readouts
        //  - synchronization mode
        //  - device mode
        //  - strips
        //  - the time preliminary burning
        //  - the raised sensitivity
        //  - shuter time
        //The remaining parameters is set automatically.
        //If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        //If one device is used then ID = 0.
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_SetExtendParameters")]
        public static extern bool CCD_SetExtendParameters(int ID, ref TCCDUSBExtendParams Prms);

        //The function CCD_GetParameters is used to get the current parameters
        //of CCD-camera.
        //If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        //If one device is used then ID = 0.
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_GetParameters")]
        public static extern bool CCD_GetParameters(int ID, ref TCCDUSBParams Prms);

        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_GetExtendParameters")]
        public static extern bool CCD_GetExtendParameters(int ID, ref TCCDUSBExtendParams Prms);

        //The function CCD_SetParameter is used to set the parameters of CCD-camera separately
        // If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        // If one device is used then ID = 0.
        // dwPrmID - parameter identification number. Its can take following values of constants:
        // PRM_READOUTS - the number of readouts
        // PRM_EXPTIME  - the exposure time
        // PRM_SYNCHR   - the synchronization mode. In the external synchronization mode the number of
        //                readouts always equals one.
        //Prm - the value of parameter
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_SetParameter")]
        public static extern bool CCD_SetParameter(int ID, uint dwPrmID, float Prm);

        //The function CCD_GetParameter is used to get the parameters of CCD-camera separately
        // If some devices are used ID is the identifier USB-device. Can be 0, 1, 2.
        // If one device is used then ID = 0.
        // dwPrmID - parameter identification number. Its can take following values of constants:
        // PRM_DIGIT       - The digit capacity of CCD-camera
        // PRM_PIXELRATE   - The pixel timing
        // PRM_NUMPIXELS   - The number of pixels
        // PRM_READOUTS    - The  number of readouts
        // PRM_EXPTIME     - The exposure time
        // PRM_SYNCHR      - The synchronization mode
        //Prm - the returned value of parameter
        [DllImport(dllname, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "CCD_GetParameter")]
        public static extern bool CCD_GetParameter(int ID, uint dwPrmID, ref float Prm);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TCCDUSBParams
        {
            public uint dwDigitCapacity;//The digit capacity of CCD-camera
            public int nPixelRate;      //The pixel rate kHz
            public int nNumPixels;      //The number of pixels

            public int nNumReadOuts;    //The number of readouts
            public int nExposureTime;   //The exposure time
            public uint dwSynchr;       //The synchronization mode
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TCCDUSBExtendParams
        {
            public uint dwDigitCapacity;// The digit capacity of CCD-camera
            public int nPixelRate;      // The pixel rate (kHz)
            public int nNumPixelsH;     // The number of pixels on a horizontal (columns number of CCD-array)
            public int nNumPixelsV;     // The number of pixels on a vertical (rows number of CCD-array)
            public uint Reserve1;       // not used
            public uint Reserve2;       // not used

            public int nNumReadOuts;   // The number of readouts
            public float sPreBurning;  // The Time preliminary burning in seconds.
                                       // Really only at synchronization SYNCHR_CONTR_NEG but not for all cameras!!!
                                       // Use GetDeviceProperty function to receive properties of the device.
                                       // Is used at a spectrum measurements.
            public float sExposureTime;// The exposure time
            public float sTime2;       // The accumelation delay in mksec
            public uint dwSynchr;      // The synchronization mode.
            public bool bSummingMode;  // Turn on(off) summing mode. Not used.

            public uint dwDeviceMode;  // Turn on(off) spectral mode of CCD-array.
                                       // Some cameras on the basis of CCD-matrixes have an additional modes. See dwProperty.
                                       // DP_MODEA2 It is an additional mode of the matrix registrar. If the device has DP_MODEA2 property it is possible to establish dwDeviceMode in value DEVICEMODEA2.
                                       // In mode DEVICEMODES the device works in a spectroscopic mode.
                                       // The photosensitive field of a matrix is broken into some strips. Strips are set by parameters nStripCount and rcStrips.
                                       // While translating the device in mode DEVICEMODES change nNumPixelsH and nNumPixelsV.
            public int nStripCount;    // The number of strips for a spectral mode

            [MarshalAs(UnmanagedType.Struct, SizeConst = 8)]
            public Rectangle[] rcStrips; // The strips for a spectral mode.//rcStrips        : array[0..8 - 1] of TRect; 
            public int Reserve11;

            public uint dwSensitivity; // Turn on (off) a mode of the raised sensitivity of a CCD-sensor control. Actually if dwProperty & DP_SENSIT <> 0.
            public uint dwProperty;    // The device property.
            public float sShuterTime;  // Shuter time (ms). Active in minimal exposure time.
                                       // Exposure time = MinExp - sShaterTime.
            public uint dwPropertyExt; // // The Extend device property.
            public uint Reserve7;      // not used
            public uint Reserve8;      // not used
            public uint Reserve9;      // not used
            public uint Reserve10;     // not used
        }

        public enum DEVICEPROPERTY : int {

            SYNCHR_CONTR = 00000001, // SYNCHR_CONTR is enaible
            SYNCHR_CONTR_FRS = 00000002, // SYNCHR_CONTR_FRS is enaible
            SYNCHR_CONTR_NEG = 00000004, // SYNCHR_CONTR_NEG is enaible
            SYNCHR_EXT = 00000008, // SYNCHR_EXT is enaible
            SYNCHR_EXT_FRS = 00000010, // SYNCHR_EXT_FRS is enaible
            SENSIT = 00000020, // The sensor or first sensor has a mode of the raised sensitivity.
            MODEA2 = 00000040, // Additional matrix mode of the camera.
            MODES1 = 00000080, // Spectroscopic mode of a CCD-matrix.
            MODES2 = 00000100, // Spectroscopic mode of a CCD-matrix.
            PREBURNING = 00000200, // Opportunity to establish preliminary burning.
            SHUTER = 00000400, // Property of an electronic shutter.
            CLOCKCONTROL = 00000800, // Control ADC clock frequency (nPixelRate).
            FASTSCAN = 00001000, // The "fast scan mode" is enaible
            STREAMSCAN = 00002000, // The "stream scan mode" is enaible
            SENSIT2 = 00004000, // The second sensor has a mode of the raised sensitivity.
            STENDMODE2 = 00008000, // Not used
            STENDLIGHTERCONTROL = 00010000, // Not used
            COLORSENSOR = 00020000, // Color sensor
            STREAMSCANBYTE = 00040000, // The "byte-per-pixel stream scan mode" is enaible
            STREAMSCANBYTE1 = 00080000, // The additional "byte-per-pixel stream scan mode" is enaible 
            STREAMSCANBYTE3 = 00100000, // The additional "byte-per-pixel stream scan mode" is enaible
            REGISTERMODE = 00200000, // Set the QUADRO register mode. ALL, LU, RU, LD, RD
            SPIMEMORY = 00400000, // SPI Memory
            SHUTTER2 = 00800000, // Extenal Shutter from Solar LS
            SHUTTER3 = 01000000, // Extenal Shutter from Solar LS


        }

        //Extended 
        public int DELAY = 00000001; // The accumelation Delay in mksec

        public enum SHUTTERSOLARLS : int
        {
            MODE_OFF = 01,
            MODE_ON = 02,
            MODE_AUTO = 04,
            MODE_INVERT = 08,
        }

        public int NCAMMAX = 3;

        // DEVICE MODE
        // Some cameras on the basis of CCD-matrixes have an additional modes. See dwProperty.
        // DP_MODEA2 It is an additional mode of the matrix registrar. If the device has DP_MODEA2 property it is possible to establish dwDeviceMode in value DEVICEMODEA2.
        // In mode DEVICEMODES the device works in a spectroscopic mode.
        // The photosensitive field of a matrix is broken into some strips. Strips are set by parameters nStripCount and rcStrips.
        // While translating the device in mode DEVICEMODES change nNumPixelsH and nNumPixelsV.
        public enum DeviceMode : int
        {
            DEVICEMODEA1 = 0002,
            DEVICEMODEA2 = 0000,
            DEVICEMODES = 0003,
            DEVICEMODESTREAM = 0004,
            DEVICEMODEFASTSCAN = 0010,
            DEVICEMODESTEND2 = 0001,
            DEVICEMODESTREAMBYTE = 0020,
            DEVICEMODESTREAMBYTE1 = 0040,
        }

        //status of measurement
        public enum StatusOfMeasurement: int
        {
            STATUS_WAIT_DATA = 1, //the measurement in processing
            STATUS_WAIT_TRIG = 2, //the waiting of synchronization pulse
            STATUS_DATA_READY = 3, //the measurement has been finished
        }

        public enum SyncMode : int
        {
            // The synchronization mode
            SYNCHR_NONE = 1,   // Without synchronization.
            SYNCHR_CONTR = 20,  // In the beginning of the first accumulation the positive
                                 // pulse of synchronization is formed.
            SYNCHR_CONTR_FRS = 4,   // Clock pulse is formed in the beginning of each accumulation.
            SYNCHR_CONTR_NEG = 8,   // One pulse of synchronization is formed on all time of registration.
                                     // A pulse of negative polarity.
            SYNCHR_EXT = 10,  // The beginning of the first accumulation is adhered to growing
                               // front of external clock pulse.
                               // All other accumulation occur so quickly as it is possible.
                               // In a limit -- without the misses.
            SYNCHR_EXT_FRS = 2,   // The beginning of each accumulation is adhered to growing front of clock pulse.
                                   // How much accumulation, so much clock pulses are expected.

        }
    }
}
