using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Typer : MonoBehaviour
{
    public float TypingTime;

    [Header("UI References")]
    public Drawer Drawer;
    public Image TyperBar;
    public TextMeshProUGUI ProgressIndicator, CurrentCardText, TimerText;
    [Tooltip("Lower index => closer to the left/center")]
    public List<TextMeshProUGUI> UpcomingWords;
    [Tooltip("Lower index => closer to the right/center")]
    public List<TextMeshProUGUI> CompletedWords;

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

    bool inTypingPhase = false;

    void Update ()
    {
        if (!inTypingPhase) return;

        Timer -= Time.deltaTime;
        TimerText.text = Mathf.Ceil(Timer).ToString();
        if (Timer <= 0)
        {
            endTypingPhase();
        }
    }

    void OnGUI ()
    {
        if (!inTypingPhase) return;

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
            // TODO: highlight bad characters in red
            Progress += e.character;
        }
    }

    public void StartTypingPhase (List<Card> cards)
    {
        EventBox.Log("\n\nThe typing phase has started.");
        StartCoroutine(countDownRoutine(cards));
    }

    IEnumerator countDownRoutine (List<Card> cards)
    {
        for (int i = 3; i > 0; i--)
        {
            TimerText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        initializePhase(cards);
    }

    void initializePhase (List<Card> cards)
    {
        Cards = cards;
        Progress = "";
        Timer = TypingTime;

        CurrentCardText.text = CurrentCard.Name;
        for (int i = 1; i < cards.Count; i++)
        {
            UpcomingWords[i - 1].text = cards[i].Name;
        }

        TyperBar.enabled = true;

        inTypingPhase = true;
    }

    void endTypingPhase ()
    {
        EventBox.Log("\n\n");
        foreach (var card in Cards)
        {
            Player.Instance.Health.IncrementValue(-card.Burn, "The word " + card.Name, "burnt");
        }
        Player.Instance.Health.IncrementValue(-Enemy.Instance.GetDamagePlan(), "The enemy", "hurt");

        foreach (var text in UpcomingWords)
        {
            text.text = "";
        }

        foreach (var text in CompletedWords)
        {
            text.text = "";
        }

        CurrentCardText.text = "";
        Progress = "";
        TimerText.text = "";

        TyperBar.enabled = false;

        Drawer.StartDrawPhase();

        inTypingPhase = false;
    }

    void popCard ()
    {
        EventBox.Log($"\nYou casted {CurrentCard.Name}.");

        Enemy.Instance.Health.IncrementValue(-CurrentCard.Damage, " You", "hurt");
        
        Progress = "";

        for (int i = 0; i < UpcomingWords.Count - 1; i++)
        {
            UpcomingWords[i].text = UpcomingWords[i + 1].text;
        }
        
        for (int i = CompletedWords.Count - 1; i > 0; i--)
        {
            CompletedWords[i].text = CompletedWords[i - 1].text;
        }

        CompletedWords[0].text = CurrentCard.Name;

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

        CurrentCardText.text = CurrentCard.Name;
    }
}
