using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int MaxHealth;
    public string ReceivingName, StatusName, StatusVerb = "has";
    public Blackboard Blackboard;

    [SerializeField]
    int health;

    public event System.Action<Agent> Death;

    void Awake ()
    {
        health = MaxHealth;
    }

    public void SetHealth (int newValue)
    {
        if (newValue == health) return;

        health = newValue;

        if (health <= 0)
        {
            var temp = Death;
            if (temp != null) temp(this);
        }
    }

    public void IncrementHealth (int delta)
    {
        SetHealth(health + delta);
    }

    public void SetHealth (int newValue, string sender, string verb)
    {
        if (newValue == health) return;

        EventBox.Log($"{sender} {verb} {ReceivingName} for {Mathf.Abs(newValue - health)} health.");

        SetHealth(newValue);
    }

    public void IncrementHealth (int delta, string sender, string verb)
    {
        SetHealth(health + delta, sender, verb);
    }

    public void LogStatus ()
    {
        EventBox.Log($"\n{StatusName} {StatusVerb} {health} health out of {MaxHealth}.");
    }
}

[System.Serializable]
public class Blackboard
{
    public Dictionary<string, int> Ints = new Dictionary<string, int>();
    public Dictionary<string, float> Floats = new Dictionary<string, float>();
    public Dictionary<string, string> Strings = new Dictionary<string, string>();
}
