using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;
using TMPro;

public class TypingTimer : MonoBehaviour
{
    public TextMeshProUGUI Display;

    bool inPreTypePhase, inTypePhase;
    MatchManager manager;

    void Start ()
    {
        manager = ManagerContainer.Manager;

        manager.OnPreTypePhaseStart += () => inPreTypePhase = true;
        manager.OnTypePhaseStart += () => {
            inPreTypePhase = false;
            inTypePhase = true;
        };
        manager.OnTypePhaseEnd += () => inTypePhase = false;
    }

    void Update ()
    {
        if (inPreTypePhase)
        {
            Display.text = Mathf.Ceil(manager.CountdownTimer).ToString();
        }
        else if (inTypePhase)
        {
            Display.text = Mathf.Ceil(manager.TypingTimer).ToString();
        }
        else
        {
            Display.text = "";
        }
    }
}
