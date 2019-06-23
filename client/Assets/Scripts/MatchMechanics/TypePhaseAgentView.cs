using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class TypePhaseAgentView : MonoBehaviour
{
	Agent agent;

	void Start ()
	{
		var mgr = ManagerContainer.Manager;

		mgr.OnTypePhaseStart += startPhase;
		mgr.OnTypePhaseEnd += endPhase;
	}

	void Update ()
	{

	}

	public void Initialize (Agent _agent)
	{
		agent = _agent;

		agent.on
	}

	void startPhase ()
	{

	}

	void endPhase ()
	{

	}
}
