using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public abstract class Deck
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

    protected abstract string fullText { get; }
    // protected abstract List<Card> cardData { get; }

    Dictionary<string, Card> _cardDataLookup = new Dictionary<string, Card>();

    public override string ToString ()
    {
        string output = "";
        foreach (var tt in taggedText)
        {
            output += tt.ToTagString() + "\n";
        }
        return output;
    }

    public void TagText ()
    {
        taggedText = new List<TaggedWord>();
        
        string currentWord = "";
        bool scanningPunctuation = !Char.IsLetter(fullText[0]);

        Action addWord = () => {
            WordStatus status;
            
            Card card = GetCard(currentWord);

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
        };

        foreach (char c in fullText)
        {
            if (scanningPunctuation == Char.IsLetter(c))
            {
                addWord();
                scanningPunctuation = !scanningPunctuation;
            }
            currentWord += c;
        }

        addWord();
    }

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

        int librarySize = taggedText.Where(t => t.Status == WordStatus.OtherCard).Count();

        if (librarySize < drawSize)
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
            remainder -= librarySize;
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

        if (_cardDataLookup.TryGetValue(name, out value))
        {
            return value;
        }
        else
        {
            var type = Type.GetType(name);
            var card = type == null ? null : (Card) Activator.CreateInstance(type);
            _cardDataLookup[name] = card;
            return card; 
        }
    }
}
