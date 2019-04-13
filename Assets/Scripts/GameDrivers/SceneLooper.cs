using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLooper : MonoBehaviour
{
    public float WaitTime = 1;
    public string TargetScene;

    bool ready = false;

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(WaitTime);
        ready = true;
    }

    void Update ()
    {
        if (!ready) return;

        if (Input.anyKey)
        {
            SceneManager.LoadScene(TargetScene);
        }
    }
}
