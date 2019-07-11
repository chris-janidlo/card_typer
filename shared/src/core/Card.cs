using System;
using System.Reflection;

namespace CTShared
{
public abstract class Card
{
    public delegate void CastEvent (Card card, Agent caster);
    public static event CastEvent BeforeCast, AfterCast;
    public static bool CastLock;

    // the actual word in the poem that this is shadowing
    public string Word;

    public Agent Owner { get; protected set; }

    public abstract string PartOfSpeech { get; }
    public abstract string Definition { get; }
    public abstract string EffectText { get; }

    public abstract int Burn { get; }

    protected MatchManager manager => Owner.Manager;

    // FIXME: change Type.GetType call to some kind of dictionary call, for security purposes
    public static Card FromName (string name, Agent owner)
    {
        Type t = Type.GetType("CTShared.Cards." + name);
        Card card = (Card) t.GetConstructor(Type.EmptyTypes).Invoke(null);

        card.Owner = owner;
        card.initialize();

        return card;
    }

    protected Card () {}

    internal void DoBehavior (MatchManager manager, Agent caster)
    {
        if (BeforeCast != null) BeforeCast(this, caster);

        if (!CastLock) behaviorImplementation(caster);

        if (AfterCast != null) AfterCast(this, caster);
    }

    protected abstract void behaviorImplementation (Agent caster);

    protected virtual void initialize () {}
}
}
