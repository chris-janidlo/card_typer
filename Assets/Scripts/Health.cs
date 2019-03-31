using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int Max;
    public string ReceivingName, StatusName, StatusVerb = "has";

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

        if (value <= 0)
        {
            var temp = Death;
            if (temp != null) temp(this);
        }
    }

    public void IncrementValue (int delta, string sender, string verb)
    {
        SetValue(value + delta, sender, verb);
    }

    public void LogStatus ()
    {
        EventBox.Log($"\n{StatusName} {StatusVerb} {value} health out of {Max}.");
    }
}
