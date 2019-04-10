using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using crass;

public class Enemy : Agent
{
    public Vector2Int DamageRange;

    int damagePlan;

    protected override void Start ()
    {
        base.Start();
        Death += a => SceneManager.LoadScene("Victory");
    }

    public int DeviseDamagePlan ()
    {
        return damagePlan = RandomExtra.Range(DamageRange);
    }

    public int GetDamagePlan ()
    {
        return damagePlan;
    }
}
