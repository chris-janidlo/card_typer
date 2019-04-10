using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class Drawer : Singleton<Drawer>
{
    public event Action OnEndPhase, OnStartPhase;

    [Header("Gameplay Values")]
    public string DeckClassName;

    [Header("Deck Tags")]
    public TagPair NonCardTag;
    public TagPair DiscardedCardTag, DrawnCardTag, CardTag, SelectedCardTag;

    [Header("UI")]
    public Typer Typer;
    public Player Player;
    public Enemy Enemy;
    public float FadeInTime, FadeOutTime;
    public RectTransform ButtonContainer;
    public Button ResetOrder, StartTyping;
    public TextMeshProUGUI DeckText;

    Deck deck;

    List<Card> handSelection;
    List<int> selectedIndices;

    public LinkTag linkTag = new LinkTag { ID = "taggedtext" };

    void Awake ()
    {
        // TODO: unsubscribe all events before resetting?
        SingletonSetInstance(this, true);

        deck = Deck.FromClassString(DeckClassName);
        deck.TaggedTextChanged += onDeckChange;

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

            string handText = listToCommaSeparated("Your current hand is ", handSelection.Select(h => h.Word).ToList());

            EventBox.Log($"\nYou selected {hover.Word}. {handText}");

            ButtonContainer.gameObject.SetActive(true);

            CardSelectSounds.Instance.PlayNewSound();
        }
    }

    void OnDestroy ()
    {
        deck.TaggedTextChanged -= onDeckChange;
    }

    public void StartDrawPhase ()
    {
        EventBox.Log("\n\n<b>The draw phase has started.</b>");
        
        EventBox.Log("\n");

        Player.LogStatus();
        Enemy.LogStatus();

        EventBox.Log($"\nThe demon plans on hurting you for {Enemy.DeviseDamagePlan()}.\n");

        deck.DrawNewHand(Player.HandSize);

        DeckText.CrossFadeAlpha(1, FadeInTime, true);

        if (OnStartPhase != null) OnStartPhase();
    }

    void onClickReset ()
    {
        handSelection = new List<Card>();
        selectedIndices = new List<int>();
        ButtonContainer.gameObject.SetActive(false);

        constructDeckString();

        EventBox.Log("\nYou cleared your hand.");

        CardSelectSounds.Instance.StopAllSounds();
    }

    void onClickStart ()
    {
        ButtonContainer.gameObject.SetActive(false);

        Typer.StartTypingPhase(handSelection);

        handSelection = new List<Card>();
        selectedIndices = new List<int>();

        DeckText.CrossFadeAlpha(0, FadeOutTime, true);

        if (OnEndPhase != null) OnEndPhase();
    }

    void onDeckChange ()
    {
        var draw = deck.GetCurrentDraw();
        
        EventBox.Log(listToCommaSeparated("\nYou drew ", draw.Select(d => d.Word).ToList()));

        constructDeckString();
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

    string listToCommaSeparated (string prelude, List<string> list)
    {
        string output = prelude + list[0];
        
        if (list.Count == 1) return output + ".";

        if (list.Count == 2) return $"{output} and {list[1]}.";

        for (int i = 1; i < list.Count - 1; i++)
        {
            output += $", {list[i]}";
        }
        
        return $"{output}, and {list[list.Count - 1]}.";
    }
}
