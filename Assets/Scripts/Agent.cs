using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int MaxHealth;
    public string SubjectName, ObjectName, StatusVerb = "has";
    public int HandSize = 7;
    public bool EssenceLock, LuxLock, NoxLock, NoxFloor;

    [SerializeField]
    int _lux, _nox, _shield;

    public int Lux
    {
        get => _lux;
        set
        {
            if (EssenceLock || LuxLock) return;

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
            if (EssenceLock || NoxLock) return;
            if (NoxFloor && value < Nox) return;
            if (_nox == value) return;

            if (value < 0) value = 0;

            EventBox.Log(essenceStatus("nox", value - _nox, value));

            _nox = value;
        }
    }

    public int Shield
    {
        get => _shield;
        set
        {
            EventBox.Log(essenceStatus("shield", value - _shield, value));
            _shield = value;
        }
    }

    [SerializeField]
    int health;

    public event System.Action<Agent> Death;

    protected virtual void Awake ()
    {
        health = MaxHealth;
    }

    protected virtual void Start ()
    {
        Typer.Instance.OnEndPhase += endTypeStep;
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

        EventBox.Log($" {sender} {verb} {ObjectName} for {Mathf.Abs(newValue - health)} health.");

        SetHealth(newValue);
    }

    public void IncrementHealth (int delta, string sender, string verb)
    {
        int temp = delta;
        if (delta < 0 && Shield > 0)
        {
            EventBox.Log($" {SubjectName} blocked {Math.Min(Shield, Math.Abs(delta))} damage.");
            temp = Mathf.Min(0, Shield + delta);
        }
        SetHealth(health + temp, sender, verb);

        if (delta < 0 && Shield > 0)
        {
            Shield = Mathf.Max(0, Shield + delta);
        }
    }

    public void LogStatus ()
    {
        EventBox.Log($"\n{SubjectName} {StatusVerb} {health} health out of {MaxHealth}.");
    }

    void endTypeStep ()
    {
        string purple = "\n\nHatred grows";
        purple += Lux > 0 ? " and the light recedes." : ".";
        EventBox.Log(purple);
        Lux--;
        Nox++;
    }

    string essenceStatus (string essenceName, int delta, int value)
    {
        return $"\n{SubjectName} now {StatusVerb} {System.Math.Abs(delta)} {(delta > 0 ? "more" : "less")} {essenceName}, for a total of {value}.";
    }
}
