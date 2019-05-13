using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class PlayerDrawer : IDrawer
{
    public string DeckClassName;

    [Header("Deck Tags")]
    public TagPair NonCardTag;
    public TagPair DiscardedCardTag, DrawnCardTag, CardTag, SelectedCardTag;

    [Header("UI")]
    public float FadeInTime;
    public float FadeOutTime;
    public RectTransform ButtonContainer;
    public Button ResetOrder, StartTyping;
    public TextMeshProUGUI DeckText;

    public override int HandSize { get; set; } = 7;

    Deck deck;

    List<Card> handSelection;
    List<int> selectedIndices;

    LinkTag linkTag = new LinkTag { ID = "taggedtext" };

    DecidedPlayCallback decidedPlayCallback;

    void Awake ()
    {
        deck = Deck.FromClassString(DeckClassName);
        deck.TaggedTextChanged += constructDeckString;

        handSelection = new List<Card>();
        selectedIndices = new List<int>();

        ResetOrder.onClick.AddListener(onClickReset);
        StartTyping.onClick.AddListener(onClickStart);

        DeckText.CrossFadeAlpha(0, 0, false);
    }

    void Update ()
    {
        int hoverI = getHoveredIndex();
        Deck.TaggedText? hoveredText = hoverI > 0 ? (Deck.TaggedText?) deck.TaggedTexts[hoverI] : null;
        Card hover = hoveredText?.Card;

        Tooltip.Instance.SetCard(hover);

        if (hover != null && deck.GetCurrentDraw().Contains(hover) && !selectedIndices.Contains(hoverI) && Input.GetMouseButtonUp(0))
        {
            handSelection.Add(hover);
            selectedIndices.Add(hoverI);

            constructDeckString();

            ButtonContainer.gameObject.SetActive(true);

            CardSelectSounds.Instance.PlayNewSound();
        }
    }

    void OnDestroy ()
    {
        deck.TaggedTextChanged -= constructDeckString;
    }

    public override void StartPhase (DecidedPlayCallback callback)
    {
        deck.DrawNewHand(HandSize);

        decidedPlayCallback = callback;

        DeckText.CrossFadeAlpha(1, FadeInTime, true);
    }

    void onClickReset ()
    {
        handSelection = new List<Card>();
        selectedIndices = new List<int>();
        ButtonContainer.gameObject.SetActive(false);

        constructDeckString();

        CardSelectSounds.Instance.StopAllSounds();
    }

    void onClickStart ()
    {
        ButtonContainer.gameObject.SetActive(false);

        DeckText.CrossFadeAlpha(0, FadeOutTime, true);

        decidedPlayCallback(handSelection);

        handSelection = new List<Card>();
        selectedIndices = new List<int>();
    }

    void constructDeckString ()
    {
        string richText = "";
        
        for (int i = 0; i < deck.TaggedTexts.Count; i++)
        {
            var tt = deck.TaggedTexts[i];
            string linked = linkTag.Wrap(tt.Text, i), toAdd;

            if (selectedIndices.Contains(i))
            {
                toAdd = SelectedCardTag.Wrap(linked);
            }
            else
            {
                switch (tt.Status)
                {
                    case Deck.TextStatus.NonCardText:
                        toAdd = NonCardTag.Wrap(linked);
                        break;

                    case Deck.TextStatus.DiscardedCard:
                        toAdd = DiscardedCardTag.Wrap(linked);
                        break;

                    case Deck.TextStatus.Card:
                        toAdd = CardTag.Wrap(linked);
                        break;

                    case Deck.TextStatus.DrawnCard:
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

    int getHoveredIndex ()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(DeckText, Input.mousePosition, CameraCache.Main);

        if (linkIndex == -1) return -1;

        string tag = DeckText.textInfo.linkInfo[linkIndex].GetLinkID();
        string id = tag.Substring(tag.LastIndexOf(":") + 1);
        return Int32.Parse(id);
    }
}
