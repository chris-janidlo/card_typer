using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyboard : MonoBehaviour
{
    public Color LabelOnColor, LabelOffColor, BorderOnColor, BorderOffColor;
    public float OnTime;

	[SerializeField]
	private List<UIKey> keys;

    public Dictionary<KeyCode, UIKey> Keys;

    void Start ()
    {
        Keys = keys.ToDictionary(k => k.Key);

        foreach (var key in keys)
        {
            key.Initialize(this);
        }
    }
}
