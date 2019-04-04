﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int MaxHealth;
    public string ReceivingName, StatusName, StatusVerb = "has";
    public int HandSize = 7;
    public bool EssenceLock;
    public int Shield;

    [SerializeField]
    int _lux, _nox;

    public int Lux
    {
        get => _lux;
        set
        {
            if (EssenceLock) return;

            if (value < 0) value = 0;

            int delta = value - _lux;

            if (delta == 0) return;

            if (delta > 0)
            {
                Nox = 0;
            }

            _lux = value;

            EventBox.Log(essenceStatus("lux", delta, value));
        }
    }

    public int Nox
    {
        get => _nox;
        set
        {
            if (EssenceLock) return;

            if (value < 0) value = 0;

            EventBox.Log(essenceStatus("nox", value - _nox, value));

            _nox = value;
        }
    }

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
        int temp = delta;
        if (delta < 0)
        {
            temp = Mathf.Min(0, delta + Shield);
            Shield = Mathf.Max(0, Shield - delta);
        }
        SetHealth(health + temp, sender, verb);
    }

    public void LogStatus ()
    {
        EventBox.Log($"\n{StatusName} {StatusVerb} {health} health out of {MaxHealth}.");
    }

    public void EndTypeStep ()
    {
        Lux--;
        Nox++;
    }

    string essenceStatus (string essenceName, int delta, int value)
    {
        return $"\n{StatusName} now {StatusVerb} {System.Math.Abs(delta)} {(delta > 0 ? "more" : "less")} {essenceName}, for a total of {value}.";
    }
}

[System.Serializable]
public class Blackboard
{
    public Dictionary<string, int> Ints = new Dictionary<string, int>();
    public Dictionary<string, float> Floats = new Dictionary<string, float>();
    public Dictionary<string, string> Strings = new Dictionary<string, string>();
}
