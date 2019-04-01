using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBehavior;

[Serializable]
public class Card
{
    public string Word, PartOfSpeech, Definition, ClassName;
    public int Burn;

    public void DoBehavior (Agent caster, Agent enemy)
    {
        EventBox.Log($"\n{caster.StatusName} casted {Word}.");
        Type t = Type.GetType($"CardBehavior.{ClassName}");
        ICardBehavior behavior = (ICardBehavior) Activator.CreateInstance(t);
        behavior.DoBehavior(this, caster, enemy);
    }
}
