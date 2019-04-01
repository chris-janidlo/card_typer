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

        public TaggedWord (string word, WordStatus status, Card card)
        {
            Word = word;
            Status = status;
            Card = card;
        }

        public string ToTagString ()
        {
            return $"'{Word}': {Status}";
        }
    }

    public event Action TaggedTextChanged;

    List<TaggedWord> taggedText;
    public ReadOnlyCollection<TaggedWord> TaggedText => taggedText.AsReadOnly();

    [SerializeField]
    string fullText;

    [SerializeField]
    List<Card> cardData;

    Dictionary<string, Card> cardDataLookup;

    public static Deck FromJson (TextAsset source)
    {
        Deck deck = JsonUtility.FromJson<Deck>(source.text);
        deck.cardDataLookup = deck.cardData.ToDictionary(cd => cd.Word, cd => cd);
        deck.tagText();
        return deck;
    }

    public override string ToString ()
    {
        string output = "";
        foreach (var tt in taggedText)
        {
            output += tt.ToTagString() + "\n";
        }
        return output;
    }

    // FIXME: doesn't properly loop graveyard back into library when library is too small for a full hand
    public void DrawNewHand (int drawSize)
    {
		for (int i = 0; i < taggedText.Count; i++)
        {
            if (taggedText[i].Status == WordStatus.Drawn)
            {
                var tt = taggedText[i];
                tt.Status = WordStatus.Discarded;
                taggedText[i] = tt;
            }
        }
        
        int remainder = drawSize;

        var library = taggedText.Where(t => t.Status == WordStatus.OtherCard);

        if (library.Count() < drawSize)
        {
            for (int i = 0; i < taggedText.Count; i++)
            {
                var tt = taggedText[i];
                WordStatus newStatus = tt.Status;
                if (tt.Status == WordStatus.Discarded)
                {
                    newStatus = WordStatus.OtherCard;
                }
                else if (tt.Status == WordStatus.OtherCard)
                {
                    newStatus = WordStatus.Drawn;
                }
                taggedText[i] = new TaggedWord(tt.Word, newStatus, tt.Card);
            }
            remainder -= library.Count();
        }

        for (int i = 0; i < remainder; i++)
        {
            // get list of indices on taggedText of words that are not drawn or discarded
            var indices = taggedText.Select((val, ind) => new { val, ind })
                                    .Where(x => x.val.Status == WordStatus.OtherCard)
                                    .Select(x => x.ind).ToList();
            var index = indices[UnityEngine.Random.Range(0, indices.Count)];
            
            var tt = taggedText[index];
            taggedText[index] = new TaggedWord(tt.Word, WordStatus.Drawn, tt.Card);
        }
        
        var temp = TaggedTextChanged;
        if (temp != null) temp();
    }

    public List<Card> GetCurrentDraw ()
    {
        return taggedText
            .Where(t => t.Status == WordStatus.Drawn)
            .Select(t => t.Card)
            .ToList();
    }

    public Card GetCard (string name)
    {
        Card value;
        return cardDataLookup.TryGetValue(name, out value) ? value : null;
    }

    void tagText ()
    {
        taggedText = new List<TaggedWord>();
        
        string currentWord = "";
        bool scanningPunctuation = !Char.IsLetter(fullText[0]);

        foreach (char c in fullText)
        {
            if (scanningPunctuation == Char.IsLetter(c))
            {
                WordStatus status;
                
                Card card;
                cardDataLookup.TryGetValue(currentWord, out card);

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
