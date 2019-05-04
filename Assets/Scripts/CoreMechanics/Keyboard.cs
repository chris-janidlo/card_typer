using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
	public KeyState BackspaceState = new KeyState { Type = KeyStateType.Active };
	public KeyState ReturnState = new KeyState { Type = KeyStateType.Active };
	public KeyState SpaceState = new KeyState { Type = KeyStateType.Active };
	public KeyState AState = new KeyState { Type = KeyStateType.Active };
	public KeyState BState = new KeyState { Type = KeyStateType.Active };
	public KeyState CState = new KeyState { Type = KeyStateType.Active };
	public KeyState DState = new KeyState { Type = KeyStateType.Active };
	public KeyState EState = new KeyState { Type = KeyStateType.Active };
	public KeyState FState = new KeyState { Type = KeyStateType.Active };
	public KeyState GState = new KeyState { Type = KeyStateType.Active };
	public KeyState HState = new KeyState { Type = KeyStateType.Active };
	public KeyState IState = new KeyState { Type = KeyStateType.Active };
	public KeyState JState = new KeyState { Type = KeyStateType.Active };
	public KeyState KState = new KeyState { Type = KeyStateType.Active };
	public KeyState LState = new KeyState { Type = KeyStateType.Active };
	public KeyState MState = new KeyState { Type = KeyStateType.Active };
	public KeyState NState = new KeyState { Type = KeyStateType.Active };
	public KeyState OState = new KeyState { Type = KeyStateType.Active };
	public KeyState PState = new KeyState { Type = KeyStateType.Active };
	public KeyState QState = new KeyState { Type = KeyStateType.Active };
	public KeyState RState = new KeyState { Type = KeyStateType.Active };
	public KeyState SState = new KeyState { Type = KeyStateType.Active };
	public KeyState TState = new KeyState { Type = KeyStateType.Active };
	public KeyState UState = new KeyState { Type = KeyStateType.Active };
	public KeyState VState = new KeyState { Type = KeyStateType.Active };
	public KeyState WState = new KeyState { Type = KeyStateType.Active };
	public KeyState XState = new KeyState { Type = KeyStateType.Active };
	public KeyState YState = new KeyState { Type = KeyStateType.Active };
	public KeyState ZState = new KeyState { Type = KeyStateType.Active };


    public KeyState GetState (KeyCode key)
    {
        return (KeyState) getField(key).GetValue(this);
    }

    public void SetState (KeyCode key, KeyState newState)
    {
        getField(key).SetValue(this, newState);
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
    public int StickyPressesRemaining;
    public float DelaySeconds;
}
