using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CardBehavior
{
public class Two : Card
{
	public override string PartOfSpeech => "adverb";
	public override string Definition => "one more than one, three less than five, six more than negative eight";
	public override string EffectText => "if this precedes an adjective, apply the effects of that adjective to both you and your opponent";

    public override int Burn => 1;

	public Two (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		Action<Card, Agent> mirror = null;

		mirror = (card, agent) => {
			if (agent == caster && card.PartOfSpeech.Equals("adjective")) 
			{
				card.DoBehavior(enemy, caster);
			}
			CardCast -= mirror;
		};

		CardCast += mirror;
	}
}

public class Faced : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "having a particular face";
	public override string EffectText => "balance your lux and nox";

	public override int Burn => 3;
	
	public Faced (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		int average = (caster.Lux + caster.Nox) / 2;
		caster.Lux = average;
		caster.Nox = average;
	}
}

public class Flaming : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "passionate, violent, used as an intensifier";
	public override string EffectText => $"if this precedes another attack, deal an extra {damagePerLux} damage per lux";

	public override int Burn => 0;

	float damagePerLux = 0.5f;
	
	public Flaming (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		var damageCards = new string[] { "sword", "abhorred" };

		Action<Card, Agent> burn = null;
		burn = (card, agent) => {
			if (agent == caster && damageCards.Contains(card.Word))
			{
				enemy.IncrementHealth((int) (-caster.Lux * damagePerLux));
			}
			CardCast -= burn;
		};

		CardCast += burn;
	}
}

public class Sword : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "military power, violence, destruction, one of the tarot suits";
	public override string EffectText => $"deal {damage} damage";

	public override int Burn => 0;

	int damage = 3;

	public Sword (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-damage);
	}
}

public class Prophet : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "spokesperson";
	public override string EffectText => $"draw {extraDraw} extra card{(extraDraw > 1 ? "s" : "")} next turn";

	public override int Burn => 5;

	int extraDraw = 1;

	public Prophet (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.HandSize += extraDraw;
		
		Action reducer = null;
		reducer = () => {
			caster.HandSize -= extraDraw;
			Drawer.Instance.OnEndPhase -= reducer;
		};
		Drawer.Instance.OnEndPhase += reducer;
	}
}

public class Priest : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one especially consecrated to the service of divinity";
	public override string EffectText => $"heal {healPerLux} health for every lux; remove {1 - luxRedux : P0} of your lux";

	public override int Burn => 0;

	int healPerLux = 2;
	float luxRedux = 0.5f;

	public Priest (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.IncrementHealth(caster.Lux * healPerLux);
		caster.Lux = (int) (caster.Lux * luxRedux);
	}
}

public class Zealot : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one marked by fervant partisanship";
	public override string EffectText => "convert all lux to nox";

	public override int Burn => 0;

	public Zealot (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Nox += caster.Lux;
		caster.Lux = 0;
	}
}

public class Abhor : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "detest; loathe; abominate; despise; hate";
	public override string EffectText => $"deal {damagePerNox} damage for every nox";

	public override int Burn => 0;

	int damagePerNox = 1;

	public Abhor (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-caster.Nox * damagePerNox);
	}
}

public class Anchorage : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a port; a source of assurance; a dwelling place of a religious refuse";
	public override string EffectText => "for the rest of the turn, do not lose any nox";

	public override int Burn => 0;

	public Anchorage (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.NoxFloor = true;

		Action unlock = null;
		unlock = () => {
			caster.NoxFloor = false;
			Typer.Instance.OnEndPhase -= unlock;
		};
		Typer.Instance.OnEndPhase += unlock;
	}
}

public class Lock : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "fasten by a key or combination";
	public override string EffectText => "the next spell has no effect on your lux or nox";

	public override int Burn => 0;

	public Lock (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.EssenceLock = true;
		
		Action<Card, Agent> unlock = null;
		unlock = (card, agent) => {
			caster.EssenceLock = false;
			CardCast -= unlock;
		};
		CardCast += unlock;
	}
}

public class Barrier : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a circumstance or obstacle that keeps people apart or prevents progress";
	public override string EffectText => $"gain {shieldPerNox} shield per nox";

	public override int Burn => 0;

	float shieldPerNox = 0.5f;

	public Barrier (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Shield += (int) (caster.Nox * shieldPerNox);
	}
}

public class Grim : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "lacking genuine levity";
	public override string EffectText => $"gain {noxGain} nox";

	public override int Burn => 0;

	int noxGain = 10;

	public Grim (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Nox += noxGain;
	}
}

public class Bulwark : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a strong support";
	public override string EffectText => $"deal {damagePerLux} damage per lux";

	public override int Burn => 0;

	float damagePerLux = 1;

	public Bulwark (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth((int) (-caster.Lux * damagePerLux));
	}
}

public class Devise : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "plan or contrive in the mind";
	public override string EffectText => $"deal {damagePerCard} damage for every spell cast so far this turn";

	public override int Burn => throw new NotImplementedException();

	int damagePerCard = 3;

	public Devise (string word) : base(word) {}

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-Typer.Instance.CardsCasted * damagePerCard);
	}
}
}