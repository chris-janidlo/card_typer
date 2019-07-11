using System;
using System.Collections;
using System.Collections.Generic;

namespace CTShared
{
public partial class Keyboard : IEnumerable<KeyState>
{
	public bool Locked;

	IEnumerator IEnumerable.GetEnumerator ()
	{
		return GetEnumerator();
	}

	public KeyState this[KeyboardKey key] => getState(key);

	// TODO: insanely hard coded; maybe do something like https://stackoverflow.com/q/2120646
	public List<KeyState> GetSurroundingKeys (KeyboardKey key)
	{
		List<KeyState> ret = new List<KeyState>();
		switch (key)
		{
			case KeyboardKey.Dash:
				ret.Add(this[KeyboardKey.P]);
				break;

			case KeyboardKey.Space:
				ret.Add(this[KeyboardKey.X]);
				ret.Add(this[KeyboardKey.C]);
				ret.Add(this[KeyboardKey.V]);
				ret.Add(this[KeyboardKey.B]);
				ret.Add(this[KeyboardKey.N]);
				ret.Add(this[KeyboardKey.M]);
				break;

			case KeyboardKey.A:
				ret.Add(this[KeyboardKey.Q]);
				ret.Add(this[KeyboardKey.W]);
				ret.Add(this[KeyboardKey.S]);
				ret.Add(this[KeyboardKey.X]);
				ret.Add(this[KeyboardKey.Z]);
				break;

			case KeyboardKey.B:
				ret.Add(this[KeyboardKey.V]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.N]);
				ret.Add(this[KeyboardKey.Space]);
				break;

			case KeyboardKey.C:
				ret.Add(this[KeyboardKey.X]);
				ret.Add(this[KeyboardKey.S]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.V]);
				ret.Add(this[KeyboardKey.Space]);
				break;

			case KeyboardKey.D:
				ret.Add(this[KeyboardKey.W]);
				ret.Add(this[KeyboardKey.E]);
				ret.Add(this[KeyboardKey.R]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.V]);
				ret.Add(this[KeyboardKey.C]);
				ret.Add(this[KeyboardKey.X]);
				ret.Add(this[KeyboardKey.S]);
				break;

