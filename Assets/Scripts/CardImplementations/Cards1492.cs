using System;
using System.Collections;
using System.Collections.Generic;

namespace Cards1492
{
public class Abhor : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "detest; loathe; abominate; despise; hate";
	public override string EffectText => $"deal {damagePerNox} damage for every nox";

	public override int Burn => 0;

	int damagePerNox = 1;

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

public class Ancient : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "having the qualities of age; old-fashioned; antique";
	public override string EffectText => "";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		throw new NotImplementedException();
	}
}

public class Barrier : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a circumstance or obstacle that keeps people apart or prevents progress";
	public override string EffectText => $"gain {shieldPerNox} shield per nox";

	public override int Burn => 0;

	float shieldPerNox = 0.5f;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Shield += (int) (caster.Nox * shieldPerNox);
	}
}

public class Bulwark : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a strong support";
	public override string EffectText => $"deal {damagePerLux} damage per lux";

	public override int Burn => 0;

	float damagePerLux = 1;

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

	public override int Burn => 0;

	int damagePerCard = 3;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-Typer.Instance.CardsCasted * damagePerCard);
	}
}

public class Flaming : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "passionate, violent, used as an intensifier";
	public override string EffectText => $"if this precedes another attack, deal an extra {damagePerLux} damage per lux";

	public override int Burn => 0;

	float damagePerLux = 0.5f;
	
	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		var damageCards = new List<string> { "sword", "abhorred" };

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

public class Grim : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "lacking genuine levity";
	public override string EffectText => $"gain {noxGain} nox";

	public override int Burn => 0;

	int noxGain = 10;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Nox += noxGain;
	}
}

public class Hound : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "pursued tenaciously, doglike";
	public override string EffectText => "";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		throw new NotImplementedException();
	}
}

public class Lock : Card
{
	public override string PartOfSpeech => "verb";
	public override string Definition => "fasten by a key or combination";
	public override string EffectText => "the next spell has no effect on your lux or nox";

	public override int Burn => 0;

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

public class Priest : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one especially consecrated to the service of divinity";
	public override string EffectText => $"heal {healPerLux} health for every lux; remove {1 - luxRedux : P0} of your lux";

	public override int Burn => 0;

	int healPerLux = 2;
	float luxRedux = 0.5f;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.IncrementHealth(caster.Lux * healPerLux);
		caster.Lux = (int) (caster.Lux * luxRedux);
	}
}

public class Prince : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "of a royal family, a nonreigning male member";
	public override string EffectText => "";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		throw new NotImplementedException();
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
		caster.HandSize += extraDraw;
		
		Action reducer = null;
		reducer = () => {
			caster.HandSize -= extraDraw;
			Drawer.Instance.OnEndPhase -= reducer;
		};
		Drawer.Instance.OnEndPhase += reducer;
	}
}

public class Sword : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "military power, violence, destruction, one of the tarot suits";
	public override string EffectText => $"deal {damage} damage";

	public override int Burn => 0;

	int damage = 3;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-damage);
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
	public override string Definition => "to make something secret known";
	public override string EffectText => "";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		throw new NotImplementedException();
	}
}

public class Weary : Card
{
	public override string PartOfSpeech => "adjective";
	public override string Definition => "tired";
	public override string EffectText => "";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		throw new NotImplementedException();
	}
}

public class Year : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "a period of approximately the same length in other calendars";
	public override string EffectText => "";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		throw new NotImplementedException();
	}
}

public class Zealot : Card
{
	public override string PartOfSpeech => "noun";
	public override string Definition => "one marked by fervant partisanship";
	public override string EffectText => "convert all lux to nox";

	public override int Burn => 0;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		caster.Nox += caster.Lux;
		caster.Lux = 0;
	}
}
}
