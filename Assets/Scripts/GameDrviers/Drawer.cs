using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Drawer : MonoBehaviour
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
    Deck deck;

    int orderIndex;

    void Awake ()
    {
        deck = Deck.FromJson(DeckJson);

        ResetOrder.onClick.AddListener(onClickReset);
        StartTyping.onClick.AddListener(onClickStart);
        deck.TaggedTextChanged += onDeckChange;
    }

    void OnDestroy ()
    {
        deck.TaggedTextChanged -= onDeckChange;
    }

    public void StartDrawPhase ()
    {
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

    }

    void onDeckChange ()
    {
        Debug.Log(deck);
    }
}
