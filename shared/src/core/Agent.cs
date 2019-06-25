using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CTShared
{
public class Agent
{
    public const int StartingMaxHealth = 100;

    public delegate void KeyPressedEvent (KeyboardKey key, KeyStateType status);

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    public event Action<string> OnAttemptedCast;
    public event KeyPressedEvent OnKeyPressed;

    public readonly MatchManager Manager;
    public readonly Deck Deck;
    public readonly Keyboard Keyboard;

    public List<Card> Play;

    int _maxHealth = StartingMaxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = Math.Max(0, value);
            Health = Math.Min(Health, value);

            if (value <= 0)
            {
                if (OnDeath != null) OnDeath();
            }
        }
    }

    int _shield;
    public int Shield
    {
        get => _shield;
        set
        {
            _shield = Math.Max(0, value);
        }
    }

    int _handSize = 7;
    public int HandSize
    {
        get => _handSize;
        set
        {
            _handSize = Math.Max(1, value);
        }
    }

    public int Health { get; protected set; }
    
    public string TypingProgress { get; protected set; }
    public int CardsCastedThisTurn { get; protected set; }
    
    public int LettersTypedThisTurn { get; protected set; }
    public int LettersAccuratelyTypedThisTurn { get; protected set; }
    
    public float AccuracyThisTurn => (float) LettersAccuratelyTypedThisTurn / LettersTypedThisTurn;

    public Agent (MatchManager manager, string deckText)
    {
        Health = MaxHealth;

        Manager = manager;
        manager.OnTypePhaseStart += startTypePhase;

        Deck = new Deck(deckText, this);
        Keyboard = new Keyboard();
    }

    public void PressKey (KeyboardKey key, bool shiftIsPressed)
    {
        KeyState state = Keyboard[key];

        if (OnKeyPressed != null) OnKeyPressed(key, state.Type);

        switch (state.Type)
        {
            case KeyStateType.Active:
                typeKey(key, shiftIsPressed);
                break;

            case KeyStateType.Deactivated:
                break;

            case KeyStateType.Sticky:
                state.StickyPressesRemaining--;
                if (state.StickyPressesRemaining <= 0)
                {
                    state.Type = KeyStateType.Active;
                }
                break;
        }
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

    void startTypePhase ()
    {
        TypingProgress = "";
        CardsCastedThisTurn = 0;
        LettersTypedThisTurn = 0;
        LettersAccuratelyTypedThisTurn = 0;
    }

    void typeKey (KeyboardKey key, bool shiftIsPressed)
    {
        LettersTypedThisTurn++;

        switch (key)
        {
            case KeyboardKey.Space:
            case KeyboardKey.Return:
                LettersTypedThisTurn++;
                tryCastSpell();
                break;

            case KeyboardKey.Backspace:
                if (TypingProgress.Length > 0)
                {
                    TypingProgress = TypingProgress.Substring(0, TypingProgress.Length - 1);
                }
                break;
            
            default:
                TypingProgress += key.ToChar(shiftIsPressed);
                LettersTypedThisTurn++;
                if (Play.Any(c => c.Word.StartsWith(TypingProgress)))
                {
                    LettersAccuratelyTypedThisTurn++;
                }
                break;
        }
    }

    void tryCastSpell ()
    {
        Card toCast = Play.FirstOrDefault(c => c.Word.Equals(TypingProgress));

        OnAttemptedCast(toCast?.Word ?? "");

        if (toCast == null) return;

        toCast.DoBehavior(Manager, this);
        CardsCastedThisTurn++;

        TypingProgress = "";
        Play.Remove(toCast);
    }
}
}
