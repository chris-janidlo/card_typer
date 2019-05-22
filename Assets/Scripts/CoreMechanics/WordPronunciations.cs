using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class WordPronunciations : Singleton<WordPronunciations>
{
    [Serializable]
    public class Pronunciation
    {
        public string Word;
        public List<Stress> StressPattern;
        public List<string> Rhymes;

        public bool RhymesWith (string other)
        {
            return Word.Equals(other) || Rhymes.Contains(other);
        }
    }

    public Pronunciation this[string word] => dictView[word];

    public TextAsset DataAsset;

    [SerializeField]
    private List<Pronunciation> Data;

    Dictionary<string, Pronunciation> dictView => Data.ToDictionary(d => d.Word);

    void Awake ()
    {
        JsonUtility.FromJsonOverwrite(DataAsset.text, this);
    }
}

public enum Stress
{
	Unstressed, Primary, Secondary
}
