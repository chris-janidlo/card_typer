using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITyper : MonoBehaviour
{
    public static int CardsCastedSinceTurnStart;

    public abstract void SetPlay (List<Card> play);
    public abstract List<Card> GetPlay ();
    public abstract void StartPhase ();
    public abstract void EndPhase ();
}
