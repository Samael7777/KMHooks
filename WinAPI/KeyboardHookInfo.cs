// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo

using System;
using System.Runtime.InteropServices;

namespace Hooks.WinAPI;

// KBDLLHOOKSTRUCT
// https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms644967(v=vs.85).aspx
[StructLayout(LayoutKind.Sequential)]
internal class KeyboardHookInfo
{
    public uint VKCode;
    public uint ScanCode;
    public uint Flags;
    public uint Time;
    public IntPtr dwExtraInfo;
}