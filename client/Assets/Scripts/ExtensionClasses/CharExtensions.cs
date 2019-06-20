using System;
using UnityEngine;
using UnityEngine.Assertions;

public static class CharExtensions
{
	public static KeyCode ToKeyCode (this char chr)
	{
		return (KeyCode) Enum.Parse(typeof(KeyCode), chr.ToString(), true);
	}
}
