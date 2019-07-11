//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace CTShared
{
public partial class Keyboard : IEnumerable<KeyState>
{
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

	KeyState getState (KeyboardKey key)
	{
		switch (key)
		{
			case KeyboardKey.Space:
				return SpaceState;
			case KeyboardKey.Dash:
				return DashState;
			case KeyboardKey.Apostrophe:
				return ApostropheState;
			case KeyboardKey.Backspace:
				return BackspaceState;
			case KeyboardKey.Return:
				return ReturnState;
			case KeyboardKey.A:
				return AState;
			case KeyboardKey.B:
				return BState;
			case KeyboardKey.C:
				return CState;
			case KeyboardKey.D:
				return DState;
			case KeyboardKey.E:
				return EState;
			case KeyboardKey.F:
				return FState;
			case KeyboardKey.G:
				return GState;
			case KeyboardKey.H:
				return HState;
			case KeyboardKey.I:
				return IState;
			case KeyboardKey.J:
				return JState;
			case KeyboardKey.K:
				return KState;
			case KeyboardKey.L:
				return LState;
			case KeyboardKey.M:
				return MState;
			case KeyboardKey.N:
				return NState;
			case KeyboardKey.O:
				return OState;
			case KeyboardKey.P:
				return PState;
			case KeyboardKey.Q:
				return QState;
			case KeyboardKey.R:
				return RState;
			case KeyboardKey.S:
				return SState;
			case KeyboardKey.T:
				return TState;
			case KeyboardKey.U:
				return UState;
			case KeyboardKey.V:
				return VState;
			case KeyboardKey.W:
				return WState;
			case KeyboardKey.X:
				return XState;
			case KeyboardKey.Y:
				return YState;
			case KeyboardKey.Z:
				return ZState;
			default:
				throw new ArgumentException($"unexpected KeyboardKey {key}");
		}
	}
}
}
