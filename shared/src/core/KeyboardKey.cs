using System;

namespace CTShared
{
public enum KeyboardKey : byte
{
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
    Space, Dash, Apostrophe,
    Backspace, Return
}

public static class KeyboardKeyExtensions
{
    public static char ToChar (this KeyboardKey key, bool uppercase = false)
    {
        if (!key.IsAcceptableInput(AcceptableKeyboardKeyFlags.Alphabetical | AcceptableKeyboardKeyFlags.Punctuation))
            throw new Exception("given key is not alphabetical or punctuation");

        switch (key)
        {
            case KeyboardKey.Space:
                return ' ';
            
            case KeyboardKey.Dash:
                return '-';

            case KeyboardKey.Apostrophe:
                return '\'';

            default:
                char chr = key.ToString()[0];
                return uppercase ? chr : char.ToLower(chr);
        }
    }

    public static string ToLabel (this KeyboardKey key)
    {
        if (key.IsAcceptableInput())
        {
            return key.ToChar().ToString();
        }
        else
        {
            switch (key)
            {
                case KeyboardKey.Backspace:
                    return "←";
                
                case KeyboardKey.Return:
                    return "return";

                default:
                    throw new System.Exception($"unexpected keycode {key}");
            }
        }
    }

    public static bool IsAcceptableInput (this KeyboardKey key, AcceptableKeyboardKeyFlags flags = AcceptableKeyboardKeyFlags.AllNonFunctional)
    {
        bool output = false;

        if (flags.HasFlag(AcceptableKeyboardKeyFlags.Functional))
        {
            output = output ||
                key == KeyboardKey.Backspace ||
                key == KeyboardKey.Return;
        }

        if (flags.HasFlag(AcceptableKeyboardKeyFlags.Punctuation))
        {
            output = output ||
                key == KeyboardKey.Space ||
                key == KeyboardKey.Dash ||
                key == KeyboardKey.Apostrophe;
        }
        
        if (flags.HasFlag(AcceptableKeyboardKeyFlags.Alphabetical))
        {
            output = output ||
                (key >= KeyboardKey.A && key <= KeyboardKey.Z);
        }
        
        return output;
    }
}

[Flags]
public enum AcceptableKeyboardKeyFlags
{
    Alphabetical = 1 << 0,
    Punctuation = 1 << 1,
    Functional = 1 << 2,

    AllNonFunctional = Alphabetical | Punctuation,
    Everything = AllNonFunctional | Functional
}
}
