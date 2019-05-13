using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
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
#endregion

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
