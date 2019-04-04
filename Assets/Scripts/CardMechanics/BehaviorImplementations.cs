using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static partial class CardBehavior
{
	public static Dictionary<string, Action<Card, Agent, Agent>> CardBehaviors = new Dictionary<string, Action<Card, Agent, Agent>>()
	{
		{ "two", (c, p, e) => {
			CardCastBehaviors.Add((cast, a) => {
				if (a == p && cast.PartOfSpeech.Equals("adjective"))
				{
					DoCardBehavior(cast, e, p);
				}
				return true;
			});
		}},

		{ "faced", (c, p, e) => {
			int average = (p.Lux + p.Nox) / 2;
			p.Lux = average;
			p.Nox = average;
		}},

		{ "flaming", (c, p, e) => {
			var damageCards = new string[] { "sword", "abhorred" };
			CardCastBehaviors.Add((cast, a) => {
				if (a == p && damageCards.Contains(cast.Word))
				{
					e.IncrementHealth(-p.Lux / 2);
				}
				return true;
			});
		}},

		{ "sword", (c, p, e) => {
			e.IncrementHealth(-3);
		}},

		{ "prophets", (c, p, e) => {
			p.HandSize++;
			EndDrawStepBehaviors.Add(() => {
				p.HandSize--;
				return true;
			});
		}},

		{ "priest", (c, p, e) => {
			p.IncrementHealth(p.Lux);
			p.Lux /= 2;
		}},

		{ "zealot", (c, p, e) => {
			p.Nox += p.Lux;
			p.Lux = 0;
		}},

		{ "abhorred", (c, p, e) => {
			e.IncrementHealth(-p.Nox);
		}},

		{ "anchorage", (c, p, e) => {
			p.EssenceLock = true;
			EndTypeStepBehaviors.Add(() => {
				p.EssenceLock = false;
				return true;
			});
		}},

		{ "locked", (c, p, e) => {
			p.EssenceLock = true;
			CardCastBehaviors.Add((card, a) => {
				if (a == p)
				{
					p.EssenceLock = false;
				}
				return true;
			});
		}},

		{ "barrier", (c, p, e) => {
			p.Shield = p.Nox / 2;
		}},

		{ "Grim", (c, p, e) => {
			p.Nox += 10;
		}},

		{ "bulwarked", (c, p, e) => {
			p.Shield = p.Lux;
		}},

		{ "devised", (c, p, e) => {
			e.IncrementHealth(-CardCount);
		}}
	};
}