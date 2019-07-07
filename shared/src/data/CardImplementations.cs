using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CTShared.Cards
{

using Keys = List<KeyboardKey>;

public static class CardUtils
{
    public static void DoAfterTime (MatchManager manager, Action action, float time)
    {
		Action<float> onTick = null;
		Action unsubscribe = null;
        float timer = time;

        onTick = dt => {
            timer -= dt;
            if (timer <= 0)
            {
                action();
                unsubscribe();
            }
        };

        unsubscribe = () => {
            manager.OnTypePhaseTick -= onTick;
            manager.OnTypePhaseEnd -= unsubscribe;
        };

        manager.OnTypePhaseTick += onTick;
        manager.OnTypePhaseEnd += unsubscribe;
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

	protected override void behaviorImplementation (Agent caster)
	{
		Action<int> reflector = d =>
		{
			if (d >= 0) return;
			var enemy = manager.GetEnemyOf(caster);
			enemy.IncrementHealth((int) (d * reflectMult));
		};

        caster.OnHealthChanged += reflector;
        CardUtils.DoAfterTime(manager, () => caster.OnHealthChanged -= reflector, reflectTime);
	}
}

public class Anchorage : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a port; a source of assurance; a dwelling place of a religious refuse";
	public override string EffectText => $"lock your keyboard to its current state for the next {lockTime} seconds";

	public override int Burn => 4;

    int lockTime = 3;

	protected override void behaviorImplementation (Agent caster)
	{
        caster.Keyboard.Locked = true;
        CardUtils.DoAfterTime(manager, () => caster.Keyboard.Locked = false, lockTime);
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
        KeyboardKey.B,
        KeyboardKey.D,
        KeyboardKey.G,
        KeyboardKey.H,
        KeyboardKey.K,
        KeyboardKey.L,
        KeyboardKey.M,
        KeyboardKey.N,
        KeyboardKey.P,
        KeyboardKey.Q,
        KeyboardKey.R,
        KeyboardKey.S,
        KeyboardKey.T,
        KeyboardKey.W,
        KeyboardKey.Y,
        KeyboardKey.Z,
    };
	int energy = 1;

	protected override void behaviorImplementation (Agent caster)
	{
		foreach (var key in keys)
        {
            caster.Keyboard[key].EnergyLevel += energy;
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
		KeyboardKey.Q,
		KeyboardKey.W,
		KeyboardKey.E,
		KeyboardKey.R,
		KeyboardKey.T,
		KeyboardKey.Y,
		KeyboardKey.U,
		KeyboardKey.I,
		KeyboardKey.O,
		KeyboardKey.P
	};

	protected override void behaviorImplementation (Agent caster)
	{
		foreach (var key in keys)
		{
			caster.Shield += (int) (shield * caster.Keyboard[key].EnergyLevel);
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
		KeyboardKey.A,
		KeyboardKey.S,
		KeyboardKey.D,
		KeyboardKey.F,
		KeyboardKey.G,
		KeyboardKey.H,
		KeyboardKey.J,
		KeyboardKey.K,
		KeyboardKey.L
	};

	protected override void behaviorImplementation (Agent caster)
	{
		float total = 0;
		foreach (var key in keys)
		{
			total += caster.Keyboard[key].EnergyLevel * damage;
		}
		manager.GetEnemyOf(caster).IncrementHealth(-(int) total);
	}
}

public class Devise : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "plan or contrive in the mind";
	public override string EffectText => $"deal {damagePerCard} damage for every spell cast so far this turn";

	public override int Burn => 7;

	int damagePerCard = 3;

	protected override void behaviorImplementation (Agent caster)
	{
		manager.GetEnemyOf(caster).IncrementHealth(-manager.CardsCastedThisTurn * damagePerCard);
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
	
	protected override void behaviorImplementation (Agent caster)
	{
		Card.CastEvent burn = null;
		Action unsubscribe = null;

		unsubscribe = () => {
			BeforeCast -= burn;
			manager.OnTypePhaseEnd -= unsubscribe;
		};

		burn = (card, agent) => {
			if (agent == caster && card.PartOfSpeech.Equals(effectedPartOfSpeech))
			{
				manager.GetEnemyOf(caster).IncrementHealth(-damage);
			}
			unsubscribe();
		};

		BeforeCast += burn;
		manager.OnTypePhaseEnd += unsubscribe;
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
		KeyboardKey.A,
		KeyboardKey.D,
		KeyboardKey.M,
		KeyboardKey.S,
		KeyboardKey.F,
		KeyboardKey.I,
		KeyboardKey.L,
		KeyboardKey.Y
	};

	protected override void behaviorImplementation (Agent caster)
	{
		foreach (var key in keys)
		{
			caster.Keyboard[key].EnergyLevel += energyGain;
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
		KeyboardKey.A,
		KeyboardKey.E,
		KeyboardKey.I,
		KeyboardKey.O,
		KeyboardKey.U,
		KeyboardKey.Y
	};

	protected override void behaviorImplementation (Agent caster)
	{
		foreach (var key in keys)
		{
			var state = caster.Keyboard[key];
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

	protected override void behaviorImplementation (Agent caster)
	{
		manager.GetEnemyOf(caster).IncrementHealth((int) (-damage * manager.TypingTimeLeftPercent));
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
		AfterCast += (casted, agent) => {
			lastCast = casted;
			lastCaster = agent;
		};
	}

	protected override void behaviorImplementation (Agent caster)
	{
		if (lastCast == null) return; // this is the first spell cast this turn

		// capture the relevant information from lastCast and lastCaster so that (when its time to disable) we disable what they were when we cast the spell, not what they are `time` seconds after casting it
		Keys keys = lastCast.Word.Select(c => c.ToKeyboardKey()).ToList();
		Agent lastCasterCapture = lastCaster;

		Action<bool> setActiveState = flag => {
			foreach (var key in keys)
			{
				lastCasterCapture.Keyboard[key].Type = flag ? KeyStateType.Active : KeyStateType.Deactivated;
			}
		};

		setActiveState(false);
		CardUtils.DoAfterTime(manager, () => setActiveState(true), time);
	}
}

public class Priest : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one especially consecrated to the service of divinity";
	public override string EffectText => $"clear {healed} random status effect{(healed == 1 ? "" : "s")} from your keyboard";

	public override int Burn => 5;

	int healed = 1;

	Random rand;

	protected override void initialize ()
	{
		rand = new Random();
	}

	protected override void behaviorImplementation (Agent caster)
	{
		for (int i = 0; i < healed; i++)
		{
			var illKeys = caster.Keyboard.Where(s => s.Type != KeyStateType.Active).ToList();

			if (illKeys.Count == 0) return;

			illKeys[rand.Next(illKeys.Count)].Type = KeyStateType.Active;
		}
	}
}

public class Prince : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "of a royal family, a nonreigning male member";
	public override string EffectText => $"disable one of the {keys.ToNaturalString(true)} keys on your opponent's keyboard";

	public override int Burn => 5;

	Keys keys = new Keys {
		KeyboardKey.P,
		KeyboardKey.R,
		KeyboardKey.I,
		KeyboardKey.N,
		KeyboardKey.C,
		KeyboardKey.E
	};

	Random rand;

	protected override void initialize ()
	{
		rand = new Random();
	}

	protected override void behaviorImplementation (Agent caster)
	{
		var enemy = manager.GetEnemyOf(caster);
		var key = keys[rand.Next(keys.Count)];

		enemy.Keyboard[key].Type = KeyStateType.Deactivated;
	}
}

public class Prophet : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "spokesperson";
	public override string EffectText => $"draw {extraDraw} extra card{(extraDraw > 1 ? "s" : "")} next turn";

	public override int Burn => 5;

	int extraDraw = 1;

	protected override void behaviorImplementation (Agent caster)
	{
		caster.HandSize += extraDraw;
		
		Action reducer = null;
		reducer = () => {
			caster.HandSize -= extraDraw;
			manager.OnDrawPhaseEnd -= reducer;
		};
		manager.OnDrawPhaseEnd += reducer;
	}
}

public class Refuse : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "decline to accept; express determination to not do something";
	public override string EffectText => $"the next spell casted by you or your opponent loses all of its effects; instead of its regular effects, it deals damage equal to its length times {damageMult}";

	public override int Burn => 10;

	int damageMult = 2;

	protected override void behaviorImplementation (Agent caster)
	{
		CastEvent beforeCast = null, afterCast = null;
		int length = 0;

		beforeCast = (card, agent) => {
			length = card.Word.Length;
			CastLock = true;
			BeforeCast -= beforeCast;
		};

		afterCast = (card, agent) => {
			manager.GetEnemyOf(caster).IncrementHealth(-length * damageMult);
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
	public override string EffectText => $"if there is less than {time} second{(time == 1 ? "" : "s")} left: deal damage based on your typing accuracy, for a maximum of {maxDamage} (this damage is at its maximum at 100% accuracy, but quickly drops off as your accuracy decreases)";

	public override int Burn => 10;

	float time = 1;
	float maxDamage = 12;

	protected override void behaviorImplementation (Agent caster)
	{
		if (manager.TypingTimer <= time)
		{
			float damage = (float) Math.Pow(maxDamage, caster.AccuracyThisTurn);
			manager.GetEnemyOf(caster).IncrementHealth(-(int) damage);
		}
	}
}

public class Sword : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "military power, violence, destruction, one of the tarot suits";
	public override string EffectText => $"deal {damage} damage";

	public override int Burn => 3;

	int damage = 3;

	protected override void behaviorImplementation (Agent caster)
	{
		manager.GetEnemyOf(caster).IncrementHealth(-damage);
	}
}

public class TwoFaced : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "deceitful";
	public override string EffectText => $"disable the space bar on both your and your opponent's keyboard";

    public override int Burn => 1;

	protected override void behaviorImplementation (Agent caster)
	{
		caster.Keyboard[KeyboardKey.Space].Type = KeyStateType.Deactivated;
		manager.GetEnemyOf(caster).Keyboard[KeyboardKey.Space].Type = KeyStateType.Deactivated;
	}
}

public class Unveil : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "to make something clear";
	public override string EffectText => $"clear the status effects of any key on your keyboard that is touching at least {healEnergy} energy";

	public override int Burn => 5;

	int healEnergy = 4;

	protected override void behaviorImplementation (Agent caster)
	{
		foreach (var state in caster.Keyboard.Where(s => s.Type != KeyStateType.Active))
		{
			if (caster.Keyboard.GetSurroundingKeys(state).Sum(s => s.EnergyLevel) >= healEnergy)
			{
				state.Type = KeyStateType.Active;
			}
		}
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

	protected override void behaviorImplementation (Agent caster)
	{
		if (manager.CardsCastedThisTurn < minCast) return;

		float elapsed = 1 - manager.TypingTimeLeftPercent;
		manager.GetEnemyOf(caster).IncrementHealth((int) (-maxDamage * elapsed));
	}
}

public class Year : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a period of approximately the same length in other calendars";
	public override string EffectText => $"deal {damageMult} damage per energy in your {keys.ToNaturalString()} keys";

	public override int Burn => 7;

	float damageMult = 0.5f;
	Keys keys = new Keys {
		KeyboardKey.N,
		KeyboardKey.U,
		KeyboardKey.M,
		KeyboardKey.B,
		KeyboardKey.E,
		KeyboardKey.R
	};

	protected override void behaviorImplementation (Agent caster)
	{
		int total = 0;
		foreach (var key in keys)
		{
			total += caster.Keyboard[key].EnergyLevel;
		}
		manager.GetEnemyOf(caster).IncrementHealth((int) (damageMult * -total));
	}
}

public class Zealot : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one marked by fervant partisanship";
	public override string EffectText => $"concentrate all of your keyboard's energy into the last key you typed before casting this spell";

	public override int Burn => 2;

	Dictionary<Agent, KeyboardKey> lastLettersTyped = new Dictionary<Agent, KeyboardKey>();

	protected override void initialize ()
	{
		AfterCast += (card, agent) => {
			var key = card.Word[card.Word.Length - 1].ToKeyboardKey();
			lastLettersTyped[agent] = key;
		};

		manager.OnTypePhaseStart += () => {
			lastLettersTyped = new Dictionary<Agent, KeyboardKey>();
		};
	}

	protected override void behaviorImplementation (Agent caster)
	{
		int total = 0;

		foreach (var state in caster.Keyboard)
		{
			total += state.EnergyLevel;
			state.EnergyLevel = 0;
		}

		KeyboardKey key;
		if (lastLettersTyped.TryGetValue(caster, out key))
		{
			caster.Keyboard[key].EnergyLevel = total;
		}
	}
}
}