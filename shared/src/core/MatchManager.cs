using System;
using System.Collections;
using System.Collections.Generic;

namespace CTShared
{
public class MatchManager
{
    public event Action OnPreTypePhaseStart, OnTypePhaseStart, OnTypePhaseEnd, OnDrawPhaseStart, OnDrawPhaseEnd;
    public event Action<float> OnTypePhaseTick;

    public const float TypingTime = 10;
    public const float TypingCountdownTime = 3;

    public readonly Agent Player1, Player2;

    public float TypingTimer { get; private set; }
    public float CountdownTimer { get; private set; }
    public float TypingTimeLeftPercent => TypingTimer / TypingTime;

    public int CardsCastedThisTurn => Player1.CardsCastedThisTurn + Player2.CardsCastedThisTurn;

    bool started;
    bool inDrawPhase, inPreTypingPhase, inTypingPhase;
    bool player1Ready, player2Ready;

    public MatchManager (string player1DeckText, string player2DeckText)
    {
        try
        {
            Player1 = new Agent(this, player1DeckText);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException("error when parsing player 1's deck", e.InnerException);
        }

        try
        {
            Player2 = new Agent(this, player2DeckText);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException("error when parsing player 2's deck", e.InnerException);
        }

        Player1.OnPlaySet += p => agentReady(Player1);
        Player2.OnPlaySet += p => agentReady(Player2);
    }

    public void Start ()
    {
        if (started)
        {
            throw new InvalidOperationException("match is already started");
        }

        started = true;
        startDrawPhase();
    }

    public void Tick (float dt)
    {
        if (inPreTypingPhase)
        {
            CountdownTimer -= dt;
            if (CountdownTimer <= 0)
            {
                startTypePhase();
            }
        }

        if (inTypingPhase)
        {
            if (OnTypePhaseTick != null) OnTypePhaseTick(dt);

            TypingTimer -= dt;
            if (TypingTimer <= 0)
            {
                endTypePhase();
            }
        }
    }

    internal Agent GetEnemyOf (Agent agent)
    {
        if (agent == Player1)
        {
            return Player2;
        }
        else if (agent == Player2)
        {
            return Player1;
        }
        else
        {
            throw new Exception($"unexpected agent {agent}");            
        }
    }

    void agentReady (Agent agent)
    {
        if (!inDrawPhase) return;

        if (agent == Player1)
        {
            player1Ready = true;
        }
        else if (agent == Player2)
        {
            player2Ready = true;
        }
        else
        {
            throw new Exception($"unexpected agent {agent}");
        }

        if (player1Ready && player2Ready)
        {
            endDrawPhase();
        }
    }

    void startDrawPhase ()
    {
        inDrawPhase = true;

        Player1.DrawNewHand();
        Player2.DrawNewHand();

        player1Ready = false;
        player2Ready = false;

        if (OnDrawPhaseStart != null) OnDrawPhaseStart();
    }

    void endDrawPhase ()
    {
        inDrawPhase = false;

        if (OnDrawPhaseEnd != null) OnDrawPhaseEnd();

        startPreTypePhase();
    }

    void startPreTypePhase ()
    {
        inPreTypingPhase = true;
        CountdownTimer = TypingCountdownTime;

        if (OnPreTypePhaseStart != null) OnPreTypePhaseStart();
    }

    void startTypePhase ()
    {
        inPreTypingPhase = false;
        inTypingPhase = true;
        TypingTimer = TypingTime;

        if (OnTypePhaseStart != null) OnTypePhaseStart();
    }

    void endTypePhase ()
    {
        inTypingPhase = false;

        if (OnTypePhaseEnd != null) OnTypePhaseEnd();

        startDrawPhase();
    }
}
}
