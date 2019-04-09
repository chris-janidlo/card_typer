using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Card
{
    public string Word, PartOfSpeech, Definition, EffectText, ClassName;
    public int Burn;

    public abstract void DoBehavior (Agent caster, Agent enemy);
    // {
	// 	EventBox.Log($"\n{caster.StatusName} casted {Word}.");
    //     CardBehavior.DoCardBehavior(this, caster, enemy);
    // }
}
