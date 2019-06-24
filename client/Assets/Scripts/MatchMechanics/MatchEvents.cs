using System;
using UnityEngine;
using UnityEngine.Events;
using CTShared;
using crass;

public abstract class MatchEvents : Singleton<MatchEvents>
{
	[Serializable]
	public class AgentEvents
	{
		public UnityEvent OnDeath;
		public IntEvent OnHealthChanged;

		public KeyPressedEvent OnKeyPressed;
		public StringEvent OnAttemptedCast;
	}

    public UnityEvent OnPreTypePhaseStart, OnTypePhaseStart, OnTypePhaseEnd, OnDrawPhaseStart, OnDrawPhaseEnd;
    public FloatEvent OnTypePhaseTick;

	public AgentEvents Player1, Player2;

	void Awake ()
	{
		if (SingletonGetInstance() != null)
		{
			Destroy(SingletonGetInstance().gameObject);
		}

		SingletonSetInstance(this, true);
	}
}

[Serializable]
public class IntEvent : UnityEvent<int> {}

[Serializable]
public class FloatEvent : UnityEvent<float> {}

[Serializable]
public class StringEvent : UnityEvent<string> {}

[Serializable]
public class KeyPressedEvent : UnityEvent<KeyboardKey, KeyStateType> {}
