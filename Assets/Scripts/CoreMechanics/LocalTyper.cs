using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using crass;

public abstract class LocalTyper : ITyper
{
    public Agent GoodGuy, BadGuy;
    public Image TyperBar;
    public TextMeshProUGUI ProgressIndicator;
    public TagPair BadLetterTag;
    public float LineShakeMagnitude, LineShakeDuration;

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

    public bool AcceptingInput { get; protected set; } = false;

    List<Card> cards;
    Vector2 lineStartPos;

    void Awake ()
    {
        lineStartPos = TyperBar.transform.localPosition;
    }

    public override void SetPlay (List<Card> cards)
    {
        if (cards == null)
        {
            this.cards = null;
        }
        else
        {
            this.cards = new List<Card>(cards);
        }
    }

    public override List<Card> GetPlay ()
    {
        return cards;
    }

    public override void StartPhase ()
    {
        Progress = "";
        AcceptingInput = true;
        TyperBar.enabled = true;
    }

    public override void EndPhase ()
    {
        Progress = "";
        AcceptingInput = false;
        TyperBar.enabled = false;

        foreach (var card in cards)
        {
            GoodGuy.IncrementHealth(-card.Burn);
        }
    }

    protected void typeKey (KeyCode key, bool shiftIsPressed = false)
    {
        Assert.IsTrue(AcceptingInput, "typing input received when it shouldn't have been");

        KeyState state = Keyboard.GetState(key);

        switch (state.Type)
        {
            case KeyStateType.Active:
                processKey(key, shiftIsPressed);
                break;

            case KeyStateType.Deactivated:
                badInputFeedback();
                break;

            case KeyStateType.Delayed:
                // TODO: feedback
                StartCoroutine(delayedKeyPress(state.DelaySeconds, key, shiftIsPressed));
                break;

            case KeyStateType.Sticky:
                // TODO: feedback
                state.StickyPressesRemaining--;
                if (state.StickyPressesRemaining <= 0)
                {
                    state.Type = KeyStateType.Active;
                }
                break;
        }
    }

    protected void typeKey (char key)
    {
        if (key == ' ')
        {
            typeKey(KeyCode.Space);
        }
        else
        {
            KeyCode code = (KeyCode) Enum.Parse(typeof(KeyCode), key.ToString(), true);
            typeKey(code, Char.IsUpper(key));
        }
    }

    void processKey (KeyCode key, bool shiftIsPressed)
    {
        switch (key)
        {
            case KeyCode.Space:
            case KeyCode.Return:
                confirmInput();
                break;
            
            case KeyCode.Backspace:
                deleteLetter();
                break;

            default:
                addLetter(key.ToChar(shiftIsPressed));
                break;
        }
    }

    IEnumerator delayedKeyPress (float delay, KeyCode key, bool shiftIsPressed)
    {
        yield return new WaitForSeconds(delay);
        processKey(key, shiftIsPressed);
    }

    void addLetter (char typed)
    {
        if (cards.Any(c => c.Word.StartsWith(Progress + typed)))
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

    void deleteLetter ()
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
    }

    void confirmInput ()
    {
        if (!tryPopCard())
        {
            badInputFeedback();
        }
    }

    void badInputFeedback ()
    {
        StartCoroutine(lineShaker());
        TypingSounds.Instance.PlayDud();
    }

    bool tryPopCard ()
    {
        Card toCast = cards.FirstOrDefault(c => c.Word.Equals(Progress));
        if (toCast == null) return false;

        TypingSounds.Instance.PlayWord();
        toCast.DoBehavior(GoodGuy, BadGuy);

        CardsCastedSinceTurnStart++;

        Progress = "";
        cards.Remove(toCast);

        if (cards.Count == 0)
        {
            AcceptingInput = false;
        }

        return true;
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
