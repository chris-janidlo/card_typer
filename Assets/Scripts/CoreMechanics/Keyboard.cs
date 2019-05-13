using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour, IEnumerable<KeyState>
{
	public bool Locked;

#region AUTO_GENERATED_FIELDS
	[SerializeField]
	private KeyState BackspaceState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState ReturnState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState SpaceState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState AState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState BState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState CState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState DState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState EState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState FState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState GState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState HState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState IState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState JState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState KState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState LState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState MState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState NState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState OState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState PState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState QState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState RState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState SState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState TState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState UState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState VState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState WState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState XState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState YState = new KeyState { Type = KeyStateType.Active };
	[SerializeField]
	private KeyState ZState = new KeyState { Type = KeyStateType.Active };

	public IEnumerator<KeyState> GetEnumerator ()
	{
		yield return BackspaceState;
		yield return ReturnState;
		yield return SpaceState;
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

	public KeyState GetState (KeyCode key)
    {
        return (KeyState) getField(key).GetValue(this);
    }

    public void SetState (KeyCode key, KeyState newState)
    {
        if (!Locked) getField(key).SetValue(this, newState);
    }

    FieldInfo getField (KeyCode key)
    {
        return this.GetType().GetField($"{key.ToString()}State");
    }
}

public enum KeyStateType
{
    Active, Deactivated, Sticky, Delayed
}

[Serializable]
public class KeyState
{
    public KeyStateType Type;
	public int EnergyLevel;
    public int StickyPressesRemaining;
    public float DelaySeconds;
}
