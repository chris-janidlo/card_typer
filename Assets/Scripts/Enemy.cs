using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using crass;

[RequireComponent(typeof(Health))]
public class Enemy : Singleton<Enemy>
{
    public Vector2Int DamageRange;

    public Health Health { get; private set; }
    
    int damagePlan;

    void Awake ()
    {
        SingletonSetInstance(this, true);
        Health = GetComponent<Health>();
        Health.Death += h => SceneManager.LoadScene("Victory");
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
