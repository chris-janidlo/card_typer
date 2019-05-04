using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using crass;

public class MatchManager : Singleton<MatchManager>
{
    public event Action OnStartTypePhase, OnEndTypePhase, OnStartDrawPhase, OnEndDrawPhase;

    public float TypingTime;
    public int TypingCountdownTime;

    [Header("UI References")]
    public Agent Player;
    public Agent Enemy;
    public TextMeshProUGUI TimerText;
    public AudioSource CountdownSource;

    public float TypingTimer { get; private set; }
    public float TypingTimeLeftPercent => TypingTimer / TypingTime;

    bool inTypingPhase = false;

    void Awake ()
    {
        var inst = SingletonGetInstance();
        if (inst != null)
        {
            inst.OnStartTypePhase = null;
            inst.OnEndTypePhase = null;
            inst.OnStartDrawPhase = null;
            inst.OnEndDrawPhase = null;
            Destroy(inst.gameObject);
        }

        Player.Death += a => SceneManager.LoadScene("Loss");
        Enemy.Death += a => SceneManager.LoadScene("Victory");
    }

    void Start ()
    {
        startDrawPhase();
    }

    void Update ()
    {
        if (!inTypingPhase) return;

        TypingTimer -= Time.deltaTime;
        TimerText.text = Mathf.Ceil(TypingTimer).ToString();
        if (TypingTimer <= 0)
        {
            endTypePhase();
        }
    }

    void startDrawPhase ()
    {
        if (OnStartDrawPhase != null) OnStartDrawPhase();

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
        if (OnEndDrawPhase != null) OnEndDrawPhase();

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

        if (OnStartTypePhase != null) OnStartTypePhase();
        
        ITyper.CardsCastedSinceTurnStart = 0;
        
        Player.Typer.StartPhase();
        Enemy.Typer.StartPhase();
    }

    void endTypePhase ()
    {
        if (OnEndTypePhase != null) OnEndTypePhase();

        TimerText.text = "";

        Player.Typer.EndPhase();
        Enemy.Typer.EndPhase();

        Player.Typer.SetPlay(null);
        Enemy.Typer.SetPlay(null);

        inTypingPhase = false;
        startDrawPhase();
    }
}
