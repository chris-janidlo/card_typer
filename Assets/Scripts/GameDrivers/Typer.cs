using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Typer : MonoBehaviour
{
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
    public AudioClip CountdownClip;

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

    Vector2 lineStartPos;

    void Awake ()
    {
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

        if (e.type != EventType.KeyDown) return;

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

        if (e.keyCode == KeyCode.Backspace && Progress.Length > 0)
        {
            int delLength = 1;
            if (Progress[Progress.Length - 1] == '>')
            {
                // get the length of a character plus a tag pair (use of c here is arbitrary)
                delLength = BadLetterTag.Wrap("c").Length;
            }
            Progress = Progress.Substring(0, Progress.Length - delLength);
            return;
        }

        if (System.Char.IsLetter(e.character))
        {
            bool letterIsGood = CurrentCard.Word.StartsWith(Progress + e.character);
            string estr = e.character.ToString();
            string toAdd = letterIsGood ? estr : BadLetterTag.Wrap(estr);
            Progress += toAdd;

            if (letterIsGood) TypingSounds.Instance.PlayLetter();
            else TypingSounds.Instance.PlayDud();
        }
    }

    public void StartTypingPhase (List<Card> cards)
    {
        EventBox.Log("\n\nThe typing phase has started.");
        CardBehavior.StartTypeStep();
        StartCoroutine(lineScaler());
        StartCoroutine(countDownRoutine(cards));
    }

    IEnumerator countDownRoutine (List<Card> cards)
    {
        for (int i = CountdownTime; i > 0; i--)
        {
            TimerText.text = i.ToString();
            CountdownSource.PlayOneShot(CountdownClip);
            yield return new WaitForSeconds(1);
        }

        TypingSounds.Instance.PlayWord();

        initializePhase(cards);
    }

    void initializePhase (List<Card> cards)
    {
        Cards = cards;
        Progress = "";
        Timer = TypingTime;

        CurrentCardText.text = CurrentCard.Word;
        for (int i = 1; i < cards.Count; i++)
        {
            UpcomingWords[i - 1].text = cards[i].Word;
        }

        inTypingPhase = true;
    }

    void endTypingPhase ()
    {
        EventBox.Log("\n\n");
        foreach (var card in Cards)
        {
            Player.IncrementHealth(-card.Burn, "The word " + card.Word, "burnt");
        }
        Player.IncrementHealth(-Enemy.GetDamagePlan(), "The enemy", "hurt");

        Player.EndTypeStep();
        Enemy.EndTypeStep();
        CardBehavior.EndTypeStep();

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
        TypingSounds.Instance.PlayWord();

        CurrentCard.DoBehavior(Player, Enemy);

        CardBehavior.OnCast(CurrentCard, Player);
        
        Progress = "";

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
                Debug.Log("Time bonus: " + (Timer / TypingTime) * 100);
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
            TyperBar.transform.localPosition = lineStartPos + Random.insideUnitCircle * LineShakeMagnitude * (timer / LineShakeDuration);
            timer -= Time.deltaTime;
            yield return null;
        }

        TyperBar.transform.localPosition = lineStartPos;
    }
}
