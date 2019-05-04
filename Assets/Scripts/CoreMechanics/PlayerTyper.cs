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

        if (e.keyCode == KeyCode.Space || e.keyCode == KeyCode.Return || e.keyCode == KeyCode.Backspace)
        {
            typeKey(e.keyCode);
            return;
        }

        char typed = e.character;

        // shoo away the weird ghost characters
        // seriously everything breaks if this isn't here because UGUI is haunted
        if (!Char.IsLetter(typed) && !Char.IsDigit(typed) && !Char.IsPunctuation(typed)) return;

        typeKey(typed);
    }
}
