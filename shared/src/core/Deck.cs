using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CTShared
{
public class Deck
{
    List<Card> _cards;
    public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

    public Agent Owner { get; private set; }

    List<Card> hand, drawPile, discardPile;
    public ReadOnlyCollection<Card> Hand => hand.AsReadOnly();
    public ReadOnlyCollection<Card> DrawPile => drawPile.AsReadOnly();
    public ReadOnlyCollection<Card> DiscardPile => discardPile.AsReadOnly();

    public string BracketedText { get; private set; }

    Random rand;

    public Deck (string bracketedText, Agent owner)
    {
        Owner = owner;

        rand = new Random();
        _cards = new List<Card>();
        discardPile = new List<Card>();

        string currentText = "";
        bool scanningCard = bracketedText[0] == '{';

        foreach (char c in bracketedText)
        {
            switch (c)
            {
                case '{':
                    if (scanningCard) throw new Exception("unexpected {");
                    scanningCard = true;
                    break;
                
                case '}':
                    if (!scanningCard) throw new Exception("unexpected }");
                    scanningCard = false;

                    var split = currentText.Split(':');
                    if (split.Count() != 2) throw new Exception("expected exactly one :");

                    Card card = Card.FromName(split[1], owner);
                    card.Word = split[0];
                    _cards.Add(card);

                    currentText = "";
                    break;
                
                default:
                    if (scanningCard) currentText += c;
                    break;
            }
        }
    }

    public void DrawNewHand (int size)
    {
        discardPile.AddRange(hand);

        hand = new List<Card>();

        for (int i = 0; i < size; i++)
        {
            if (drawPile.Count == 0)
            {
                drawPile = new List<Card>(Cards);
                discardPile = new List<Card>();
            }

            int index = rand.Next(drawPile.Count);
            hand.Add(drawPile[index]);
            drawPile.RemoveAt(index);
        }
    }
}
}
