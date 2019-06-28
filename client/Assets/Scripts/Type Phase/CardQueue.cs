using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;
using TMPro;

public class CardQueue : MonoBehaviour
{
	public TextMeshProUGUI DisplayPrefab;
	public RectTransform DisplayContainer;

	List<TextMeshProUGUI> displays = new List<TextMeshProUGUI>();

	public void Initialize (Agent agent)
	{
		agent.OnAttemptedCast += castCard;
		ManagerContainer.Manager.OnTypePhaseEnd += clearQueue;
	}

	public void SetPlay (List<Card> play)
	{
		foreach (var card in play)
		{
			var display = Instantiate(DisplayPrefab);
			display.transform.SetParent(DisplayContainer, false);
			displays.Add(display);
		}
	}

	void castCard (string name)
	{
		if (name.Equals("")) return;

		var display = displays.First(d => d.text.Equals(name));
		displays.Remove(display);
		Destroy(display.gameObject);
	}

	void clearQueue ()
	{
		foreach (var disp in displays)
		{
			Destroy(disp.gameObject);
		}

		displays.Clear();
	}
}
