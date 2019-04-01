using UnityEngine;

namespace CardBehavior
{
public class WordLengthDamager : ICardBehavior
{
	public void DoBehavior(Card card, Agent caster, Agent enemy)
	{
		int damage = card.Word.Length;
		enemy.IncrementHealth(-damage);
		EventBox.Log($" Sparks fly everywhere, some hitting {enemy.ReceivingName}, who takes {damage} damage.");
	}
}
}
