using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using crass;

public class Player : Agent
{
    void Start ()
    {
        Death += a => SceneManager.LoadScene("Loss");
    }
}
