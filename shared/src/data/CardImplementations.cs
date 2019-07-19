using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LiteNetLib.Utils;

namespace CTShared.Cards
{

using Keys = List<KeyboardKey>;

internal class Abhor : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "detest; loathe; abominate; despise; hate";
	public override string EffectText => $"for the next {reflectTime} seconds: any time you take any damage, deal {reflectMult} times the amount of damage you took to your opponent";

	public override int Burn => 5;

	// stats
	float reflectTime = 3;
    float reflectMult = .9f;

	// state
	float timeLeft;

	internal override void Deserialize (NetDataReader reader)
	{
		timeLeft = reader.GetFloat();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(timeLeft);
	}

	protected override void behaviorImplementation ()
	{
		timeLeft = reflectTime;
	}

	internal override void OnAgentHealthChanged (Agent agent, int delta)
	{
		if (timeLeft <= 0 || agent != Owner || delta >= 0) return;

		var enemy = manager.GetEnemyOf(agent);
		enemy.IncrementHealth((int) (delta * reflectMult));
	}

	internal override void OnTypePhaseTick (float dt)
	{
		timeLeft -= dt;
	}
}

internal class Anchorage : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a port; a source of assurance; a dwelling place of a religious refuse";
	public override string EffectText => $"lock your keyboard to its current state for the next {lockTime} seconds";

	public override int Burn => 4;

	// stats
    int lockTime = 3;

	// state
	float timeLeft;
	bool shouldUnlock;

	internal override void Deserialize (NetDataReader reader)
	{
		timeLeft = reader.GetFloat();
		shouldUnlock = reader.GetBool();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(timeLeft);
		writer.Put(shouldUnlock);
	}

	protected override void behaviorImplementation ()
	{
		if (!Owner.Keyboard.Locked)
		{
			timeLeft = lockTime;
			shouldUnlock = true;
		}
	}

	internal override void OnTypePhaseTick (float dt)
	{
		timeLeft -= dt;

		// allow any number of other timed locks as long as they also follow this pattern
		if (timeLeft > 0)
		{
			Owner.Keyboard.Locked = true;
		}
		else if (shouldUnlock)
		{
			Owner.Keyboard.Locked = false;
			shouldUnlock = false;
		}
	}
}

internal class Ancient : Card
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

	protected override void behaviorImplementation ()
	{
		foreach (var key in keys)
        {
            Owner.Keyboard[key].EnergyLevel += energy;
        }
	}
}

internal class Barrier : Card
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

	protected override void behaviorImplementation ()
	{
		foreach (var key in keys)
		{
			Owner.Shield += (int) (shield * Owner.Keyboard[key].EnergyLevel);
		}
	}
}

internal class Bulwark : Card
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

	protected override void behaviorImplementation ()
	{
		float total = 0;
		foreach (var key in keys)
		{
			total += Owner.Keyboard[key].EnergyLevel * damage;
		}
		manager.GetEnemyOf(Owner).IncrementHealth(-(int) total);
	}
}

internal class Devise : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "plan or contrive in the mind";
	public override string EffectText => $"deal {damagePerCard} damage for every spell cast so far this turn";

	public override int Burn => 7;

	int damagePerCard = 3;

	protected override void behaviorImplementation ()
	{
		manager.GetEnemyOf(Owner).IncrementHealth(-manager.CardsCastedThisTurn * damagePerCard);
	}
}

internal class Flaming : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "passionate, violent, used as an intensifier";
	public override string EffectText => $"the next time you cast a spell this turn: if it's {anA} {effectedPartOfSpeech}, deal {damage} damage; otherwise, this does nothing";

	public override int Burn => 7;


	// stats
	string anA = "a";
	string effectedPartOfSpeech = "noun";
	int damage = 1;

	// state
	bool primed;

	internal override void Deserialize (NetDataReader reader)
	{
		primed = reader.GetBool();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(primed);
	}
	
	protected override void behaviorImplementation ()
	{
		primed = true;
	}

	internal override void BeforeCardCast (Card card, Agent caster)
	{
		if (caster == Owner)
		{
			if (primed && card.PartOfSpeech.Equals(effectedPartOfSpeech))
			{
				manager.GetEnemyOf(Owner).IncrementHealth(-damage);
			}
			primed = false;
		}
	}

	internal override void OnTypePhaseEnd ()
	{
		primed = false;
	}
}

internal class Grim : Card
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

	protected override void behaviorImplementation ()
	{
		foreach (var key in keys)
		{
			Owner.Keyboard[key].EnergyLevel += energyGain;
		}
	}
}

internal class Heart : Card
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

	protected override void behaviorImplementation ()
	{
		foreach (var key in keys)
		{
			var state = Owner.Keyboard[key];
			Owner.IncrementHealth(state.EnergyLevel);
			state.EnergyLevel = 0;
		}
	}
}

