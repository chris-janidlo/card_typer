using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class TypePhaseView : MonoBehaviour
{
    void Start ()
    {
        var mgr = ManagerContainer.Manager;

        mgr.OnTypePhaseStart += startPhase;
        mgr.OnTypePhaseEnd += endPhase;
    }

    void startPhase ()
    {

    }

    void endPhase ()
    {

    }
}
