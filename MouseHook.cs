using System;
using HooksLib.WinAPI;

namespace HooksLib
{
    public delegate void HookMouseEventHandler(object sender, HookMouseEventArgs e);

    public class MouseHook : HookBase, IDisposable
    {
        private readonly SafeHookHandle _mouseHookHandle;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly MouseHookProc _mouseCallback;
        
        public event HookMouseEventHandler MouseEvent;

        public MouseHook()
        {
            _mouseCallback = MouseHookProc;
            _mouseHookHandle = SetMouseHook(_mouseCallback);
        }

        private int MouseHookProc(int code, WindowsMessage wParam, ref MouseHookStruct lParam)
        {
            if (code < 0) 
                return User32.CallNextHookEx(_mouseHookHandle, code, wParam, ref lParam);

            MouseButtons mouseButton;
            switch (wParam)
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

            var arg = new HookMouseEventArgs(mouseButton, 0, lParam.X, lParam.Y, 0, lParam);
            MouseEvent?.Invoke(this, arg);
            return User32.CallNextHookEx(_mouseHookHandle, code, wParam, ref lParam);

        }

        private SafeHookHandle SetMouseHook(MouseHookProc proc)
        {
            var user32Handle = GetUser32Handle();
            return User32.SetWindowsHookEx(WindowsHook.MouseLowLevel, proc, user32Handle, 0);
        }
        
        
        #region Dispose
        
        private bool _disposed;

        ~MouseHook()
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
}