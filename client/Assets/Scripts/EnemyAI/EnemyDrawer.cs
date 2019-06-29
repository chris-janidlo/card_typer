using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class EnemyDrawer : MonoBehaviour
{
    public TypingDisplay Typer;

    Deck deck;

    public void Initialize (Deck deck)
    {
        this.deck = deck;
        ManagerContainer.Manager.OnDrawPhaseStart += startPhase;
    }

    void startPhase ()
    {
        var play = deck.Hand.ToList();

        Typer.SetPlay(play);
        ManagerContainer.Manager.ReadyUp(deck.Owner, play);
    }
}
