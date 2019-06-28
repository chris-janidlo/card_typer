using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;
using TMPro;

public class AgentHealth : MonoBehaviour
{
	public TextMeshProUGUI HealthDisplay;
	public float HealthUpdateTime;

	int currentHealthDisplay, targetHealthDisplay, oldHealthDisplay;

	public void Initialize (Agent agent)
	{
		targetHealthDisplay = Agent.StartingMaxHealth;

		agent.OnHealthChanged += changeHealth;
	}

	void Update ()
	{
		HealthDisplay.text = currentHealthDisplay.ToString();
	}

	void changeHealth (int newHealth)
	{
		oldHealthDisplay = targetHealthDisplay;
		targetHealthDisplay += newHealth;

		currentHealthDisplay = oldHealthDisplay;

		StopAllCoroutines();
		StartCoroutine(healthUpdateRoutine());
	}

	IEnumerator healthUpdateRoutine ()
	{
		float timer = 0;

		while (timer < HealthUpdateTime)
		{
			currentHealthDisplay = (int) Mathf.Lerp(oldHealthDisplay, targetHealthDisplay, timer / HealthUpdateTime);

			timer -= Time.deltaTime;
			yield return null;
		}

		currentHealthDisplay = targetHealthDisplay;
	}
}
