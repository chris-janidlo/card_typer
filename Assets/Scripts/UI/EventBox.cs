using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using crass;

public class EventBox : Singleton<EventBox>
{
    public float WordsPerMinute;
    [Tooltip("The text goes faster as a function of how far off it is; change this to change how much faster or slower it goes.")]
    public float LagWeight;

    [SerializeField]
    TextMeshProUGUI contents;

    float charactersPerSecond => WordsPerMinute * 5 / 60 + (targetText.Length - contents.text.Length) * LagWeight;

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
