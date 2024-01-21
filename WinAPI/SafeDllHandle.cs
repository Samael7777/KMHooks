using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KMHooks.WinAPI;

[DebuggerDisplay("{handle}")]
public class SafeDllHandle : SafeHandle
{
    public SafeDllHandle() : base(IntPtr.Zero, true)
    { }

    protected override bool ReleaseHandle()
    {
            return IsInvalid || Kernel32.FreeLibrary(handle);
        }

    public override bool IsInvalid =>  handle == IntPtr.Zero;
}