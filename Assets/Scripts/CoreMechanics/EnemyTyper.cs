using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTyper : LocalTyper
{
    public float CharsPerSecond;

    string typingGoal = "Grim abhorred hatred heart ";

    float typingIndex;
    int typingTicker;

    public override void StartPhase ()
    {
        base.StartPhase();
        typingIndex = -1;
        typingTicker = -1;
    }

    void Update ()
    {
        if (!AcceptingInput) return;

        typingIndex += CharsPerSecond * Time.deltaTime;
        
        if (typingIndex >= typingTicker + 1)
        {
            typingTicker++;
            typeKey(typingGoal[typingTicker]);
        }
    }
}
