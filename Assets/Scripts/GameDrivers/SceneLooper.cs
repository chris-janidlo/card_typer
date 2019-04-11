using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLooper : MonoBehaviour
{
    public string TargetScene;

    void Update ()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(TargetScene);
        }
    }
}
