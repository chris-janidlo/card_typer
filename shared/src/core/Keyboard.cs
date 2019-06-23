using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace CTShared
{
public class Keyboard : IEnumerable<KeyState>
{
	public bool Locked;

#region AUTO_GENERATED_CODE
	private KeyState SpaceState = new KeyState { Key = KeyboardKey.Space, Type = KeyStateType.Active };
	private KeyState DashState = new KeyState { Key = KeyboardKey.Dash, Type = KeyStateType.Active };
	private KeyState ApostropheState = new KeyState { Key = KeyboardKey.Apostrophe, Type = KeyStateType.Active };
	private KeyState BackspaceState = new KeyState { Key = KeyboardKey.Backspace, Type = KeyStateType.Active };
	private KeyState ReturnState = new KeyState { Key = KeyboardKey.Return, Type = KeyStateType.Active };
	private KeyState AState = new KeyState { Key = KeyboardKey.A, Type = KeyStateType.Active };
	private KeyState BState = new KeyState { Key = KeyboardKey.B, Type = KeyStateType.Active };
	private KeyState CState = new KeyState { Key = KeyboardKey.C, Type = KeyStateType.Active };
	private KeyState DState = new KeyState { Key = KeyboardKey.D, Type = KeyStateType.Active };
	private KeyState EState = new KeyState { Key = KeyboardKey.E, Type = KeyStateType.Active };
	private KeyState FState = new KeyState { Key = KeyboardKey.F, Type = KeyStateType.Active };
	private KeyState GState = new KeyState { Key = KeyboardKey.G, Type = KeyStateType.Active };
	private KeyState HState = new KeyState { Key = KeyboardKey.H, Type = KeyStateType.Active };
	private KeyState IState = new KeyState { Key = KeyboardKey.I, Type = KeyStateType.Active };
	private KeyState JState = new KeyState { Key = KeyboardKey.J, Type = KeyStateType.Active };
	private KeyState KState = new KeyState { Key = KeyboardKey.K, Type = KeyStateType.Active };
	private KeyState LState = new KeyState { Key = KeyboardKey.L, Type = KeyStateType.Active };
	private KeyState MState = new KeyState { Key = KeyboardKey.M, Type = KeyStateType.Active };
	private KeyState NState = new KeyState { Key = KeyboardKey.N, Type = KeyStateType.Active };
	private KeyState OState = new KeyState { Key = KeyboardKey.O, Type = KeyStateType.Active };
	private KeyState PState = new KeyState { Key = KeyboardKey.P, Type = KeyStateType.Active };
	private KeyState QState = new KeyState { Key = KeyboardKey.Q, Type = KeyStateType.Active };
	private KeyState RState = new KeyState { Key = KeyboardKey.R, Type = KeyStateType.Active };
	private KeyState SState = new KeyState { Key = KeyboardKey.S, Type = KeyStateType.Active };
	private KeyState TState = new KeyState { Key = KeyboardKey.T, Type = KeyStateType.Active };
	private KeyState UState = new KeyState { Key = KeyboardKey.U, Type = KeyStateType.Active };
	private KeyState VState = new KeyState { Key = KeyboardKey.V, Type = KeyStateType.Active };
	private KeyState WState = new KeyState { Key = KeyboardKey.W, Type = KeyStateType.Active };
	private KeyState XState = new KeyState { Key = KeyboardKey.X, Type = KeyStateType.Active };
	private KeyState YState = new KeyState { Key = KeyboardKey.Y, Type = KeyStateType.Active };
	private KeyState ZState = new KeyState { Key = KeyboardKey.Z, Type = KeyStateType.Active };

	public IEnumerator<KeyState> GetEnumerator()
	{
		yield return SpaceState;
		yield return DashState;
		yield return ApostropheState;
		yield return BackspaceState;
		yield return ReturnState;
		yield return AState;
		yield return BState;
		yield return CState;
		yield return DState;
		yield return EState;
		yield return FState;
		yield return GState;
		yield return HState;
		yield return IState;
		yield return JState;
		yield return KState;
		yield return LState;
		yield return MState;
		yield return NState;
		yield return OState;
		yield return PState;
		yield return QState;
		yield return RState;
		yield return SState;
		yield return TState;
		yield return UState;
		yield return VState;
		yield return WState;
		yield return XState;
		yield return YState;
		yield return ZState;
	}
#endregion

	IEnumerator IEnumerable.GetEnumerator ()
	{
		return GetEnumerator();
	}

	public KeyState this[KeyboardKey key]
	{
		get { return (KeyState) getField(key).GetValue(this); }
		set { if (!Locked) getField(key).SetValue(this, value); }
	}

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

    FieldInfo getField (KeyboardKey key)
    {
        return this.GetType().GetField($"{key.ToString()}State", BindingFlags.Instance | BindingFlags.NonPublic);
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
