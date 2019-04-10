using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class Deck
{
    public enum TextStatus
    {
        Card,
        DrawnCard,
        DiscardedCard,
        NonCardText
    }

    public struct TaggedText
    {
        public string Text;
        public TextStatus Status;
        public Card Card;

        public TaggedText (string word, TextStatus status, Card card)
        {
            Text = word;
            Status = status;
            Card = card;
        }

        public string ToTagString ()
        {
            return $"'{Text}': {Status}";
        }
    }

    public event Action TaggedTextChanged;

    List<TaggedText> taggedTexts;
    public ReadOnlyCollection<TaggedText> TaggedTexts => taggedTexts.AsReadOnly();

    protected abstract string bracketedText { get; }
    protected abstract List<Card> cardList { get; }

    public static Deck FromClassString (string name)
    {
        Type type = Type.GetType(name) ?? Type.GetType("Deck" + name);
        Deck value = (Deck) Activator.CreateInstance(type);
        value.tagText();
        return value;
    }

    public override string ToString ()
    {
        string output = "";
        foreach (var tt in taggedTexts)
        {
            output += tt.ToTagString() + "\n";
        }
        return output;
    }

    public void DrawNewHand (int drawSize)
    {
		for (int i = 0; i < taggedTexts.Count; i++)
        {
            if (taggedTexts[i].Status == TextStatus.DrawnCard)
            {
                var tt = taggedTexts[i];
                tt.Status = TextStatus.DiscardedCard;
                taggedTexts[i] = tt;
            }
        }
        
        int remainder = drawSize;

        int librarySize = taggedTexts.Where(t => t.Status == TextStatus.Card).Count();

        if (librarySize < drawSize)
        {
            for (int i = 0; i < taggedTexts.Count; i++)
            {
                var tt = taggedTexts[i];
                TextStatus newStatus = tt.Status;
                if (tt.Status == TextStatus.DiscardedCard)
                {
                    newStatus = TextStatus.Card;
                }
                else if (tt.Status == TextStatus.Card)
                {
                    newStatus = TextStatus.DrawnCard;
                }
                taggedTexts[i] = new TaggedText(tt.Text, newStatus, tt.Card);
            }
            remainder -= librarySize;
        }

        for (int i = 0; i < remainder; i++)
        {
            // get list of indices on taggedText of words that are not drawn or discarded
            var indices = taggedTexts.Select((val, ind) => new { val, ind })
                                    .Where(x => x.val.Status == TextStatus.Card)
                                    .Select(x => x.ind).ToList();
            var index = indices[UnityEngine.Random.Range(0, indices.Count)];
            
            var tt = taggedTexts[index];
            taggedTexts[index] = new TaggedText(tt.Text, TextStatus.DrawnCard, tt.Card);
        }
        
        var temp = TaggedTextChanged;
        if (temp != null) temp();
    }

    public List<Card> GetCurrentDraw ()
    {
        return taggedTexts
            .Where(t => t.Status == TextStatus.DrawnCard)
            .Select(t => t.Card)
            .ToList();
    }

    void tagText ()
    {
        taggedTexts = new List<TaggedText>();
        
        int wordIndex = 0;
        string currentText = "";
        bool scanningCard = bracketedText[0] == '{', atStart = true;

        Action addWord = () => {
            Card card = null;

            if (scanningCard)
            {
                card = cardList[wordIndex++];
                card.Word = currentText;
            }

            TaggedText next = new TaggedText
            {
                Text = currentText,
                Status = scanningCard ? TextStatus.Card : TextStatus.NonCardText,
                Card = card
            };
            taggedTexts.Add(next);

            currentText = "";
        };

        foreach (char c in bracketedText)
        {
            switch (c)
            {
                case '{':
                    if (scanningCard) throw new Exception("unexpected {");
                    if (!atStart) addWord();
                    scanningCard = true;
                    break;
                
                case '}':
                    if (!scanningCard) throw new Exception("unexpected }");
                    addWord();
                    scanningCard = false;
                    break;
                
                default:
                    currentText += c;
                    break;
            }
            atStart = false;
        }

        addWord();
    }
}
