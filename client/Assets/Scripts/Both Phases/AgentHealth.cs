using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;
using TMPro;

public class AgentHealth : MonoBehaviour
{
	public TextMeshProUGUI HealthDisplay;
	public TextMeshProUGUI ShieldDisplay;
	public float HealthUpdateTime;

	int currentHealthAmountDisplay, targetHealthAmount, oldHealthAmount;
	Agent agent;

	public void Initialize (Agent agent)
	{
		this.agent = agent;

		targetHealthAmount = Agent.StartingMaxHealth;
		currentHealthAmountDisplay = targetHealthAmount;

		agent.OnHealthChanged += changeHealth;
	}

	void Update ()
	{
		HealthDisplay.text = currentHealthAmountDisplay.ToString();
		ShieldDisplay.text = agent.Shield.ToString();
	}

	void changeHealth (int healthDelta)
	{
		oldHealthAmount = targetHealthAmount;
		targetHealthAmount += healthDelta;

		currentHealthAmountDisplay = oldHealthAmount;

		StopAllCoroutines();
		StartCoroutine(healthUpdateRoutine());
	}

	IEnumerator healthUpdateRoutine ()
	{
		float timer = 0;

		while (timer < HealthUpdateTime)
		{
			currentHealthAmountDisplay = (int) Mathf.Lerp(oldHealthAmount, targetHealthAmount, timer / HealthUpdateTime);

			timer += Time.deltaTime;
			yield return null;
		}

		currentHealthAmountDisplay = targetHealthAmount;
	}
}
