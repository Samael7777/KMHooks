using System;
using HooksLib.WinAPI;

namespace HooksLib
{
    public abstract class HookBase
    {
        private static readonly IntPtr User32Handle;

        static HookBase()
        {
            User32Handle = Kernel32.LoadLibrary("user32");
        }

        protected IntPtr GetUser32Handle() => User32Handle;
    }
}