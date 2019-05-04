using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public event Action<Agent> Death;

    public int MaxHealth;
    public IDrawer Drawer;
    public ITyper Typer;

    [SerializeField]
    int _health, _shield;
    public int Health { get => _health; protected set => _health = value; }
    public int Shield { get => _shield; protected set => _shield = value; }

    protected virtual void Awake ()
    {
        Health = MaxHealth;
    }

    public void SetHealth (int newValue)
    {
        IncrementHealth(newValue - Health);
    }

    public void IncrementHealth (int delta)
    {
        if (delta == 0) return;

        int shieldedDelta = delta;

        if (delta < 0 && Shield > 0)
        {
            shieldedDelta = Mathf.Min(0, Shield + delta);
            Shield = Mathf.Max(0, Shield + delta);
        }

        Health += shieldedDelta;

        if (Health <= 0)
        {
            var temp = Death;
            if (temp != null) temp(this);
        }
    }
}
