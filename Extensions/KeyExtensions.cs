using System.Linq;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace Hooks.Extensions;

public static class KeyExtensions
{
    private static readonly Key[] Modifiers =
    {
        Key.RCONTROL,
        Key.LCONTROL,
        Key.INV_CONTROL,
        Key.RSHIFT,
        Key.LSHIFT,
        Key.INV_SHIFT,
        Key.LWIN,
        Key.RWIN,
        Key.INV_WIN,
        Key.RALT,
        Key.LALT,
        Key.INV_ALT
    };

    private static readonly Key[] SideInvariants =
    {
        Key.INV_SHIFT,
        Key.INV_ALT,
        Key.INV_CONTROL,
        Key.INV_WIN
    };

    public static bool IsModifier(this Key keycode)
    {
        return Modifiers.Contains(keycode);
    }

    public static bool IsSideInvariant(this Key keycode)
    {
        return SideInvariants.Contains(keycode);
    }

    public static Key ToSideInvariant(this Key key)
    {
        Key result;
        switch (key)
        {
            case Key.RALT:
            case Key.LALT:
                result = Key.INV_ALT;
                break;
            case Key.LWIN:
            case Key.RWIN:
                result = Key.INV_WIN;
                break;
            case Key.LCONTROL:
            case Key.RCONTROL:
                result = Key.INV_CONTROL;
                break;
            case Key.RSHIFT:
            case Key.LSHIFT:
                result = Key.INV_SHIFT;
                break;
            default:
                result = key;
                break;
        }
        return result;
    }

    public static (Key Right, Key Left) FromInvariant(this Key key)
    {
        switch (key)
        {
            case Key.INV_ALT:
                return (Key.RALT, Key.LALT);
            case Key.INV_SHIFT:
                return (Key.RSHIFT, Key.LSHIFT);
            case Key.INV_CONTROL:
                return (Key.RCONTROL, Key.LCONTROL);
            case Key.INV_WIN:
                return (Key.RWIN, Key.LWIN);
            default:
                return (key, key);
        }
    }
}