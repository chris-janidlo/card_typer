using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTShared;

public class EnemyTyper : MonoBehaviour
{
    public UIKeyboard UIKeyboard;
    public int WordsPerMinute;

    public float SecondsPerCharacter => 12f / WordsPerMinute;

    Agent agent;
    List<Card> play;

    IEnumerator typeEnum;

    void Start ()
    {
        ManagerContainer.Manager.OnTypePhaseStart += startPhase;
        ManagerContainer.Manager.OnTypePhaseEnd += endPhase;
    }

    public void Initialize (Agent agent)
    {
        this.agent = agent;

        agent.OnPlaySet += p => play = p;
    }

    void startPhase ()
    {
        typeEnum = typeRoutine();
        StartCoroutine(typeEnum);
    }

    void endPhase ()
    {
        StopCoroutine(typeEnum);
    }

    IEnumerator typeRoutine ()
    {
        string input = String.Join(" ", play.Select(c => c.Word)) + " ";

        foreach (char c in input)
        {
            var key = c.ToKeyboardKey();

            agent.PressKey(key, Char.IsUpper(c));
            UIKeyboard.Keys[key].SetActiveState(true);

            yield return new WaitForSeconds(SecondsPerCharacter);

            UIKeyboard.Keys[key].SetActiveState(false);
        }
    }
}
