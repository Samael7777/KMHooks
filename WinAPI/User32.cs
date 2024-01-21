using System;
using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo

namespace KMHooks.WinAPI;

public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

internal static class User32
{
    [DllImport("user32")]
    public static extern int CallNextHookEx(SafeHookHandle hHk, int nCode,
        IntPtr wParam, IntPtr lParam);
        
    [DllImport("user32")]
    public static extern SafeHookHandle SetWindowsHookEx(WindowsHookType idHookType,
        IntPtr lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32")]
    public static extern bool UnhookWindowsHookEx(IntPtr hHk);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern short VkKeyScan(char ch);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MapVirtualKey(int uCode, int uMapType);
}