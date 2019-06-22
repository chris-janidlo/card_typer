using System;

namespace CTShared
{
public abstract class Card
{
    public delegate void CastEvent (Card card, MatchManager manager, Agent caster, Agent enemy);
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

    public void DoBehavior (MatchManager manager, Agent caster, Agent enemy)
    {
        if (BeforeCast != null) BeforeCast(this, manager, caster, enemy);

        if (!CastLock)
        {
            behaviorImplementation(manager, caster, enemy);
            caster.CardsCastedThisTurn++;
        }

        if (AfterCast != null) AfterCast(this, manager, caster, enemy);
    }

    protected abstract void behaviorImplementation (MatchManager manager, Agent caster, Agent enemy);

    protected virtual void initialize () {}
}
}
