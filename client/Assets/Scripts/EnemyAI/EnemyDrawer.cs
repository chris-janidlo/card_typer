using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class EnemyDrawer : MonoBehaviour
{
    Deck deck;

    public void Initialize (Deck deck)
    {
        this.deck = deck;
        ManagerContainer.Manager.OnDrawPhaseStart += startPhase;
    }

    void startPhase ()
    {
        var play = deck.Hand.ToList();
        deck.Owner.SetPlay(play);
    }
}