internal class Hound : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "pursue tenaciously, doglike";
	public override string EffectText => $"deal {damage} damage times the percentage of typing time remaining after casting this";

	public override int Burn => 5;

	public int damage = 10;

	protected override void behaviorImplementation ()
	{
		manager.GetEnemyOf(Owner).IncrementHealth((int) (-damage * manager.TypingTimeLeftPercent));
	}
}

internal class Lock : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "fasten by a key or combination";
	public override string EffectText => $"for {time} seconds, disable every key that was used to type the last spell that was cast (from either side!)";

	public override int Burn => 7;

	// stats
	float time = 4;

	// state
	string lastCastWord;
	Agent lastCaster;
	float timer;
	bool shouldReenable;

	internal override void Deserialize (NetDataReader reader)
	{
		lastCastWord = reader.GetString();
		timer = reader.GetFloat();
		shouldReenable = reader.GetBool();
		bool wasMe = reader.GetBool();
		if (wasMe)
		{
			lastCaster = Owner;
		}
		else
		{
			lastCaster = manager.GetEnemyOf(Owner);
		}
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(lastCastWord);
		writer.Put(timer);
		writer.Put(shouldReenable);
		writer.Put(lastCaster == Owner);
	}

	protected override void behaviorImplementation ()
	{
		if (lastCastWord == null) return; // this is the first spell cast this turn
		if (timer > 0) return; // we're currently disabling things, and don't want to replace the current owner/word state

		setActiveState(false);
		timer = time;
		shouldReenable = true;
	}

	void setActiveState (bool value)
	{
		Keys keys = lastCastWord.Select(c => c.ToKeyboardKey()).ToList();
		foreach (var key in keys)
		{
			lastCaster.Keyboard[key].Type = value ? KeyStateType.Active : KeyStateType.Deactivated;
		}
	}

	internal override void AfterCardCast (Card card, Agent caster)
	{
		lastCastWord = card.Word;
		lastCaster = caster;
	}

	internal override void OnTypePhaseTick (float dt)
	{
		timer -= dt;

		if (timer > 0)
		{
			setActiveState(false);
		}
		else if (shouldReenable)
		{
			setActiveState(true);
		}
	}
}

internal class Priest : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one especially consecrated to the service of divinity";
	public override string EffectText => $"clear {healed} status effect{(healed == 1 ? "" : "s")} from your keyboard";

	public override int Burn => 5;

	int healed = 3;

	protected override void behaviorImplementation ()
	{
		var illKeys = Owner.Keyboard.Where(s => s.Type != KeyStateType.Active).ToList();

		for (int i = 0; i < healed && i < illKeys.Count; i++)
		{
			illKeys[i].Type = KeyStateType.Active;
		}
	}
}

internal class Prince : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "of a royal family, a nonreigning male member";
	public override string EffectText => $"disable one of the {keys.ToNaturalString(true)} keys on your opponent's keyboard";

	public override int Burn => 5;

	// stats
	Keys keys = new Keys {
		KeyboardKey.P,
		KeyboardKey.R,
		KeyboardKey.I,
		KeyboardKey.N,
		KeyboardKey.C,
		KeyboardKey.E
	};

	// state
	byte cycle = 0;

	internal override void Deserialize (NetDataReader reader)
	{
		cycle = reader.GetByte();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(cycle);
	}

	protected override void behaviorImplementation ()
	{
		var enemy = manager.GetEnemyOf(Owner);
		var key = keys[cycle];

		cycle = (byte) ((cycle + 1) % keys.Count);

		enemy.Keyboard[key].Type = KeyStateType.Deactivated;
	}
}

internal class Prophet : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "spokesperson";
	public override string EffectText => $"draw {extraDraw} extra card{(extraDraw > 1 ? "s" : "")} next turn";

	public override int Burn => 5;

	// stats
	int extraDraw = 1;

	// state
	bool inOverdraw;

	internal override void Deserialize (NetDataReader reader)
	{
		inOverdraw = reader.GetBool();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(inOverdraw);
	}

	protected override void behaviorImplementation ()
	{
		Owner.HandSize += extraDraw;
		inOverdraw = true; 
	}

	internal override void OnDrawPhaseEnd ()
	{
		if (inOverdraw)
		{
			Owner.HandSize -= extraDraw;
			inOverdraw = false;
		}
	}
}

