using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrawer : IDrawer
{
    public override int HandSize { get; set; } = 4;

    public override void StartPhase (DecidedPlayCallback callback)
    {
        var cards = new List<Card>
        {
            new WordLengthDamager { Word = "Grim" },
            new WordLengthDamager { Word = "abhorred" },
            new WordLengthDamager { Word = "hatred" },
            new WordLengthDamager { Word = "heart" },
        };
        callback(cards);
    }
}
