using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CTShared.Networking;
using LiteNetLib.Utils;

namespace CTShared
{
public class Agent : Packet 
{
    public const int StartingMaxHealth = 100;

    public delegate void KeyPressedEvent (KeyboardKey key, bool shift);

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    public event Action<List<Card>> OnPlaySet;

    public event Action<string> OnAttemptedCast;
    public event KeyPressedEvent OnKeyPressed;

    public readonly MatchManager Manager;
    public readonly Deck Deck;
    public readonly Keyboard Keyboard;

    List<Card> play;
    public ReadOnlyCollection<Card> Play => play.AsReadOnly();

    // following values are bytes for serialization's sake, even though semantically we'd rather think of them as ints

    byte _maxHealth = StartingMaxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = (byte) Math.Max(0, value);
            Health = Math.Min(Health, value);

            if (value <= 0)
            {
                if (OnDeath != null) OnDeath();
            }
        }
    }

    byte _shield;
    public int Shield
    {
        get => _shield;
        set
        {
            _shield = (byte) Math.Max(0, value);
        }
    }

    byte _handSize = 7;
    public int HandSize
    {
        get => _handSize;
        set
        {
            _handSize = (byte) Math.Max(1, value);
        }
    }

    byte _health;
    public int Health
    {
        get => _health;
        protected set
        {
            _health = (byte) value;
        }
    }

    byte _cardsCasted;
    public int CardsCastedThisTurn
    {
        get => _cardsCasted;
        protected set
        {
            _cardsCasted = (byte) value;
        }
    }

    byte _lettersTyped;
    public int LettersTypedThisTurn
    {
        get => _lettersTyped;
        protected set
        {
            _lettersTyped = (byte) value;
        }
    }

    byte _lettersAccuratelyTyped;
    public int LettersAccuratelyTypedThisTurn
    {
        get => _lettersAccuratelyTyped;
        protected set
        {
            _lettersAccuratelyTyped = (byte) value;
        }
    }

    public string TypingProgress { get; protected set; }
    
    public float AccuracyThisTurn => (float) LettersAccuratelyTypedThisTurn / LettersTypedThisTurn;

    internal Agent (MatchManager manager, string deckText)
    {
        Health = MaxHealth;

        Manager = manager;
        manager.OnTypePhaseStart += startTypePhase;

        try
        {
            Deck = new Deck(deckText, this);
        }
        catch (Exception e)
        {
            throw new ArgumentException("error when parsing deck", e);
        }

        Keyboard = new Keyboard();
    }

    // for packet serialization
    internal Agent ()
    {
        Keyboard = new Keyboard();
        Deck = new Deck(this);
    }

    public void PressKey (KeyboardKey key, bool shift)
    {
        KeyState state = Keyboard[key];

        switch (state.Type)
        {
            case KeyStateType.Active:
                typeKey(key, shift);
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

        if (OnKeyPressed != null) OnKeyPressed(key, shift);
    }

    internal override void Deserialize (NetDataReader reader)
    {
        _maxHealth = reader.GetByte();
        _health = reader.GetByte();
        _shield = reader.GetByte();

        _handSize = reader.GetByte();
        _cardsCasted = reader.GetByte();

        _lettersTyped = reader.GetByte();
        _lettersAccuratelyTyped = reader.GetByte();

        Keyboard.Deserialize(reader);

        Deck.Deserialize(reader);
    }

    internal override void Serialize (NetDataWriter writer)
    {
        writer.Put(_maxHealth);
        writer.Put(_health);
        writer.Put(_shield);

        writer.Put(_handSize);
        writer.Put(_cardsCasted);

        writer.Put(_lettersTyped);
        writer.Put(_lettersAccuratelyTyped);

        Keyboard.Serialize(writer);

        Deck.Serialize(writer);
    }

    internal void DrawNewHand ()
    {
        Deck.DrawNewHand(HandSize);
    }

    public void SetPlay (List<Card> play)
    {
        this.play = play;
        if (OnPlaySet != null) OnPlaySet(play);
    }

    internal void SetHealth (int newValue)
    {
        IncrementHealth(newValue - Health);
    }

    internal void IncrementHealth (int delta)
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
                if (play.Any(c => c.Word.StartsWith(TypingProgress)))
                {
                    LettersAccuratelyTypedThisTurn++;
                }
                break;
        }
    }

    void tryCastSpell ()
    {
        Card toCast = play.FirstOrDefault(c => c.Word.Equals(TypingProgress));

        OnAttemptedCast(toCast?.Word ?? "");

        if (toCast == null) return;

        // space bar and return are considered accurate presses if they're used to cast a spell
        LettersAccuratelyTypedThisTurn++;

        toCast.DoBehavior(Manager, this);
        CardsCastedThisTurn++;

        TypingProgress = "";
        play.Remove(toCast);
    }
}
}