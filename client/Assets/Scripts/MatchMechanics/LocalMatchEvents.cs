using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using CTShared;
using crass;

public class LocalMatchEvents : MatchEvents
{
    [SerializeField]
    private TextAsset player1Deck, player2Deck;

    MatchManager manager;

    void Start ()
    {
        manager = new MatchManager(player1Deck.text, player2Deck.text);
        setupEvents();
    }

    void Update ()
    {
        manager.Tick(Time.deltaTime);
    }

    void setupEvents ()
    {
        setupAgentEvents(manager.Player1, Player1);
        setupAgentEvents(manager.Player2, Player2);

        manager.OnPreTypePhaseStart += OnPreTypePhaseStart.Invoke;
        manager.OnTypePhaseStart += OnTypePhaseStart.Invoke;
        manager.OnTypePhaseEnd += OnTypePhaseEnd.Invoke;

        manager.OnDrawPhaseStart += OnDrawPhaseStart.Invoke;
        manager.OnDrawPhaseEnd += OnDrawPhaseEnd.Invoke;

        manager.OnTypePhaseTick += OnTypePhaseTick.Invoke;
    }

    void setupAgentEvents (Agent manPlay, AgentEvents thisPlay)
    {
        manPlay.OnDeath += thisPlay.OnDeath.Invoke;
        manPlay.OnHealthChanged += thisPlay.OnHealthChanged.Invoke;
        manPlay.OnAttemptedCast += thisPlay.OnAttemptedCast.Invoke;
        manPlay.OnKeyPressed += thisPlay.OnKeyPressed.Invoke;
    }
}
