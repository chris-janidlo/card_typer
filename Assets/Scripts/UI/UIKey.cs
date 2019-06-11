using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIKey : MonoBehaviour
{
    public KeyCode Key;

    Image border;
    TextMeshProUGUI label;

    UIKeyboard parent;

    IEnumerator pressRef;

    public void Initialize (UIKeyboard parent)
    {
        this.parent = parent;
        
        border = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        label.text = Key.ToChar().ToString();
        
        SetActiveState(false);
    }

    public void SetActiveState (bool value)
    {
        border.color = value ? parent.BorderOnColor : parent.BorderOffColor;
        label.color = value ? parent.LabelOnColor : parent.LabelOffColor;
    }
}
