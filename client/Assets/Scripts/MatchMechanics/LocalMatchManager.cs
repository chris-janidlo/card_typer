using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CTShared;
using TMPro;
using crass;

public class LocalMatchManager : Singleton<LocalMatchManager>
{
    public TextAsset Player1Deck, Player2Deck;

    [Header("UI References")]
    public TextMeshProUGUI TimerText;
    public AudioSource CountdownSource;

    MatchManager manager;

    void Awake ()
    {
        if (SingletonGetInstance() != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SingletonSetInstance(this, false);
            DontDestroyOnLoad(gameObject);

            manager = new MatchManager(Player1Deck.text, Player2Deck.text);
            
            manager.Player1.OnDeath += () => SceneManager.LoadScene("Loss");
            manager.Player2.OnDeath += () => SceneManager.LoadScene("Victory");
        }
    }

    void Start ()
    {
        Player.Drawer.InitializeGame();
        Enemy.Drawer.InitializeGame();
        startDrawPhase();
    }

    void Update ()
    {
        manager.Tick();
    }

    void startDrawPhase ()
    {
        if (OnDrawPhaseStart != null) OnDrawPhaseStart();

        Action checker = () =>
        {
            if (Player.Typer.GetPlay() != null && Enemy.Typer.GetPlay() != null)
            {
                endDrawPhase();
            }
        };

        Player.Drawer.StartPhase(play => { Player.Typer.SetPlay(play); checker(); });
         Enemy.Drawer.StartPhase(play => {  Enemy.Typer.SetPlay(play); checker(); });
    }

    void endDrawPhase ()
    {
        if (OnDrawPhaseEnd != null) OnDrawPhaseEnd();

        StartCoroutine(startTypePhase());
    }

    IEnumerator startTypePhase ()
    {
        for (int i = TypingCountdownTime; i > 0; i--)
        {
            TimerText.text = i.ToString();
            CountdownSource.Play();
            yield return new WaitForSeconds(1);
        }

        CountdownSource.Play();

        inTypingPhase = true;
        TypingTimer = TypingTime;

        if (OnTypePhaseStart != null) OnTypePhaseStart();
        
        ITyper.CardsCastedSinceTurnStart = 0;
        
        Player.Typer.StartPhase();
        Enemy.Typer.StartPhase();
    }

    void endTypePhase ()
    {
        if (OnTypePhaseEnd != null) OnTypePhaseEnd();

        TimerText.text = "";

        Player.Typer.EndPhase();
        Enemy.Typer.EndPhase();

        Player.Typer.SetPlay(null);
        Enemy.Typer.SetPlay(null);

        inTypingPhase = false;
        startDrawPhase();
    }
}
