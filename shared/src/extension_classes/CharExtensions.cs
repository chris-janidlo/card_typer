using System;

namespace CTShared
{
public static class CharExtensions
{
	public static KeyboardKey ToKeyboardKey (this char chr)
	{
		return (KeyboardKey) Enum.Parse(typeof(KeyboardKey), chr.ToString(), true);
	}
}
}
