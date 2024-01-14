using System;
using System.Runtime.InteropServices;
using Hooks.WinAPI;

namespace Hooks;

public class KeyboardGlobalHook : GlobalHookBase, IDisposable
{
    private readonly SafeHookHandle _keyboardHookHandle;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly HookProc _keyboardCallback;

    public event EventHandler<HookKeyEventArgs>? KeyEvent;

    public KeyboardGlobalHook()
    {
        _keyboardCallback = KeyboardHookProc;
        _keyboardHookHandle = SetGlobalHook(WindowsHookType.WH_KEYBOARD_LL, _keyboardCallback);
    }

    private int KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code < 0)
            return User32.CallNextHookEx(_keyboardHookHandle, code, wParam, lParam);

        if (!(Marshal.PtrToStructure(lParam, typeof(KeyboardHookInfo)) is KeyboardHookInfo keyboardInfo))
            return User32.CallNextHookEx(_keyboardHookHandle, code, wParam, lParam);

        var key = (Key)keyboardInfo.VKCode;

        KeyState keyState;
        var message = (WindowsMessage)wParam.ToInt32();
        switch (message)
        {
            case WindowsMessage.KeyDown:
            case WindowsMessage.SysKeyDown:
                keyState = KeyState.Down;
                break;
            case WindowsMessage.KeyUp:
            case WindowsMessage.SysKeyUp:
                keyState = KeyState.Up;
                break;
            default:
                keyState = KeyState.Unknown;
                break;
        }

        var eventArgs = new HookKeyEventArgs(key, keyState);

        KeyEvent?.Invoke(this, eventArgs);
            
        return eventArgs.Handled
            ? 1 : User32.CallNextHookEx(_keyboardHookHandle, code, wParam, lParam);
    }
        
    #region Dispose
        
    private bool _disposed;

    ~KeyboardGlobalHook()
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
            _keyboardHookHandle.Dispose();
        }

        //free unmanaged resources (unmanaged objects) and override finalizer
        //set large fields to null
        _disposed = true;
    }

    #endregion
}