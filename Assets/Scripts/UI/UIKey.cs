using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIKey : MonoBehaviour
{
    public KeyCode Key;
    public Color LabelOnColor, LabelOffColor, BorderOnColor, BorderOffColor;

    Image border;
    TextMeshProUGUI label;

    void Start ()
    {
        border = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        label.text = Key.ToChar().ToString();
    }

    void Update ()
    {
        bool on = Input.GetKey(Key);

        border.color = on ? BorderOnColor : BorderOffColor;
        label.color = on ? LabelOnColor : LabelOffColor;
    }
}
