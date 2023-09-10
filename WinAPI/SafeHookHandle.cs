using System;
using System.Runtime.InteropServices;

namespace HooksLib.WinAPI
{
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
}