using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour, IEnumerable<KeyState>
{
	public bool Locked;

#region AUTO_GENERATED_CODE
	[SerializeField]
	private KeyState BackspaceState = new KeyState { Key = KeyCode.Backspace, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState MinusState = new KeyState { Key = KeyCode.Minus, Type = KeyStateType.Active };
	private KeyState ReturnState = new KeyState { Key = KeyCode.Return, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState SpaceState = new KeyState { Key = KeyCode.Space, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState QuoteState = new KeyState { Key = KeyCode.Quote, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState AState = new KeyState { Key = KeyCode.A, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState BState = new KeyState { Key = KeyCode.B, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState CState = new KeyState { Key = KeyCode.C, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState DState = new KeyState { Key = KeyCode.D, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState EState = new KeyState { Key = KeyCode.E, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState FState = new KeyState { Key = KeyCode.F, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState GState = new KeyState { Key = KeyCode.G, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState HState = new KeyState { Key = KeyCode.H, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState IState = new KeyState { Key = KeyCode.I, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState JState = new KeyState { Key = KeyCode.J, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState KState = new KeyState { Key = KeyCode.K, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState LState = new KeyState { Key = KeyCode.L, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState MState = new KeyState { Key = KeyCode.M, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState NState = new KeyState { Key = KeyCode.N, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState OState = new KeyState { Key = KeyCode.O, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState PState = new KeyState { Key = KeyCode.P, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState QState = new KeyState { Key = KeyCode.Q, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState RState = new KeyState { Key = KeyCode.R, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState SState = new KeyState { Key = KeyCode.S, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState TState = new KeyState { Key = KeyCode.T, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState UState = new KeyState { Key = KeyCode.U, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState VState = new KeyState { Key = KeyCode.V, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState WState = new KeyState { Key = KeyCode.W, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState XState = new KeyState { Key = KeyCode.X, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState YState = new KeyState { Key = KeyCode.Y, Type = KeyStateType.Active };
	[SerializeField]
	private KeyState ZState = new KeyState { Key = KeyCode.Z, Type = KeyStateType.Active };

	public IEnumerator<KeyState> GetEnumerator()
	{
		yield return BackspaceState;
		yield return MinusState;
		yield return ReturnState;
		yield return SpaceState;
		yield return QuoteState;
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

	public KeyState this[KeyCode key]
	{
		get { return (KeyState) getField(key).GetValue(this); }
		set { if (!Locked) getField(key).SetValue(this, value); }
	}

	// TODO: insanely hard coded; maybe do something like https://stackoverflow.com/q/2120646
	public List<KeyState> GetSurroundingKeys (KeyCode key)
	{
		List<KeyState> ret = new List<KeyState>();
		switch (key)
		{
			case KeyCode.Minus:
				ret.Add(this[KeyCode.P]);
				break;

			case KeyCode.Space:
				ret.Add(this[KeyCode.X]);
				ret.Add(this[KeyCode.C]);
				ret.Add(this[KeyCode.V]);
				ret.Add(this[KeyCode.B]);
				ret.Add(this[KeyCode.N]);
				ret.Add(this[KeyCode.M]);
				break;

			case KeyCode.A:
				ret.Add(this[KeyCode.Q]);
				ret.Add(this[KeyCode.W]);
				ret.Add(this[KeyCode.S]);
				ret.Add(this[KeyCode.X]);
				ret.Add(this[KeyCode.Z]);
				break;

			case KeyCode.B:
				ret.Add(this[KeyCode.V]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.N]);
				ret.Add(this[KeyCode.Space]);
				break;

			case KeyCode.C:
				ret.Add(this[KeyCode.X]);
				ret.Add(this[KeyCode.S]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.V]);
				ret.Add(this[KeyCode.Space]);
				break;

			case KeyCode.D:
				ret.Add(this[KeyCode.W]);
				ret.Add(this[KeyCode.E]);
				ret.Add(this[KeyCode.R]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.V]);
				ret.Add(this[KeyCode.C]);
				ret.Add(this[KeyCode.X]);
				ret.Add(this[KeyCode.S]);
				break;

			case KeyCode.E:
				ret.Add(this[KeyCode.W]);
				ret.Add(this[KeyCode.S]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.R]);
				break;

			case KeyCode.F:
				ret.Add(this[KeyCode.E]);
				ret.Add(this[KeyCode.R]);
				ret.Add(this[KeyCode.T]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.B]);
				ret.Add(this[KeyCode.V]);
				ret.Add(this[KeyCode.C]);
				ret.Add(this[KeyCode.D]);
				break;

			case KeyCode.G:
				ret.Add(this[KeyCode.R]);
				ret.Add(this[KeyCode.T]);
				ret.Add(this[KeyCode.Y]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.N]);
				ret.Add(this[KeyCode.B]);
				ret.Add(this[KeyCode.V]);
				ret.Add(this[KeyCode.F]);
				break;

			case KeyCode.H:
				ret.Add(this[KeyCode.T]);
				ret.Add(this[KeyCode.Y]);
				ret.Add(this[KeyCode.U]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.M]);
				ret.Add(this[KeyCode.N]);
				ret.Add(this[KeyCode.B]);
				ret.Add(this[KeyCode.G]);
				break;

			case KeyCode.I:
				ret.Add(this[KeyCode.U]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.K]);
				ret.Add(this[KeyCode.L]);
				ret.Add(this[KeyCode.O]);
				break;

			case KeyCode.J:
				ret.Add(this[KeyCode.Y]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.N]);
				ret.Add(this[KeyCode.M]);
				ret.Add(this[KeyCode.K]);
				ret.Add(this[KeyCode.I]);
				ret.Add(this[KeyCode.U]);
				break;

			case KeyCode.K:
				ret.Add(this[KeyCode.U]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.M]);
				ret.Add(this[KeyCode.L]);
				ret.Add(this[KeyCode.O]);
				ret.Add(this[KeyCode.I]);
				break;

			case KeyCode.L:
				ret.Add(this[KeyCode.I]);
				ret.Add(this[KeyCode.K]);
				ret.Add(this[KeyCode.O]);
				ret.Add(this[KeyCode.P]);
				break;

			case KeyCode.M:
				ret.Add(this[KeyCode.N]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.K]);
				ret.Add(this[KeyCode.Space]);
				break;

			case KeyCode.N:
				ret.Add(this[KeyCode.B]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.M]);
				ret.Add(this[KeyCode.Space]);
				break;

			case KeyCode.O:
				ret.Add(this[KeyCode.I]);
				ret.Add(this[KeyCode.K]);
				ret.Add(this[KeyCode.L]);
				ret.Add(this[KeyCode.P]);
				break;

			case KeyCode.P:
				ret.Add(this[KeyCode.O]);
				ret.Add(this[KeyCode.L]);
				ret.Add(this[KeyCode.Minus]);
				break;

			case KeyCode.Q:
				ret.Add(this[KeyCode.A]);
				ret.Add(this[KeyCode.W]);
				ret.Add(this[KeyCode.S]);
				break;

			case KeyCode.R:
				ret.Add(this[KeyCode.E]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.T]);
				break;

			case KeyCode.S:
				ret.Add(this[KeyCode.Q]);
				ret.Add(this[KeyCode.W]);
				ret.Add(this[KeyCode.E]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.C]);
				ret.Add(this[KeyCode.X]);
				ret.Add(this[KeyCode.Z]);
				ret.Add(this[KeyCode.A]);
				break;

			case KeyCode.T:
				ret.Add(this[KeyCode.R]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.Y]);
				break;

			case KeyCode.U:
				ret.Add(this[KeyCode.Y]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.K]);
				ret.Add(this[KeyCode.I]);
				break;

			case KeyCode.V:
				ret.Add(this[KeyCode.C]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.F]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.B]);
				ret.Add(this[KeyCode.Space]);
				break;

			case KeyCode.W:
				ret.Add(this[KeyCode.Q]);
				ret.Add(this[KeyCode.A]);
				ret.Add(this[KeyCode.S]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.E]);
				break;

			case KeyCode.X:
				ret.Add(this[KeyCode.Z]);
				ret.Add(this[KeyCode.A]);
				ret.Add(this[KeyCode.S]);
				ret.Add(this[KeyCode.D]);
				ret.Add(this[KeyCode.C]);
				ret.Add(this[KeyCode.Space]);
				break;

			case KeyCode.Y:
				ret.Add(this[KeyCode.T]);
				ret.Add(this[KeyCode.G]);
				ret.Add(this[KeyCode.H]);
				ret.Add(this[KeyCode.J]);
				ret.Add(this[KeyCode.U]);
				break;

			case KeyCode.Z:
				ret.Add(this[KeyCode.A]);
				ret.Add(this[KeyCode.S]);
				ret.Add(this[KeyCode.X]);
				break;
		}
		return ret;
	}

	public List<KeyState> GetSurroundingKeys (KeyState state)
	{
		return GetSurroundingKeys(state.Key);
	}

    FieldInfo getField (KeyCode key)
    {
        return this.GetType().GetField($"{key.ToString()}State", BindingFlags.Instance | BindingFlags.NonPublic);
    }
}

public enum KeyStateType
{
    Active, Deactivated, Sticky, Delayed
}

[Serializable]
public class KeyState
{
	public KeyCode Key;
    public KeyStateType Type;
	public int EnergyLevel;
    public int StickyPressesRemaining;
    public float DelaySeconds;
}
