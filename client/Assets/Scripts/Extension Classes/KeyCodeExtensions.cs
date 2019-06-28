using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public static class KeyCodeExtensions
{
    public static KeyboardKey? ToKeyboardKey (this KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Space:
                return KeyboardKey.Space;
            case KeyCode.Minus:
                return KeyboardKey.Dash;
            case KeyCode.Quote:
                return KeyboardKey.Apostrophe;
            case KeyCode.Backspace:
                return KeyboardKey.Backspace;
            case KeyCode.Return:
                return KeyboardKey.Return;
        }

        if (key >= KeyCode.A && key <= KeyCode.Z)
        {
            return (KeyboardKey) (key - KeyCode.A);
        }
        else
        {
            return null;
        }
    }
}
