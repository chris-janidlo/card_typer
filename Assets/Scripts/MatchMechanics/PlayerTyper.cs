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
        if (!AcceptingInput) return;

        Event e = Event.current;

        if (!e.isKey || e.type != EventType.KeyDown) return;

        KeyCode key = e.keyCode;

        if (key.IsAcceptableInput(true))
        {
            typeKey(key, e.shift);
        }
    }
}
