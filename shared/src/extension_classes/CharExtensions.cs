using System;

namespace CTShared
{
public static class CharExtensions
{
	public static KeyboardKey ToKeyboardKey (this char chr)
	{
		if (chr == ' ')
		{
			return KeyboardKey.Space;
		}
			
		if (chr == '-')
		{
			return KeyboardKey.Dash;
		}
			
		if (chr == '\'')
		{
			return KeyboardKey.Apostrophe;
		}

		if ((chr >= 'a' && chr <= 'z') || (chr >= 'A' && chr <= 'Z'))
		{
			return (KeyboardKey) Enum.Parse(typeof(KeyboardKey), chr.ToString(), true);
		}

		throw new ArgumentException(chr + " is not a supported KeyboardKey");
	}
}
}