internal class Refuse : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "decline to accept; express determination to not do something";
	public override string EffectText => $"the next spell casted by you or your opponent loses all of its effects; instead of its regular effects, it deals damage equal to its length times {damageMult}";

	public override int Burn => 10;

	// stats
	int damageMult = 2;

	// state
	bool beforeFlag, afterFlag;

	internal override void Deserialize (NetDataReader reader)
	{
		beforeFlag = reader.GetBool();
		afterFlag = reader.GetBool();
	}

	internal override void Serialize(NetDataWriter writer)
	{
		writer.Put(beforeFlag);
		writer.Put(afterFlag);
	}

	protected override void behaviorImplementation ()
	{
		beforeFlag = true;
	}

	internal override void BeforeCardCast (Card card, Agent caster)
	{
		if (beforeFlag)
		{
			CastLock = true;
			beforeFlag = false;
			afterFlag = true;
		}
	}

	internal override void AfterCardCast(Card card, Agent caster)
	{
		if (afterFlag)
		{
			manager.GetEnemyOf(Owner).IncrementHealth(-card.Word.Length * damageMult);
			afterFlag = false;
		}
	}
}

internal class Sunset : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "the apparent descent of the sun";
	public override string EffectText => $"if there is less than {time} second{(time == 1 ? "" : "s")} left: deal damage based on your typing accuracy, for a maximum of {maxDamage} (this damage is at its maximum at 100% accuracy, but quickly drops off as your accuracy decreases)";

	public override int Burn => 10;

	float time = 1;
	float maxDamage = 12;

	protected override void behaviorImplementation ()
	{
		if (manager.TypingTimer <= time)
		{
			float damage = (float) Math.Pow(maxDamage, Owner.AccuracyThisTurn);
			manager.GetEnemyOf(Owner).IncrementHealth(-(int) damage);
		}
	}
}

internal class Sword : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "military power, violence, destruction, one of the tarot suits";
	public override string EffectText => $"deal {damage} damage";

	public override int Burn => 3;

	int damage = 3;

	protected override void behaviorImplementation ()
	{
		manager.GetEnemyOf(Owner).IncrementHealth(-damage);
	}
}

internal class TwoFaced : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "deceitful";
	public override string EffectText => $"disable the space bar on both your and your opponent's keyboard";

    public override int Burn => 1;

	protected override void behaviorImplementation ()
	{
		Owner.Keyboard[KeyboardKey.Space].Type = KeyStateType.Deactivated;
		manager.GetEnemyOf(Owner).Keyboard[KeyboardKey.Space].Type = KeyStateType.Deactivated;
	}
}

internal class Unveil : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "to make something clear";
	public override string EffectText => $"clear the status effects of any key on your keyboard that is touching at least {healEnergy} energy";

	public override int Burn => 5;

	int healEnergy = 4;

	protected override void behaviorImplementation ()
	{
		foreach (var state in Owner.Keyboard.Where(s => s.Type != KeyStateType.Active))
		{
			if (Owner.Keyboard.GetSurroundingKeys(state).Sum(s => s.EnergyLevel) >= healEnergy)
			{
				state.Type = KeyStateType.Active;
			}
		}
	}
}

internal class Weary : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "tired";
	public override string EffectText => $"if you've casted at least {minCast} spells this turn, deal {maxDamage} damage multiplied by the percent of the timer that has elapsed before casting this";

	public override int Burn => 20;

	int minCast = 3;
	int maxDamage = 10;

	protected override void behaviorImplementation ()
	{
		if (manager.CardsCastedThisTurn < minCast) return;

		float elapsed = 1 - manager.TypingTimeLeftPercent;
		manager.GetEnemyOf(Owner).IncrementHealth((int) (-maxDamage * elapsed));
	}
}

internal class Year : Card
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

	protected override void behaviorImplementation ()
	{
		int total = 0;
		foreach (var key in keys)
		{
			total += Owner.Keyboard[key].EnergyLevel;
		}
		manager.GetEnemyOf(Owner).IncrementHealth((int) (damageMult * -total));
	}
}

internal class Zealot : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one marked by fervant partisanship";
	public override string EffectText => $"concentrate all of your keyboard's energy into the last key you used to cast a spell before casting this one";

	public override int Burn => 2;

	bool keyIsSet;
	KeyboardKey lastKey;

	internal override void Deserialize (NetDataReader reader)
	{
		keyIsSet = reader.GetBool();
		lastKey = (KeyboardKey) reader.GetByte();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(keyIsSet);
		writer.Put((byte) lastKey);
	}

	protected override void behaviorImplementation ()
	{
		int total = 0;

		foreach (var state in Owner.Keyboard)
		{
			total += state.EnergyLevel;
			state.EnergyLevel = 0;
		}

		if (keyIsSet)
		{
			Owner.Keyboard[(KeyboardKey) lastKey].EnergyLevel = total;
		}
	}

	internal override void OnTypePhaseStart ()
	{
		keyIsSet = false;
	}

	internal override void AfterCardCast (Card card, Agent caster)
	{
		if (caster == Owner)
		{
			lastKey = card.Word[card.Word.Length - 1].ToKeyboardKey();
			keyIsSet = true;
		}
	}
}
}