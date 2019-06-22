using System;
using System.Collections;
using System.Collections.Generic;

namespace CTShared
{
public class Agent
{
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    public readonly Deck Deck;
    public readonly Keyboard Keyboard;

    public int MaxHealth, Shield;

    public int HandSize = 7, CardsCastedThisTurn;
    public List<Card> Play;

    public int LettersTypedThisTurn, LettersAccuratelyTypedThisTurn;

    public int Health { get; protected set; }
    public float AccuracyThisTurn => (float) LettersAccuratelyTypedThisTurn / LettersTypedThisTurn;

    public Agent (string deckText)
    {
        Health = MaxHealth;

        Deck = new Deck(deckText);
        Keyboard = new Keyboard();
    }

    public void DrawNewHand ()
    {
        Deck.DrawNewHand(HandSize);
    }

    public void SetHealth (int newValue)
    {
        IncrementHealth(newValue - Health);
    }

    public void IncrementHealth (int delta)
    {
        if (delta == 0) return;

        int shieldedDelta = delta;

        if (delta < 0 && Shield > 0)
        {
            shieldedDelta = Math.Min(0, Shield + delta);
            Shield = Math.Max(0, Shield + delta);
        }

        Health += shieldedDelta;

        if (OnHealthChanged != null) OnHealthChanged(shieldedDelta);

        if (Health <= 0)
        {
            var temp = OnDeath;
            if (temp != null) temp();
        }
    }
}
}
