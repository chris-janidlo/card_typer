using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public static event Action<Card, Agent> BeforeCast, AfterCast;
    public static bool CastLock;

    // the actual word in the poem that this is shadowing
    public string Word;

    public abstract string PartOfSpeech { get; }
    public abstract string Definition { get; }
    public abstract string EffectText { get; }

    public abstract int Burn { get; }

    public Card ()
    {
        initialize();
    }

    public void DoBehavior (Agent caster, Agent enemy)
    {
        if (BeforeCast != null) BeforeCast(this, caster);
        if (!CastLock) behaviorImplementation(caster, enemy);
        if (AfterCast != null) AfterCast(this, caster);
    }

    protected abstract void behaviorImplementation (Agent caster, Agent enemy);

    protected virtual void initialize () {}
}
