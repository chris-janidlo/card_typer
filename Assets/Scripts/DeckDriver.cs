using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckDriver : MonoBehaviour
{
    public TextAsset DeckJson;
    public int DrawSize;

    [Header("UI References")]
    public UICard CardObject;
    public Typer Typer;
    public RectTransform DrawContainer, ButtonContainer;
    public Button ResetOrder, StartTyping;
    public TextMeshProUGUI PlanPlace;

    [SerializeField]
    List<Card> deck;

    List<Card> library, handChoice;
    List<UICard> uiCards;
    int orderIndex;

    void Awake ()
    {
        deck = JsonUtility.FromJson<CardCollection>(DeckJson.text).Cards;

        ResetOrder.onClick.AddListener(onClickReset);
        StartTyping.onClick.AddListener(onClickStart);
    }

    public void StartDrawPhase ()
    {
        orderIndex = 0;
        handChoice = new List<Card>();
        library = new List<Card>(deck);

        var draw = getDraw();

        uiCards = new List<UICard>();
        foreach (var card in draw)
        {
            var cardInstance = Instantiate(CardObject, DrawContainer);
            cardInstance.Card = card;
            cardInstance.Button.onClick.AddListener(() => onClickCard(cardInstance));
            uiCards.Add(cardInstance);
        }

        PlanPlace.text = $"Enemy will hurt for {Enemy.Instance.DeviseDamagePlan()}";
    }

    List<Card> getDraw ()
    {
        List<Card> draw = new List<Card>();
        int remainder = DrawSize;

        if (library.Count < DrawSize)
        {
            draw = new List<Card>(library);
            remainder -= library.Count;
            library = new List<Card>(deck.Except(library));
        }

        for (int i = 0; i < remainder; i++)
        {
            var nextCard = library[Random.Range(0, library.Count)];
            library.Remove(nextCard);
            draw.Add(nextCard);
        }
        
        return draw;
    }

    void onClickCard (UICard sender)
    {
        sender.SelectionOrder.text = $"({++orderIndex})";
        sender.Button.interactable = false;
        handChoice.Add(sender.Card);

        ButtonContainer.gameObject.SetActive(true);
    }

    void onClickReset ()
    {
        orderIndex = 0;

        foreach (var uiCard in uiCards)
        {
            uiCard.Button.interactable = true;
            uiCard.SelectionOrder.text = "";
        }

        handChoice = new List<Card>();

        ButtonContainer.gameObject.SetActive(false);
    }

    void onClickStart ()
    {
        foreach (var card in uiCards)
        {
            Destroy(card.gameObject);
        }
        ButtonContainer.gameObject.SetActive(false);
        PlanPlace.text = "";
        Typer.StartTypingPhase(handChoice);
    }
}
