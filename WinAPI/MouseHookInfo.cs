// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo

using System;
using System.Runtime.InteropServices;

namespace Hooks.WinAPI;

// MSLLHOOKSTRUCT
// https://msdn.microsoft.com/ru-ru/library/windows/desktop/ms644970(v=vs.85).aspx
[StructLayout(LayoutKind.Sequential)]
public class MouseHookInfo
{
    public int X;
    public int Y;
    public uint MouseData;
    public uint Flags;
    public uint Time;
    public IntPtr dwExtraInfo;
}