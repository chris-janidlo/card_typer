using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class UIKeyboard : MonoBehaviour
{
    public Color LabelOnColor, LabelOffColor, BorderOnColor, BorderOffColor;

#pragma warning disable CS0649 // disable 'never assigned to' warning since this is assigned in the inspector
	[SerializeField]
	private List<UIKey> keys;
#pragma warning restore CS0649

    public Dictionary<KeyboardKey, UIKey> Keys;

    public Agent Agent { get; private set; }

    void Start ()
    {
        Keys = keys.ToDictionary(k => k.Key);

        foreach (var key in keys)
        {
            key.Initialize(this);
        }
    }

    public void Initialize (Agent agent)
    {
        Agent = agent;
    }
}
