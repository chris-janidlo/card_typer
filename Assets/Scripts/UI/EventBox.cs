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
    public AudioSource LetterSource;
    public int CharactersPerSound;

    [SerializeField]
    TextMeshProUGUI contents;

    float charactersPerSecond => WordsPerMinute * 5 / 60 + (targetText.Length - contents.text.Length) * LagWeight;

    string targetText;
    float characterIndex;

    int soundCounter;

    void Awake ()
    {
        SingletonSetInstance(this, true);
        targetText = contents.text;
    }

    void Update ()
    {
        characterIndex += charactersPerSecond * Time.deltaTime;
        characterIndex = Mathf.Clamp(characterIndex, 0, targetText.Length);

        string newStr = targetText.Substring(0, (int) characterIndex);

        if (contents.text != newStr)
        {
            soundCounter--;
            if (soundCounter <= 0)
            {
                LetterSource.Play();
                soundCounter = CharactersPerSound;
            }
        }

        contents.text = newStr;
    }

    public static void Log (string text)
    {
        Instance.targetText += text;
    }
}
