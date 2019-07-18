using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CTShared.Networking;
using LiteNetLib.Utils;

namespace CTShared
{
public class Deck : Packet
{
    List<Card> _cards = new List<Card>();
    public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

    public Agent Owner { get; private set; }

    List<Card> hand = new List<Card>(),
               drawPile = new List<Card>(),
               discardPile = new List<Card>();

    public ReadOnlyCollection<Card> Hand => hand.AsReadOnly();
    public ReadOnlyCollection<Card> DrawPile => drawPile.AsReadOnly();
    public ReadOnlyCollection<Card> DiscardPile => discardPile.AsReadOnly();

    public string BracketedText { get; private set; }

    internal Deck (string bracketedText, Agent owner)
    {
        Owner = owner;

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
                    drawPile.Add(card);

                    currentText = "";
                    break;
                
                default:
                    if (scanningCard) currentText += c;
                    break;
            }
        }

        BracketedText = bracketedText;
    }

    // for packet serialization
    internal Deck (Agent owner)
    {
        Owner = owner;
    }

    internal void DrawNewHand (int size)
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

            int index = Owner.Manager.Rand.Next(drawPile.Count);
            hand.Add(drawPile[index]);
            drawPile.RemoveAt(index);
        }
    }

    internal override void Deserialize (NetDataReader reader)
    {
        BracketedText = reader.GetString();

        int numCards = reader.GetByte();

        _cards = new List<Card>();
        drawPile = new List<Card>();
        hand = new List<Card>();
        discardPile = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            var card = Card.FromName(reader.GetString(), Owner);
            card.Deserialize(reader); // get any internal state

            _cards.Add(card);

            switch ((CardStatus) reader.GetByte())
            {
                case CardStatus.DrawPile:
                    drawPile.Add(card);
                    break;
                
                case CardStatus.Hand:
                    hand.Add(card);
                    break;

                case CardStatus.DiscardPile:
                    discardPile.Add(card);
                    break;
                
                default:
                    throw new ParseException("packet contains byte that is not an expected CardStatus");
            }
        }
    }

    internal override void Serialize (NetDataWriter writer)
    {
        writer.Put(BracketedText);

        // atm, don't expect to see over 255 cards in any deck
        writer.Put((byte) Cards.Count);

        foreach (var card in _cards)
        {
            writer.Put(card.Name);
            card.Serialize(writer); // get any internal state

            byte status;
            
            if (drawPile.Contains(card))
            {
                status = (byte) CardStatus.DrawPile;
            }
            else if (hand.Contains(card))
            {
                status = (byte) CardStatus.Hand;
            }
            else if (discardPile.Contains(card))
            {
                status = (byte) CardStatus.DiscardPile;
            }
            else
            {
                throw new InvalidOperationException($"card {card.Word} is unexpectedly not in draw pile, hand, or discard pile");
            }

            writer.Put(status);
        }
    }
}

public enum CardStatus : byte
{
    DrawPile, Hand, DiscardPile
}
}
