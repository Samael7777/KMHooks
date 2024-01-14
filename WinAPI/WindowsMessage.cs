namespace Hooks.WinAPI;

internal enum WindowsMessage 
{
    KeyDown = 0x100,
    KeyUp = 0x101,

    SysKeyDown = 0x104,
    SysKeyUp = 0x105,

    MouseMove = 0x200,
    LeftButtonDown = 0x201,
    LeftButtonUp = 0x202,

    RightButtonDown = 0x204,
    RightButtonUp = 0x205,

    MiddleButtonDown = 0x207,
    MiddleButtonUp = 0x208,
}