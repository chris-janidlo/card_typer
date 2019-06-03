using UnityEngine;
using UnityEngine.Assertions;

public static class KeyCodeExtensions
{
    public static char ToChar (this KeyCode key, bool uppercase = false)
    {
        Assert.IsTrue(key.IsAcceptableInput(false));

        switch (key)
        {
            case KeyCode.Space:
                return ' ';
            
            case KeyCode.Minus:
                return '-';

            case KeyCode.Quote:
                return '\'';

            default:
                char chr = key.ToString()[0];
                return uppercase ? chr : char.ToLower(chr);
        }
    }

    public static bool IsAcceptableInput (this KeyCode key, bool includeSpecial = true)
    {
        bool special = 
            key == KeyCode.Backspace ||
            key == KeyCode.Return;

        bool keyboard = 
            key == KeyCode.Space ||
            key == KeyCode.Minus ||
            key == KeyCode.Quote ||
            (key >= KeyCode.A && key <= KeyCode.Z);
        
        return includeSpecial ? (special || keyboard) : (keyboard);
    }
}
