using System;
using System.Runtime.InteropServices;
using KMHooks.WinAPI;

namespace KMHooks;

public class MouseGlobalHook : GlobalHookBase, IDisposable
{
    private readonly SafeHookHandle _mouseHookHandle;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly HookProc _mouseCallback;
        
    public event EventHandler<HookMouseEventArgs>? MouseEvent;

    public MouseGlobalHook()
    {
        _mouseCallback = MouseHookProc;
        _mouseHookHandle = SetGlobalHook(WindowsHookType.WH_MOUSE_LL, _mouseCallback);
    }

    private int MouseHookProc(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code < 0) 
            return User32.CallNextHookEx(_mouseHookHandle, code, wParam, lParam);

        MouseButtons mouseButton;
        var message = (WindowsMessage)wParam.ToInt32();
        switch (message)
        {
            case WindowsMessage.LeftButtonDown:
            case WindowsMessage.LeftButtonUp:
                mouseButton = MouseButtons.Left;
                break;

            case WindowsMessage.RightButtonDown:
            case WindowsMessage.RightButtonUp:
                mouseButton = MouseButtons.Right;
                break;

            case WindowsMessage.MiddleButtonDown:
            case WindowsMessage.MiddleButtonUp:
                mouseButton = MouseButtons.Middle;
                break;

            case WindowsMessage.MouseMove:
            default:
                mouseButton = MouseButtons.None;
                break;
        }

        if (!(Marshal.PtrToStructure(lParam, typeof(MouseHookInfo)) is MouseHookInfo mouseInfo))
            return User32.CallNextHookEx(_mouseHookHandle, code, wParam, lParam);

        var arg = new HookMouseEventArgs(mouseButton, 0, mouseInfo.X, mouseInfo.Y, 0, mouseInfo);
        MouseEvent?.Invoke(this, arg);
        return User32.CallNextHookEx(_mouseHookHandle, code, wParam, lParam);

    }

    #region Dispose
        
    private bool _disposed;

    ~MouseGlobalHook()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            //dispose managed state (managed objects)
            _mouseHookHandle.Dispose();
        }

        //free unmanaged resources (unmanaged objects) and override finalizer
        //set large fields to null
        _disposed = true;
    }

    #endregion
}