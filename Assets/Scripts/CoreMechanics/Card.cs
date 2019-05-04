using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public static event Action<Card, Agent> CardCast;

    // the actual word in the poem that this is shadowing
    public string Word;

    public abstract string PartOfSpeech { get; }
    public abstract string Definition { get; }
    public abstract string EffectText { get; }

    public abstract int Burn { get; }

    public void DoBehavior (Agent caster, Agent enemy)
    {
        if (CardCast != null) CardCast(this, caster);
        behaviorImplementation(caster, enemy);
    }

    protected abstract void behaviorImplementation (Agent caster, Agent enemy);
}
