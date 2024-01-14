using System;
using System.Runtime.InteropServices;

namespace Hooks.WinAPI;

internal static class Kernel32
{
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    public static extern SafeDllHandle LoadLibrary([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);
        
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr hLibModule);
}