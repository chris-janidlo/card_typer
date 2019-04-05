using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class Drawer : MonoBehaviour
{
    [Header("Gameplay Values")]
    public TextAsset DeckJson;

    [Header("Deck Tags")]
    public TagPair PunctuationTag;
    public TagPair NonCardWordTag, DiscardedTag, DrawnTag, OtherCardTag, SelectedCardTag;
    public LinkTag CardLinkTag;

    [Header("UI")]
    public Typer Typer;
    public Player Player;
    public Enemy Enemy;
    public float FadeInTime, FadeOutTime;
    public RectTransform ButtonContainer;
    public Button ResetOrder, StartTyping;
    public TextMeshProUGUI DeckText;

    [SerializeField]
    Deck deck;

    List<Card> handSelection;
    List<int> selectedIndices;

    int orderIndex;

    void Awake ()
    {
        deck = Deck.FromJson(DeckJson);

        handSelection = new List<Card>();
        selectedIndices = new List<int>();

        ResetOrder.onClick.AddListener(onClickReset);
        StartTyping.onClick.AddListener(onClickStart);
        deck.TaggedTextChanged += onDeckChange;

        DeckText.CrossFadeAlpha(0, 0, false);
    }

    void Update ()
    {
        var hover = getHoveredCard();
        var hoverI = getHoveredIndex();

        Tooltip.Instance.SetCard(hover);

        if (hover != null && deck.GetCurrentDraw().Contains(hover) && !selectedIndices.Contains(hoverI) && Input.GetMouseButtonUp(0))
        {
            handSelection.Add(hover);
            selectedIndices.Add(hoverI);

            constructDeckString();

            string handText = listToCommaSeparated("Your current hand is ", handSelection.Select(h => h.Word).ToList());

            EventBox.Log($"\nYou selected {hover.Word}. {handText}");

            ButtonContainer.gameObject.SetActive(true);
        }
    }

    void OnDestroy ()
    {
        deck.TaggedTextChanged -= onDeckChange;
    }

    public void StartDrawPhase ()
    {
        EventBox.Log("\n");
        
        Player.LogStatus();
        Enemy.LogStatus();

        EventBox.Log("\n\nThe draw phase has started.");

        EventBox.Log($"\nThe demon plans on hurting you for {Enemy.DeviseDamagePlan()}.\n");

        deck.DrawNewHand(Player.HandSize);

        DeckText.CrossFadeAlpha(1, FadeInTime, true);
    }

    void onClickReset ()
    {
        handSelection = new List<Card>();
        selectedIndices = new List<int>();
        ButtonContainer.gameObject.SetActive(false);

        constructDeckString();

        EventBox.Log("\nYou cleared your hand.");
    }

    void onClickStart ()
    {
        ButtonContainer.gameObject.SetActive(false);

        Typer.StartTypingPhase(handSelection);

        handSelection = new List<Card>();
        selectedIndices = new List<int>();

        DeckText.CrossFadeAlpha(0, FadeOutTime, true);
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
        
        for (int i = 0; i < deck.TaggedText.Count; i++)
        {
            var tt = deck.TaggedText[i];
            string toAdd;

            if (selectedIndices.Contains(i))
            {
                toAdd = SelectedCardTag.Wrap(CardLinkTag.Wrap(tt.Word, i));
            }
            else
            {
                switch (tt.Status)
                {
                    case Deck.WordStatus.Punctuation:
                        toAdd = PunctuationTag.Wrap(tt.Word);
                        break;

                    case Deck.WordStatus.NonCardWord:
                        toAdd = NonCardWordTag.Wrap(tt.Word);
                        break;

                    case Deck.WordStatus.Discarded:
                        toAdd = DiscardedTag.Wrap(CardLinkTag.Wrap(tt.Word, i));
                        break;

                    case Deck.WordStatus.OtherCard:
                        toAdd = OtherCardTag.Wrap(CardLinkTag.Wrap(tt.Word, i));
                        break;

                    case Deck.WordStatus.Drawn:
                        toAdd = DrawnTag.Wrap(CardLinkTag.Wrap(tt.Word, i));
                        break;
                    
                    default:
                        throw new Exception("Unexpected default when switching on " + tt.Status);
                }                
            }

            richText += toAdd;
        }

        DeckText.text = richText;
    }

    Card getHoveredCard ()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(DeckText, Input.mousePosition, CameraCache.Main);

        if (linkIndex == -1) return null;

        string hoveredWord = DeckText.textInfo.linkInfo[linkIndex].GetLinkText();
        return deck.GetCard(hoveredWord);
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
