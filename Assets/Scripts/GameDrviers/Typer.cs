using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Typer : MonoBehaviour
{
    public float TypingTime;

    [Header("UI References")]
    public UICard CardPreviewObject;
    public Drawer DeckDriver;
    public Image TimeBar;
    public RectTransform PreviewLane, CurrentCardLocation;
    public TextMeshProUGUI ProgressIndicator, CountdownText;

    [SerializeField]
    string _progress;
    public string Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            ProgressIndicator.text = _progress;
        }
    }

    public float Timer { get; private set; }
    public List<Card> Cards { get; private set; }

    public Card CurrentCard => Cards[0];

    List<UICard> previews;
    bool inTypingPhase = false;

    void Update ()
    {
        if (!inTypingPhase) return;

        Timer -= Time.deltaTime;
        TimeBar.fillAmount = Mathf.Clamp(Timer / TypingTime, 0, 1);
        if (Timer <= 0)
        {
            endTypingPhase();
        }
    }

    void OnGUI ()
    {
        Event e = Event.current;

        if (e.type != EventType.KeyDown) return;

        if (e.keyCode == KeyCode.Space || e.keyCode == KeyCode.Return)
        {
            if (Progress.Equals(CurrentCard.Name))
            {
                popCard();
            }
            else
            {
                // TODO: feedback to player that they tried to confirm bad input
            }
            return;
        }

        if (e.keyCode == KeyCode.Backspace && Progress.Length > 0)
        {
            Progress = Progress.Substring(0, Progress.Length - 1);
            return;
        }

        if (System.Char.IsLetter(e.character))
        {
            Progress += e.character;
        }
    }

    public void StartTypingPhase (List<Card> cards)
    {
        StartCoroutine(countDownRoutine(cards));
    }

    IEnumerator countDownRoutine (List<Card> cards)
    {
        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        initializePhase(cards);

        CountdownText.text = "Go";
        yield return new WaitForSeconds(1);
        CountdownText.text = "";
    }

    void initializePhase (List<Card> cards)
    {
        Cards = cards;
        Progress = "";
        Timer = TypingTime;

        previews = new List<UICard>();
        foreach (var card in cards)
        {
            var uiCard = Instantiate(CardPreviewObject, PreviewLane);
            uiCard.Button.interactable = false;
            uiCard.Card = card;
            previews.Add(uiCard);
        }

        previews[0].transform.SetParent(CurrentCardLocation, false);

        inTypingPhase = true;
    }

    void endTypingPhase ()
    {
        foreach (var card in Cards)
        {
            Player.Instance.Health.IncrementValue(-card.Burn, "The word " + card.Name, "burnt");
        }
        Player.Instance.Health.IncrementValue(-Enemy.Instance.GetDamagePlan(), "The enemy", "hurt");

        foreach (var card in previews)
        {
            Destroy(card.gameObject);
        }

        TimeBar.fillAmount = 0;
        Progress = "";

        DeckDriver.StartDrawPhase();

        inTypingPhase = false;
    }

    void popCard ()
    {
        Enemy.Instance.Health.IncrementValue(-CurrentCard.Damage, "You", "hurt");
        
        Progress = "";

        Cards.RemoveAt(0);

        if (Cards.Count == 0)
        {
            if (Timer > 0)
            {
                Debug.Log("Time bonus: " + (Timer / TypingTime) * 100);
                endTypingPhase();
            }
            return;
        }

        Destroy(previews[0].gameObject);
        previews.RemoveAt(0);
        previews[0].transform.SetParent(CurrentCardLocation, false);
    }
}
