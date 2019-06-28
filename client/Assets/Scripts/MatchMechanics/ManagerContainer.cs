using System;
using UnityEngine;
using UnityEngine.Events;
using CTShared;
using crass;

public abstract class ManagerContainer : Singleton<ManagerContainer>
{
	protected MatchManager manager;
	public static MatchManager Manager => Instance.manager;

	protected void Awake ()
	{
		if (SingletonGetInstance() != null)
		{
			Destroy(SingletonGetInstance().gameObject);
		}

		SingletonSetInstance(this, true);
	}
}