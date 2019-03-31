using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using crass;

public class EventBox : Singleton<EventBox>
{
    public float WordsPerMinute;

    [SerializeField]
    TextMeshProUGUI contents;

    float charactersPerSecond => WordsPerMinute * 5 / 60;

    string targetText;
    float characterIndex;

    void Awake ()
    {
        SingletonSetInstance(this, true);
        targetText = contents.text;
    }

    void Update ()
    {
        characterIndex += charactersPerSecond * Time.deltaTime;
        characterIndex = Mathf.Clamp(characterIndex, 0, targetText.Length);
        contents.text = targetText.Substring(0, (int) characterIndex);
    }

    public static void Log (string text)
    {
        Instance.targetText += text;
    }
}
