using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using crass;

[RequireComponent(typeof(Health))]
public class Player : Singleton<Player>
{
    public Health Health { get; private set; }

    void Awake ()
    {
        SingletonSetInstance(this, true);
        Health = GetComponent<Health>();
        Health.Death += h => SceneManager.LoadScene("Loss");
    }
}
