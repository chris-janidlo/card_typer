using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

namespace Cards1492
{

using Keys = List<KeyCode>;

public static class CardUtils
{
    public static void DoAfterTime (Action action, float time)
    {
		Action onTick = null, unsubscribe = null;
        float timer = time;

        onTick = () => {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                action();
                unsubscribe();
            }
        };

        unsubscribe = () => {
            MatchManager.Instance.OnTypePhaseTick -= onTick;
            MatchManager.Instance.OnTypePhaseEnd -= unsubscribe;
        };

        MatchManager.Instance.OnTypePhaseTick += onTick;
        MatchManager.Instance.OnTypePhaseEnd += unsubscribe;
    }
}

public class Abhor : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "detest; loathe; abominate; despise; hate";
	public override string EffectText => $"for the next {reflectTime} seconds: any time you take any damage, deal {reflectMult} times the amount of damage you took to your opponent";

	public override int Burn => 5;

	float reflectTime = 3;
    float reflectMult = .9f;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		Action<int> reflector = d => enemy.IncrementHealth((int) (d * reflectMult));
        caster.OnDamage += reflector;
        CardUtils.DoAfterTime(() => caster.OnDamage -= reflector, reflectTime);
	}
}

public class Anchorage : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a port; a source of assurance; a dwelling place of a religious refuse";
	public override string EffectText => $"lock your keyboard to its current state for the next {lockTime} seconds";

	public override int Burn => 4;

    int lockTime = 3;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
        caster.Typer.Keyboard.Locked = true;
        CardUtils.DoAfterTime(() => caster.Typer.Keyboard.Locked = false, lockTime);
	}
}

public class Ancient : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "having the qualities of age; old-fashioned; antique";
	public override string EffectText => $"your {keys.ToNaturalString()} keys each gain {energy} energy";

	public override int Burn => 5;

    // Phoenician alphabet
    Keys keys = new Keys {
        KeyCode.B,
        KeyCode.D,
        KeyCode.G,
        KeyCode.H,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.W,
        KeyCode.Y,
        KeyCode.Z,
    };
	int energy = 1;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		foreach (var key in keys)
        {
            caster.Typer.Keyboard.GetState(key).EnergyLevel += energy;
        }
	}
}

public class Barrier : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a circumstance or obstacle that keeps people apart or prevents progress";
	public override string EffectText => $"gain {shield} per energy on the top row ({keys.ToNaturalString()})";

	public override int Burn => 3;

	float shield = 1f;
	Keys keys = new Keys {
		KeyCode.Q,
		KeyCode.W,
		KeyCode.E,
		KeyCode.R,
		KeyCode.T,
		KeyCode.Y,
		KeyCode.U,
		KeyCode.I,
		KeyCode.O,
		KeyCode.P
	};

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		foreach (var key in keys)
		{
			caster.Shield += (int) (shield * caster.Typer.Keyboard.GetState(key).EnergyLevel);
		}
	}
}

public class Bulwark : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a strong support";
	public override string EffectText => $"deal {damage} damage per energy on the home row ({keys.ToNaturalString()})";

	public override int Burn => 4;

	float damage = 1;
	Keys keys = new Keys {
		KeyCode.A,
		KeyCode.S,
		KeyCode.D,
		KeyCode.F,
		KeyCode.G,
		KeyCode.H,
		KeyCode.J,
		KeyCode.K,
		KeyCode.L
	};

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		float total = 0;
		foreach (var key in keys)
		{
			total += caster.Typer.Keyboard.GetState(key).EnergyLevel * damage;
		}
		enemy.IncrementHealth(-(int) total);
	}
}

public class Devise : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "plan or contrive in the mind";
	public override string EffectText => $"deal {damagePerCard} damage for every spell cast so far this turn";

	public override int Burn => 7;

	int damagePerCard = 3;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-ITyper.CardsCastedSinceTurnStart * damagePerCard);
	}
}

