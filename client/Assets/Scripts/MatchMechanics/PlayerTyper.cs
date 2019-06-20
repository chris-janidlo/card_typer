using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class PlayerTyper : LocalTyper
{
    void OnGUI ()
    {
        Event e = Event.current;

        if (!e.isKey) return;

        KeyCode key = e.keyCode;

        // filter out events with valid character but invalid keyCode (https://docs.unity3d.com/ScriptReference/EventType.KeyDown.html). also filter out non-input keys
        if (e.keyCode == KeyCode.None || !key.IsAcceptableInput(true)) return;

        UIKeyboard.Keys[key].SetActiveState(e.type == EventType.KeyDown);

        if (!AcceptingInput || e.type != EventType.KeyDown) return;

        typeKey(key, e.shift);
    }
}
