using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int Max;
    public string ReceivingName;

    [SerializeField]
    int value;

    public event System.Action<Health> Death;

    void Awake ()
    {
        value = Max;
    }

    public void SetValue (int newValue, string sender, string verb)
    {
        if (newValue == value) return;

        EventBox.Log($"{sender} {verb} {ReceivingName} for {Mathf.Abs(newValue - value)} health.");

        value = newValue;
    }

    public void IncrementValue (int delta, string sender, string verb)
    {
        SetValue(value + delta, sender, verb);
    }
}
