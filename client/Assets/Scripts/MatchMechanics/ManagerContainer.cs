using System;
using UnityEngine;
using UnityEngine.Events;
using CTShared;
using crass;

public abstract class ManagerContainer : Singleton<ManagerContainer>
{
	public abstract MatchManager Manager { get; }

	protected void Awake ()
	{
		if (SingletonGetInstance() != null)
		{
			Destroy(SingletonGetInstance().gameObject);
		}

		SingletonSetInstance(this, true);
	}
}