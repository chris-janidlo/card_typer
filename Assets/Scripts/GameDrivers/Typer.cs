using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using crass;

public class Typer : Singleton<Typer>
{
    public event Action OnEndPhase, OnStartPhase;

    public float TypingTime;
    public int CountdownTime;

    [Header("UI References")]
    public Drawer Drawer;
    public Player Player;
    public Enemy Enemy;
    public Image TyperBar;
    public TextMeshProUGUI ProgressIndicator, CurrentCardText, TimerText;
    public TagPair BadLetterTag;
    public float LineShakeMagnitude, LineShakeDuration;
    [Tooltip("Lower index => closer to the left/center")]
    public List<TextMeshProUGUI> UpcomingWords;
    [Tooltip("Lower index => closer to the right/center")]
    public List<TextMeshProUGUI> CompletedWords;
    public AudioSource CountdownSource;

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
    public float TimeLeftPercent => Timer / TypingTime;
    public List<Card> Cards { get; private set; }
    public int CardsCasted { get; private set; }

    public Card CurrentCard => Cards[0];

    bool inTypingPhase = false;

    Vector2 lineStartPos;

    void Awake ()
    {
        // TODO: unsubscribe all events before resetting?
        SingletonSetInstance(this, true);
        lineStartPos = TyperBar.transform.localPosition;
    }

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

        if (!e.isKey || e.type != EventType.KeyDown) return;

        if (e.keyCode == KeyCode.Space || e.keyCode == KeyCode.Return)
        {
            if (Progress.Equals(CurrentCard.Word))
            {
                popCard();
            }
            else
            {
                StartCoroutine(lineShaker());
                TypingSounds.Instance.PlayDud();
            }
            return;
        }

        if (e.keyCode == KeyCode.Backspace)
        {
            if (Progress.Length == 0)
            {
                TypingSounds.Instance.PlayDud();
                return;
            }
            
            TypingSounds.Instance.PlayLetter();
            int delLength = 1;
            if (Progress[Progress.Length - 1] == '>')
            {
                // get the length of a character plus a tag pair (use of c here is arbitrary)
                delLength = BadLetterTag.Wrap("c").Length;
            }
            Progress = Progress.Substring(0, Progress.Length - delLength);
            return;
        }

        char typed = e.character;

        // shoo away the weird ghost characters
        // seriously everything breaks if this isn't here because UGUI is haunted
        if (!Char.IsLetter(typed) && !Char.IsDigit(typed) && !Char.IsPunctuation(typed)) return;

        if (CurrentCard.Word.StartsWith(Progress + typed))
        {
            Progress += typed;
            TypingSounds.Instance.PlayLetter();
        }
        else
        {
            Progress += BadLetterTag.Wrap(typed);
            TypingSounds.Instance.PlayDud();
        }
    }

    public void StartTypingPhase (List<Card> cards)
    {
        Cards = new List<Card>(cards);

        EventBox.Log("\n\n<b>The typing phase has started.</b>");
        if (OnStartPhase != null) OnStartPhase();
        StartCoroutine(lineScaler());
        StartCoroutine(countDownRoutine(cards));
    }

    IEnumerator countDownRoutine (List<Card> cards)
    {
        for (int i = CountdownTime; i > 0; i--)
        {
            TimerText.text = i.ToString();
            CountdownSource.Play();
            yield return new WaitForSeconds(1);
        }

        CountdownSource.Play();

        initializePhase();
    }

    void initializePhase ()
    {
        Progress = "";
        Timer = TypingTime;
        CardsCasted = 0;

        CurrentCardText.text = CurrentCard.Word;
        for (int i = 1; i < Cards.Count; i++)
        {
            UpcomingWords[i - 1].text = Cards[i].Word;
        }

        inTypingPhase = true;
    }

    void endTypingPhase ()
    {
        CardSelectSounds.Instance.StopAllSounds(Cards.Count > 0);

        EventBox.Log("\n\n");
        foreach (var card in Cards)
        {
            Player.IncrementHealth(-card.Burn, "The word " + card.Word, "burnt");
        }

        castEnemyCards();

        if (OnEndPhase != null) OnEndPhase();

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

    void castEnemyCards ()
    {
        var cards = Enemy.GetHand();
        if (cards.Count == 0)
        {
            EventBox.Log($"\n{Enemy.SubjectName} failed to cast any spells.");
        }
        else
        {
            foreach (var card in cards)
            {
                CardsCasted++;
                card.DoBehavior(Enemy, Player);
            }
        }
    }

    void popCard ()
    {
        TypingSounds.Instance.PlayWord();

        CurrentCard.DoBehavior(Player, Enemy);

        Progress = "";

        CardsCasted++;

        for (int i = 0; i < UpcomingWords.Count - 1; i++)
        {
            UpcomingWords[i].text = UpcomingWords[i + 1].text;
        }
        
        for (int i = CompletedWords.Count - 1; i > 0; i--)
        {
            CompletedWords[i].text = CompletedWords[i - 1].text;
        }

        CompletedWords[0].text = CurrentCard.Word;

        Cards.RemoveAt(0);

        if (Cards.Count == 0)
        {
            if (Timer > 0)
            {
                Debug.Log("Time bonus: " + (TimeLeftPercent) * 100);
                endTypingPhase();
            }
            return;
        }

        CurrentCardText.text = CurrentCard.Word;
    }

    IEnumerator lineScaler ()
    {
        TyperBar.enabled = true;

        TyperBar.rectTransform.sizeDelta = new Vector2
        (
            0,
            TyperBar.rectTransform.sizeDelta.y
        );

        while (TyperBar.rectTransform.sizeDelta.x < Screen.width)
        {
            TyperBar.rectTransform.sizeDelta += Vector2.right * ((float) Screen.width / CountdownTime) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator lineShaker ()
    {
        float timer = LineShakeDuration;

        while (timer > 0)
        {
            TyperBar.transform.localPosition = lineStartPos + UnityEngine.Random.insideUnitCircle * LineShakeMagnitude * (timer / LineShakeDuration);
            timer -= Time.deltaTime;
            yield return null;
        }

        TyperBar.transform.localPosition = lineStartPos;
    }
}
