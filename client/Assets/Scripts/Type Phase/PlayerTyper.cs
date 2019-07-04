using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;
using TMPro;
using crass;

public class PlayerTyper : MonoBehaviour
{
    public UIKeyboard UIKeyboard;

    Agent agent;
    bool acceptingInput;

    void Start ()
    {
        var mgr = ManagerContainer.Manager;

        mgr.OnTypePhaseStart += startPhase;
        mgr.OnTypePhaseEnd += endPhase;
    }

    void OnGUI ()
    {
        Event e = Event.current;

        if (!e.isKey) return;

        KeyCode keyCode = e.keyCode;
        KeyboardKey? maybeKey = keyCode.ToKeyboardKey();

        // filter out events with valid character but invalid keyCode (https://docs.unity3d.com/ScriptReference/EventType.KeyDown.html). also filter out non-input keys
        if (e.keyCode == KeyCode.None || maybeKey == null) return;

        KeyboardKey key = (KeyboardKey) maybeKey;

        UIKeyboard.Keys[key].SetActiveState(e.type == EventType.KeyDown);

        if (!acceptingInput || e.type != EventType.KeyDown) return;

        // TODO: key state feedback, hook up to agent.OnKeyPressed
        agent.PressKey(key, e.shift);
    }

    public void Initialize (Agent agent)
    {
        this.agent = agent;
    }

    void startPhase ()
    {
        acceptingInput = true;
    }

    void endPhase ()
    {
        acceptingInput = false;
    }
}
