using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using CTShared;
using TMPro;
using crass;

public class DeckDisplay : MonoBehaviour
{
    public enum TextStatus
    {
        InDrawPile,
        InHand,
        InDiscardPile,
        NotACard
    }

    public class TaggedText
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

    [Header("Deck Tags")]
    public TagPair NonCardTag;
    public TagPair DiscardedCardTag, DrawnCardTag, CardTag, SelectedCardTag;

    [Header("UI")]
    public TypingDisplay Typer;
    public float FadeInTime;
    public float FadeOutTime;
    public RectTransform ButtonContainer;
    public Button ResetOrder, StartTyping;
    public TextMeshProUGUI DeckText;

    Deck deck;
    List<TaggedText> taggedTexts = new List<TaggedText>();

    List<Card> handSelection = new List<Card>();
    List<int> selectedIndices = new List<int>();

    LinkTag linkTag = new LinkTag { ID = "taggedtext" };

    void Start ()
    {
        ManagerContainer.Manager.OnDrawPhaseStart += startPhase;

        ResetOrder.onClick.AddListener(onClickReset);
        StartTyping.onClick.AddListener(onClickStart);

        DeckText.CrossFadeAlpha(0, 0, false);
    }

    void Update ()
    {
        int hoverI = getHoveredIndex();
        TaggedText hoveredText = hoverI > 0 ? taggedTexts[hoverI] : null;
        Card hover = hoveredText?.Card;

        Tooltip.Instance.SetCard(hover);

        if (hover != null && deck.Hand.Contains(hover) && !selectedIndices.Contains(hoverI) && Input.GetMouseButtonUp(0))
        {
            handSelection.Add(hover);
            selectedIndices.Add(hoverI);

            constructDeckString();

            ButtonContainer.gameObject.SetActive(true);

            CardSelectSounds.Instance.PlayNewSound();
        }
    }

    public void Initialize (Deck deck)
    {
        this.deck = deck;
        scanText();
    }

    void startPhase ()
    {
        reTagText();
        constructDeckString();

        handSelection = new List<Card>();
        selectedIndices = new List<int>();

        DeckText.CrossFadeAlpha(1, FadeInTime, true);
    }

    void onClickReset ()
    {
        constructDeckString();

        handSelection = new List<Card>();
        selectedIndices = new List<int>();

        ButtonContainer.gameObject.SetActive(false);
        CardSelectSounds.Instance.StopAllSounds();
    }

    void onClickStart ()
    {
        Typer.SetPlay(handSelection);
        ManagerContainer.Manager.ReadyUp(deck.Owner, handSelection);

        ButtonContainer.gameObject.SetActive(false);
        DeckText.CrossFadeAlpha(0, FadeOutTime, true);
    }

    void scanText ()
    {
        taggedTexts = new List<TaggedText>();
        
        int wordIndex = 0;
        string currentText = "";
        bool scanningCard = deck.BracketedText[0] == '{', atStart = true;

        Action addWord = () => {
            Card card = null;
            string word = currentText;

            if (scanningCard)
            {
                card = deck.Cards[wordIndex++];
                word = currentText.Split(':')[0];
            }

            TaggedText next = new TaggedText
            (
                word,
                scanningCard ? TextStatus.InDrawPile : TextStatus.NotACard,
                card
            );
            taggedTexts.Add(next);

            currentText = "";
        };

        foreach (char c in deck.BracketedText)
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

    void reTagText ()
    {
        Action<ReadOnlyCollection<Card>, TextStatus> reTag = (col, stat) =>
        {
            foreach (var card in col)
            {
                taggedTexts.Single(tt => tt.Card == card).Status = stat;
            }
        };

        reTag(deck.DrawPile, TextStatus.InDrawPile);
        reTag(deck.Hand, TextStatus.InHand);
        reTag(deck.DiscardPile, TextStatus.InDiscardPile);

        constructDeckString();
    }

    int getHoveredIndex ()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(DeckText, Input.mousePosition, CameraCache.Main);

        if (linkIndex == -1) return -1;

        string tag = DeckText.textInfo.linkInfo[linkIndex].GetLinkID();
        string id = tag.Substring(tag.LastIndexOf(":") + 1);
        return Int32.Parse(id);
    }

    void constructDeckString ()
    {
        string richText = "";
        
        for (int i = 0; i < taggedTexts.Count; i++)
        {
            var tt = taggedTexts[i];
            string linked = linkTag.Wrap(tt.Text, i), toAdd;

            if (selectedIndices.Contains(i))
            {
                toAdd = SelectedCardTag.Wrap(linked);
            }
            else
            {
                switch (tt.Status)
                {
                    case TextStatus.NotACard:
                        toAdd = NonCardTag.Wrap(linked);
                        break;

                    case TextStatus.InDiscardPile:
                        toAdd = DiscardedCardTag.Wrap(linked);
                        break;

                    case TextStatus.InDrawPile:
                        toAdd = CardTag.Wrap(linked);
                        break;

                    case TextStatus.InHand:
                        toAdd = DrawnCardTag.Wrap(linked);
                        break;
                    
                    default:
                        throw new Exception("Unexpected default when switching on " + tt.Status);
                }                
            }

            richText += toAdd;
        }

        DeckText.text = richText;
    }
}
