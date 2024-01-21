using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KMHooks.WinAPI;

[DebuggerDisplay("{handle.ToInt64()}")]
public class SafeHookHandle : SafeHandle
{
    public SafeHookHandle() : base(IntPtr.Zero, true)
    { }

    protected override bool ReleaseHandle()
    {
            return IsInvalid || User32.UnhookWindowsHookEx(handle);
        }

    public override bool IsInvalid => handle == IntPtr.Zero;
}