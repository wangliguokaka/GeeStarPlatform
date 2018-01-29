using System.Runtime.InteropServices;

namespace MyCsActiveXControl
{
    [ComImport, Guid("CB5BDC81-93C1-11CF-8F20-00805F2CD064")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectSafety
    {
        [PreserveSig]
        void GetInterfacceSafyOptions(
        int riid,
        out int pdwSupportedOptions,
        out int pdwEnabledOptions);

        [PreserveSig]
        void SetInterfaceSafetyOptions(
        int riid,
        int dwOptionsSetMask,
        int dwEnabledOptions);
    }
}