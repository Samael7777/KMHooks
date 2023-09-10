using System;
using System.Collections.Generic;


namespace HooksLib
{
    public class KeyPressedEventArgs : EventArgs
    {
        public HashSet<Key> Modifiers { get; }
        public Key PressedKey { get; }

        public KeyPressedEventArgs(Key pressedKey, IEnumerable<Key> modifiers = null)
        {
            PressedKey = pressedKey;
            Modifiers = modifiers != null ? new HashSet<Key>(modifiers) : new HashSet<Key>();
        }
    }
}