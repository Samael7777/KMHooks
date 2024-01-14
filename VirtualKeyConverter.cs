using Hooks.WinAPI;
// ReSharper disable IdentifierTypo

namespace Hooks;

public static class VirtualKeyConverter
{
    // ReSharper disable once InconsistentNaming
    private const int MAPVK_VK_TO_CHAR = 2;

    public static (Key Key, bool ShiftState) FromChar(char ch)
    {
            var result = User32.VkKeyScan(ch);
            var key = (Key)(result & 0xFF);
            var shift = (result & 0xFF00) > 0;
            return (key, shift);
        }

    public static char ToChar(Key key)
    {
            var charCode = User32.MapVirtualKey((int)key, MAPVK_VK_TO_CHAR);
            return (char)(charCode & 0xFF);
        }
}