public class Flaming : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "passionate, violent, used as an intensifier";
	public override string EffectText => $"the next time you cast a spell this turn: if it's {anA} {effectedPartOfSpeech}, deal {damage} damage; otherwise, this does nothing";

	public override int Burn => 7;

	string anA = "a";
	string effectedPartOfSpeech = "noun";
	int damage = 1;
	
	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		Action<Card, Agent> burn = null;
		Action unsubscribe = null;

		unsubscribe = () => {
			BeforeCast -= burn;
			MatchManager.Instance.OnTypePhaseEnd -= unsubscribe;
		};

		burn = (card, agent) => {
			if (agent == caster && card.PartOfSpeech.Equals(effectedPartOfSpeech))
			{
				enemy.IncrementHealth(-damage);
			}
			unsubscribe();
		};

		BeforeCast += burn;
		MatchManager.Instance.OnTypePhaseEnd += unsubscribe;
	}
}

public class Grim : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "lacking genuine levity";
	public override string EffectText => $"your {keys.ToNaturalString()} each gain {energyGain} energy";

	public override int Burn => 5;

	int energyGain = 1;
	Keys keys = new Keys {
		KeyCode.A,
		KeyCode.D,
		KeyCode.M,
		KeyCode.S,
		KeyCode.F,
		KeyCode.I,
		KeyCode.L,
		KeyCode.Y
	};

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		foreach (var key in keys)
		{
			caster.Typer.Keyboard.GetState(key).EnergyLevel += energyGain;
		}
	}
}

public class Heart : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "courage or enthusiasm";
	public override string EffectText => $"heal {healthGain} health per energy on your {keys.ToNaturalString()} keys; each of those keys loses all of its energy";

	public override int Burn => 6;

	int healthGain = 2;
	Keys keys = new Keys {
		KeyCode.A,
		KeyCode.E,
		KeyCode.I,
		KeyCode.O,
		KeyCode.U,
		KeyCode.Y
	};

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		foreach (var key in keys)
		{
			var state = caster.Typer.Keyboard.GetState(key);
			caster.IncrementHealth(state.EnergyLevel);
			state.EnergyLevel = 0;
		}
	}
}

public class Hound : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "pursue tenaciously, doglike";
	public override string EffectText => $"deal {damage} damage times the percentage of typing time remaining after casting this";

	public override int Burn => 5;

	public int damage = 1;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth((int) (-damage * MatchManager.Instance.TypingTimeLeftPercent));
	}
}

public class Lock : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "fasten by a key or combination";
	public override string EffectText => $"for {time} seconds, disable every key that was used to type the last spell that was cast (from either side!)";

	public override int Burn => 7;

	float time = 4;
	Card lastCast;
	Agent lastCaster;

	protected override void initialize ()
	{
		BeforeCast += (casted, agent) => {
			if (casted.GetType() != GetType())
			{
				lastCast = casted;
				lastCaster = agent;
			}
		};
	}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		// capture the relevant information from lastCast and lastCaster so that (when its time to disable) we disable what they were when we cast the spell, not what they are `time` seconds after casting it
		Keys keys = lastCast.Word.Select(c => c.ToKeyCode()).ToList();
		Agent lastCasterCapture = lastCaster;

		Action<bool> setActiveState = flag => {
			foreach (var key in keys)
			{
				lastCasterCapture.Typer.Keyboard.GetState(key).Type = flag ? KeyStateType.Active : KeyStateType.Deactivated;
			}
		};

		setActiveState(false);
		CardUtils.DoAfterTime(() => setActiveState(true), time);
	}
}

public class Priest : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one especially consecrated to the service of divinity";
	// TODO: format percentage as a percentage
	public override string EffectText => $"if there is less than {percent} of the typing time left, clear all status effects from your keyboard";

	public override int Burn => 5;

	float percent = 0.1f;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		if (MatchManager.Instance.TypingTimeLeftPercent <= percent)
		{
			foreach (var state in caster.Typer.Keyboard)
			{
				state.Type = KeyStateType.Active;
			}
		}
	}
}