			case KeyboardKey.E:
				ret.Add(this[KeyboardKey.W]);
				ret.Add(this[KeyboardKey.S]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.R]);
				break;

			case KeyboardKey.F:
				ret.Add(this[KeyboardKey.E]);
				ret.Add(this[KeyboardKey.R]);
				ret.Add(this[KeyboardKey.T]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.B]);
				ret.Add(this[KeyboardKey.V]);
				ret.Add(this[KeyboardKey.C]);
				ret.Add(this[KeyboardKey.D]);
				break;

			case KeyboardKey.G:
				ret.Add(this[KeyboardKey.R]);
				ret.Add(this[KeyboardKey.T]);
				ret.Add(this[KeyboardKey.Y]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.N]);
				ret.Add(this[KeyboardKey.B]);
				ret.Add(this[KeyboardKey.V]);
				ret.Add(this[KeyboardKey.F]);
				break;

			case KeyboardKey.H:
				ret.Add(this[KeyboardKey.T]);
				ret.Add(this[KeyboardKey.Y]);
				ret.Add(this[KeyboardKey.U]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.M]);
				ret.Add(this[KeyboardKey.N]);
				ret.Add(this[KeyboardKey.B]);
				ret.Add(this[KeyboardKey.G]);
				break;

			case KeyboardKey.I:
				ret.Add(this[KeyboardKey.U]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.K]);
				ret.Add(this[KeyboardKey.L]);
				ret.Add(this[KeyboardKey.O]);
				break;

			case KeyboardKey.J:
				ret.Add(this[KeyboardKey.Y]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.N]);
				ret.Add(this[KeyboardKey.M]);
				ret.Add(this[KeyboardKey.K]);
				ret.Add(this[KeyboardKey.I]);
				ret.Add(this[KeyboardKey.U]);
				break;

			case KeyboardKey.K:
				ret.Add(this[KeyboardKey.U]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.M]);
				ret.Add(this[KeyboardKey.L]);
				ret.Add(this[KeyboardKey.O]);
				ret.Add(this[KeyboardKey.I]);
				break;

			case KeyboardKey.L:
				ret.Add(this[KeyboardKey.I]);
				ret.Add(this[KeyboardKey.K]);
				ret.Add(this[KeyboardKey.O]);
				ret.Add(this[KeyboardKey.P]);
				break;

			case KeyboardKey.M:
				ret.Add(this[KeyboardKey.N]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.K]);
				ret.Add(this[KeyboardKey.Space]);
				break;

			case KeyboardKey.N:
				ret.Add(this[KeyboardKey.B]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.M]);
				ret.Add(this[KeyboardKey.Space]);
				break;

			case KeyboardKey.O:
				ret.Add(this[KeyboardKey.I]);
				ret.Add(this[KeyboardKey.K]);
				ret.Add(this[KeyboardKey.L]);
				ret.Add(this[KeyboardKey.P]);
				break;

			case KeyboardKey.P:
				ret.Add(this[KeyboardKey.O]);
				ret.Add(this[KeyboardKey.L]);
				ret.Add(this[KeyboardKey.Dash]);
				break;

			case KeyboardKey.Q:
				ret.Add(this[KeyboardKey.A]);
				ret.Add(this[KeyboardKey.W]);
				ret.Add(this[KeyboardKey.S]);
				break;

			case KeyboardKey.R:
				ret.Add(this[KeyboardKey.E]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.T]);
				break;

			case KeyboardKey.S:
				ret.Add(this[KeyboardKey.Q]);
				ret.Add(this[KeyboardKey.W]);
				ret.Add(this[KeyboardKey.E]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.C]);
				ret.Add(this[KeyboardKey.X]);
				ret.Add(this[KeyboardKey.Z]);
				ret.Add(this[KeyboardKey.A]);
				break;

			case KeyboardKey.T:
				ret.Add(this[KeyboardKey.R]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.Y]);
				break;

			case KeyboardKey.U:
				ret.Add(this[KeyboardKey.Y]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.K]);
				ret.Add(this[KeyboardKey.I]);
				break;

			case KeyboardKey.V:
				ret.Add(this[KeyboardKey.C]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.F]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.B]);
				ret.Add(this[KeyboardKey.Space]);
				break;

			case KeyboardKey.W:
				ret.Add(this[KeyboardKey.Q]);
				ret.Add(this[KeyboardKey.A]);
				ret.Add(this[KeyboardKey.S]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.E]);
				break;

			case KeyboardKey.X:
				ret.Add(this[KeyboardKey.Z]);
				ret.Add(this[KeyboardKey.A]);
				ret.Add(this[KeyboardKey.S]);
				ret.Add(this[KeyboardKey.D]);
				ret.Add(this[KeyboardKey.C]);
				ret.Add(this[KeyboardKey.Space]);
				break;

			case KeyboardKey.Y:
				ret.Add(this[KeyboardKey.T]);
				ret.Add(this[KeyboardKey.G]);
				ret.Add(this[KeyboardKey.H]);
				ret.Add(this[KeyboardKey.J]);
				ret.Add(this[KeyboardKey.U]);
				break;

			case KeyboardKey.Z:
				ret.Add(this[KeyboardKey.A]);
				ret.Add(this[KeyboardKey.S]);
				ret.Add(this[KeyboardKey.X]);
				break;
		}
		return ret;
	}

	public List<KeyState> GetSurroundingKeys (KeyState state)
	{
		return GetSurroundingKeys(state.Key);
	}
}

public enum KeyStateType
{
    Active, Deactivated, Sticky
}

[Serializable]
public class KeyState
{
	public KeyboardKey Key;
    public KeyStateType Type;
	public int EnergyLevel;
    public int StickyPressesRemaining;
}
}
