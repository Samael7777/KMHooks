using System;
using HooksLib.WinAPI;

namespace HooksLib
{
    public delegate void HookKeyEventHandler(object sender, HookKeyEventArgs e);

    public class KeyboardHook : HookBase, IDisposable
    {
        private readonly SafeHookHandle _keyboardHookHandle;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly KeyboardHookProc _keyboardCallback;

        public event HookKeyEventHandler KeyEvent;

        public KeyboardHook()
        {
            _keyboardCallback = KeyboardHookProc;
            _keyboardHookHandle = SetKeyboardHook(_keyboardCallback);
        }

        private int KeyboardHookProc(int code, WindowsMessage wParam, ref KeyboardHookStruct lParam)
        {
            if (code < 0)
                return User32.CallNextHookEx(_keyboardHookHandle, code, wParam, ref lParam);
            
            var key = (Key)lParam.VKCode;

            KeyState keyState;
            switch (wParam)
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
                ? 1 : User32.CallNextHookEx(_keyboardHookHandle, code, wParam, ref lParam);
        }

        private SafeHookHandle SetKeyboardHook(KeyboardHookProc hookProc)
        {
            var user32Handle = GetUser32Handle();
            return User32.SetWindowsHookEx(WindowsHook.KeyboardLowLevel, hookProc, user32Handle, 0);
        }

        #region Dispose
        
        private bool _disposed;

        ~KeyboardHook()
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
}