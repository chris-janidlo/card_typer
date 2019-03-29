using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLooper : MonoBehaviour
{
    void Update ()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
