using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static partial class CardStepBehaviors
{
	public static int CardCount;

	public static List<Func<bool>> StartDrawStepBehaviors = new List<Func<bool>>(), 
								   EndDrawStepBehaviors = new List<Func<bool>>(),
								   StartTypeStepBehaviors = new List<Func<bool>>(),
								   EndTypeStepBehaviors = new List<Func<bool>>();
								   
	public static List<Func<Card, Agent, bool>> CardCastBehaviors = new List<Func<Card, Agent, bool>>();

	public static void StartDrawStep ()
	{
		doStepBehaviors(StartDrawStepBehaviors);
	}

	public static void EndDrawStep ()
	{
		doStepBehaviors(EndDrawStepBehaviors);
	}

	public static void StartTypeStep ()
	{
		CardCount = 0;
		doStepBehaviors(StartTypeStepBehaviors);
	}

	public static void EndTypeStep ()
	{
		CardCount = 0;
		doStepBehaviors(EndTypeStepBehaviors);
	}

	public static void OnCast (Card card, Agent caster)
	{
		doStepBehaviors(CardCastBehaviors.Select<Func<Card, Agent, bool>, Func<bool>>(f => () => f(card, caster)).ToList());
	}

	static void doStepBehaviors (List<Func<bool>> list)
	{
		List<int> indicesToRemove = new List<int>();

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i]()) indicesToRemove.Add(i);
		}

		foreach (int i in indicesToRemove.OrderByDescending(v => v))
		{
			list.RemoveAt(i);
		}
	}
}
