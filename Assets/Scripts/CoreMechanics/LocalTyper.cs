using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public override void StartPhase () {
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

    protected void typeLetter (char typed)
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

    protected void deleteLetter ()
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

    protected void typeConfirmKey ()
    {
        if (!tryPopCard())
        {
            StartCoroutine(lineShaker());
            TypingSounds.Instance.PlayDud();
        }
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
