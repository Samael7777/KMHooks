using System.Collections.Generic;

namespace HooksLib
{
    public delegate void KeyPressedHandler(object sender, KeyPressedEventArgs args);

    public class KeyCombinationHook
    {
        private readonly KeyboardHook _keyboardHook;
        private readonly HashSet<Key> _currentModifiers;
       
        public event KeyPressedHandler OnKeyPressed;

        public KeyCombinationHook(KeyboardHook keyboardHook)
        {
            _currentModifiers = new HashSet<Key>();
            _keyboardHook = keyboardHook;
            _keyboardHook.KeyEvent += OnKeyboardStateChanged;
        }

        ~KeyCombinationHook()
        {
            _keyboardHook.KeyEvent -= OnKeyboardStateChanged;
        }

        private void OnKeyboardStateChanged(object sender, HookKeyEventArgs args)
        {
            var key = args.KeyData;
            if (KeyHelper.IsModifier(key))
            {
                switch (args.KeyState)
                {
                    case KeyState.Down:
                        _currentModifiers.Add(args.KeyData);
                        break;
                    case KeyState.Up:
                        _currentModifiers.Remove(args.KeyData);
                        break;
                }
            } 
            else if (args.KeyState == KeyState.Down)
            {
                var keyPressedArgs = new KeyPressedEventArgs(args.KeyData, _currentModifiers);
                _currentModifiers.Clear();
                OnKeyPressed?.Invoke(this, keyPressedArgs);
            }
        }
    }
}