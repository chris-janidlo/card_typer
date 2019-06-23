using System;

namespace CTShared
{
public abstract class Card
{
    public delegate void CastEvent (Card card, MatchManager manager, Agent caster);
    public static event CastEvent BeforeCast, AfterCast;
    public static bool CastLock;

    // the actual word in the poem that this is shadowing
    public string Word;

    public abstract string PartOfSpeech { get; }
    public abstract string Definition { get; }
    public abstract string EffectText { get; }

    public abstract int Burn { get; }

    // TODO:
    public static Card FromName (string name)
    {
        Card card = null;
        card.initialize();
        return null;
    }

    private Card ()
    {
    }

    public void DoBehavior (MatchManager manager, Agent caster)
    {
        if (BeforeCast != null) BeforeCast(this, manager, caster);

        if (!CastLock) behaviorImplementation(manager, caster);

        if (AfterCast != null) AfterCast(this, manager, caster);
    }

    protected abstract void behaviorImplementation (MatchManager manager, Agent caster);

    protected virtual void initialize () {}
}
}
