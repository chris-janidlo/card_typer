using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class Deck
{
    public enum WordStatus
    {
        Punctuation,
        NonCardWord, // for words like "a", "the"
        Discarded,
        Drawn,
        OtherCard // for any card that is neither discarded nor drawn
    }

    public struct TaggedWord
    {
        public string Word;
        public WordStatus Status;
        public Card Card;
    }

    public event Action<Deck> TaggedTextChanged;

    List<TaggedWord> taggedText;
    public ReadOnlyCollection<TaggedWord> TaggedText => taggedText.AsReadOnly();

    [SerializeField]
    string fullText;

    [SerializeField]
    List<Card> cardData;

    public static Deck FromJson (TextAsset source)
    {
        Deck deck = JsonUtility.FromJson<Deck>(source.text);
        deck.tagText();
        return deck;
    }

    public void DrawNewHand (int drawSize)
    {
        List<Card> draw = new List<Card>();
        int remainder = drawSize;

        if (library.Count < drawSize)
        {
            draw = new List<Card>(library);
            remainder -= library.Count;
            library = new List<Card>(deck.Except(library));
        }

        for (int i = 0; i < remainder; i++)
        {
            var nextCard = library[Random.Range(0, library.Count)];
            library.Remove(nextCard);
            draw.Add(nextCard);
        }
        
        var temp = TaggedTextChanged;
        if (temp != null) temp(this);
    }

    void tagText ()
    {
        string currentWord = "";
        bool scanningPunctuation = Char.IsLetter(currentWord[0]);

        foreach (char c in fullText)
        {
            if (scanningPunctuation != Char.IsLetter(c))
            {
                WordStatus status;
                Card card = cardData.SingleOrDefault(cd => cd.Name == currentWord);

                if (scanningPunctuation)
                {
                    status = WordStatus.Punctuation;
                }
                else if (card != null)
                {
                    status = WordStatus.OtherCard;
                }
                else
                {
                    status = WordStatus.NonCardWord;
                }

                TaggedWord next = new TaggedWord
                {
                    Word = currentWord,
                    Status = status,
                    Card = card
                };
                taggedText.Add(next);

                currentWord = "";
                scanningPunctuation = !scanningPunctuation;
            }
            currentWord += c;
        }
    }
}
// struct TagPair
// {
//     string start, end;

//     public TagPair (string start, string end)
//     {
//         this.start = start;
//         this.end = end; 
//     }

//     public string Wrap (string target)
//     {
//         return start + target + end;
//     }
// }

// struct LinkTag
// {
//     TagPair tags;

//     public LinkTag (string id)
//     {
//         this.tags = new TagPair($"<link=\"{id}\">", "</link>");
//     }

//     public string Wrap (string target)
//     {
//         return tags.Wrap(target);
//     }
// }
