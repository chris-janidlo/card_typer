using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDrawer : MonoBehaviour
{
    public abstract int HandSize { get; set; }

    public abstract void InitializeGame ();
    // the callback Must be called when the implementer has determined the play (whether the player chose their cards, the enemy calculated their play, the online opponent sent over their play, etc) in order for the game flow to continue
    public abstract void StartPhase (DecidedPlayCallback callback);
}

public delegate void DecidedPlayCallback (List<Card> play);