public class Prince : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "of a royal family, a nonreigning male member";
	public override string EffectText => $"disable one of the {keys.ToNaturalString(true)} keys on your opponent's keyboard";

	public override int Burn => 5;

	int luxGain = 4;
	Keys keys = new Keys {
		KeyCode.P,
		KeyCode.R,
		KeyCode.I,
		KeyCode.N,
		KeyCode.C,
		KeyCode.E
	};

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.Typer.Keyboard.GetState(keys.PickRandom()).Type = KeyStateType.Deactivated;
	}
}

public class Prophet : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "spokesperson";
	public override string EffectText => $"draw {extraDraw} extra card{(extraDraw > 1 ? "s" : "")} next turn";

	public override int Burn => 5;

	int extraDraw = 1;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Drawer.HandSize += extraDraw;
		
		Action reducer = null;
		reducer = () => {
			caster.Drawer.HandSize -= extraDraw;
			MatchManager.Instance.OnDrawPhaseEnd -= reducer;
		};
		MatchManager.Instance.OnDrawPhaseEnd += reducer;
	}
}

public class Refuse : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "decline to accept; express determination to not do something";
	public override string EffectText => $"the next spell casted by you or your opponent loses all of its effects; instead of its regular effects, it deals damage equal to its length times {damageMult}";

	public override int Burn => 10;

	int damageMult = 2;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		Action<Card, Agent> beforeCast = null, afterCast = null;
		int length = 0;

		beforeCast = (card, agent) => {
			length = card.Word.Length;
			CastLock = true;
			BeforeCast -= beforeCast;
		};

		afterCast = (card, agent) => {
			enemy.IncrementHealth(-length * damageMult);
			CastLock = false;
			AfterCast -= afterCast;
		};

		BeforeCast += beforeCast;
		AfterCast += afterCast;
	}
}

public class Sunset : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "the apparent descent of the sun";
	public override string EffectText => $"gain {luxPerCard} lux per spell casted so far this turn";

	public override int Burn => 10;

	int luxPerCard = 1;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Lux += Typer.Instance.CardsCasted * luxPerCard;
	}
}

public class Sword : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "military power, violence, destruction, one of the tarot suits";
	public override string EffectText => $"deal {damage} damage";

	public override int Burn => 3;

	int damage = 3;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-damage, caster.SubjectName, "stabbed");
	}
}

public class TwoFaced : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "deceitful";
	public override string EffectText => $"you and your opponent each gain {luxGain} lux";

    public override int Burn => 1;

	int luxGain = 3;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Lux += luxGain;
		enemy.Lux += luxGain;
	}
}

public class Unveil : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "to make something clear";
	public override string EffectText => $"gain {luxPerNox} lux per nox";

	public override int Burn => 5;

	float luxPerNox = 0.5f;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Lux += (int) (luxPerNox * caster.Nox);
	}
}

public class Weary : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "tired";
	public override string EffectText => $"if you've casted at least {minCast} spells this turn, deal {maxDamage} damage multiplied by the percent of the timer that has elapsed before casting this";

	public override int Burn => 20;

	int minCast = 3;
	int maxDamage = 10;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		if (Typer.Instance.CardsCasted < minCast) return;

		float elapsed = 1 - Typer.Instance.TimeLeftPercent;
		enemy.IncrementHealth((int) (-maxDamage * elapsed), caster.SubjectName, "incapacitated");
	}
}

public class Year : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a period of approximately the same length in other calendars";
	public override string EffectText => $"deal {damagePerLux} damage per lux multiplied by {damagePerCard} damage per spell casted this turn";

	public override int Burn => 7;

	float damagePerLux = 0.5f;
	float damagePerCard = 1;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		float lux = caster.Lux * damagePerLux;
		float card = Typer.Instance.CardsCasted * damagePerCard;
		if (lux * card == 0)
		{
			EventBox.Log(" But nothing happened...");
		}
		else
		{
			enemy.IncrementHealth((int) (-lux * card), caster.SubjectName, "hurt");
		}
	}
}

public class Zealot : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one marked by fervant partisanship";
	public override string EffectText => $"gain {noxPerLux} nox per lux; lose all your lux";

	public override int Burn => 2;

	float noxPerLux = 1;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Nox += (int) (caster.Lux * noxPerLux);
		caster.Lux = 0;
	}
}
}