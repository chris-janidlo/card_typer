using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CTShared;
using TMPro;

public class UIKey : MonoBehaviour
{
    public KeyboardKey Key;

    Image border;
    TextMeshProUGUI label;

    UIKeyboard parent;

    IEnumerator pressRef;

    public void Initialize (UIKeyboard parent)
    {
        this.parent = parent;
        
        border = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        label.text = getKeyLabel(Key);
        
        SetActiveState(false);
    }

    public void SetActiveState (bool value)
    {
        border.color = value ? parent.BorderOnColor : parent.BorderOffColor;
        label.color = value ? parent.LabelOnColor : parent.LabelOffColor;
    }

    string getKeyLabel (KeyboardKey key)
    {
        if (key.IsAcceptableInput())
        {
            return key.ToChar().ToString();
        }
        else
        {
            switch (key)
            {
                case KeyboardKey.Backspace:
                    return "←";
                
                case KeyboardKey.Return:
                    return "return";

                default:
                    throw new System.Exception($"unexpected keycode {key}");
            }
        }
    }
}
