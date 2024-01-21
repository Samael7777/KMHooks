using System.Runtime.InteropServices;
using KMHooks.WinAPI;

namespace KMHooks;

public abstract class GlobalHookBase
{
    protected static SafeHookHandle SetGlobalHook(WindowsHookType hookType, HookProc hookProc)
    {
        var user32Handle = Kernel32.LoadLibrary("user32");
        var hookProcHandle = Marshal.GetFunctionPointerForDelegate(hookProc);
        return User32.SetWindowsHookEx(hookType, hookProcHandle, user32Handle.DangerousGetHandle(), 0);
    }
}