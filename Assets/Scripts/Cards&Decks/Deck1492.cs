using System;
using System.Collections;
using System.Collections.Generic;
using Cards1492;

public class Deck1492 : Deck
{
	// 1492 by emma lazarus

	protected override string bracketedText => "Thou {two-faced} {year}, Mother of Change and Fate,\nDidst weep when Spain cast forth with {flaming} {sword},\nThe children of the {prophets} of the Lord,\n{Prince}, {priest}, and people, spurned by {zealot} hate.\n{Hounded} from sea to sea, from state to state,\nThe West {refused} them, and the East {abhorred}.\nNo {anchorage} the known world could afford,\nClose-{locked} was every port, barred every gate.\nThen smiling, thou {unveil'dst}, O {two-faced} year,\nA virgin world where doors of {sunset} part,\nSaying, \"Ho, all who {weary}, enter here!\nThere falls each {ancient} {barrier} that the art\nOf race or creed or rank {devised}, to rear\n{Grim} {bulwarked} hatred between {heart} and {heart}!\"";

	protected override List<Card> cardList => new List<Card>
	{
		new TwoFaced(),
		new Year(),
		new Flaming(),
		new Sword(),
		new Prophet(),
		new Prince(),
		new Priest(),
		new Zealot(),
		new Hound(),
		new Refuse(),
		new Abhor(),
		new Anchorage(),
		new Lock(),
		new Unveil(),
		new TwoFaced(),
		new Sunset(),
		new Weary(),
		new Ancient(),
		new Barrier(),
		new Devise(),
		new Grim(),
		new Bulwark(),
		new Heart(),
		new Heart()
	};
}

public class WordLengthDamager : Card
{
	public override string PartOfSpeech => "everything";
	public override string Definition => "depends on the word";
	public override string EffectText => "deal damage based on the length of the word";

	public override int Burn => Word.Length;

	protected override void behaviorImplementation (Agent caster, Agent enemy)
	{
		enemy.IncrementHealth(-Word.Length);
	}
}
