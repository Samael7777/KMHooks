using System.Linq;

namespace HooksLib
{
    public static class KeyHelper
    {
        private static readonly Key[] Modifiers =
        {
            Key.LAlt,
            Key.RAlt,
            Key.RControlKey,
            Key.LControlKey,
            Key.RShiftKey,
            Key.LShiftKey,
            Key.LWin,
            Key.RWin,
            Key.Apps
        };

        public static bool IsModifier(Key keycode)
        {
            return Modifiers.Contains(keycode);
        }
    }
}