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
    public int DrawSize;

    [Header("Deck Tags")]
    public TagPair PunctuationTag;
    public TagPair NonCardWordTag, DiscardedTag, DrawnTag, OtherCardTag;
    public LinkTag DrawnCardLink, RegularCardLink;

    [Header("UI References")]
    public UICard CardObject;
    public Typer Typer;
    public RectTransform DrawContainer, ButtonContainer;
    public Button ResetOrder, StartTyping;
    public TextMeshProUGUI DeckText;

    [SerializeField]
    Deck deck;

    int orderIndex;

    [Serializable]
    public struct TagPair
    {
        public string Start, End;

        public string Wrap (string target)
        {
            return Start + target + End;
        }
    }

    [Serializable]
    public struct LinkTag
    {
        public string ID;

        public string Wrap (string target, int index)
        {
            return $"<link=\"{ID}:{target}:{index}\">{target}</link>";
        }
    }

    bool go = false;

    void Awake ()
    {
        deck = Deck.FromJson(DeckJson);

        ResetOrder.onClick.AddListener(onClickReset);
        StartTyping.onClick.AddListener(onClickStart);
        deck.TaggedTextChanged += onDeckChange;
    }

    IEnumerator Start ()
    {
        yield return new WaitForSeconds(1);
        go = true;
    }

    void Update ()
    {
        Tooltip.Instance.SetCard(getHoveredCard());
    }

    void OnDestroy ()
    {
        deck.TaggedTextChanged -= onDeckChange;
    }

    public void StartDrawPhase ()
    {
        EventBox.Log("\n\nThe draw phase has started.");
        deck.DrawNewHand(DrawSize);
    }

    void onClickCard (UICard sender)
    {

    }

    void onClickReset ()
    {

    }

    void onClickStart ()
    {
        DeckText.text = "";
    }

    void onDeckChange ()
    {
        var draw = deck.GetCurrentDraw();
        string drawString = $"\nYou drew {draw[0].Name}";
        for (int i = 1; i < draw.Count - 1; i++)
        {
            drawString += $", {draw[i].Name}";
        }
        EventBox.Log(drawString + $", and {draw[draw.Count - 1].Name}.");

        DeckText.text = constructDeckString();
    }

    string constructDeckString ()
    {
        string richText = "";
        
        for (int i = 0; i < deck.TaggedText.Count; i++)
        {
            var tt = deck.TaggedText[i];
            string toAdd;

            switch (tt.Status)
            {
                case Deck.WordStatus.Punctuation:
                    toAdd = PunctuationTag.Wrap(tt.Word);
                    break;

                case Deck.WordStatus.NonCardWord:
                    toAdd = NonCardWordTag.Wrap(tt.Word);
                    break;

                case Deck.WordStatus.Discarded:
                    toAdd = DiscardedTag.Wrap(RegularCardLink.Wrap(tt.Word, i));
                    break;

                case Deck.WordStatus.OtherCard:
                    toAdd = OtherCardTag.Wrap(RegularCardLink.Wrap(tt.Word, i));
                    break;

                case Deck.WordStatus.Drawn:
                    toAdd = DrawnTag.Wrap(DrawnCardLink.Wrap(tt.Word, i));
                    break;
                
                default:
                    throw new Exception("Unexpected default when switching on " + tt.Status);
            }

            richText += toAdd;
        }

        return richText;
    }

    Card getHoveredCard ()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(DeckText, Input.mousePosition, null);

        if (linkIndex == -1) return null;

        string hoveredWord = DeckText.textInfo.linkInfo[linkIndex].GetLinkText();
        return deck.GetCard(hoveredWord);
    }
}
