using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using CTShared;
using crass;

public class ManagerContainer : Singleton<ManagerContainer>
{
    [SerializeField]
    private TextAsset player1Deck, player2Deck;

    public static MatchManager Manager => Instance.manager;
    MatchManager manager;

    void Awake ()
    {
        if (SingletonGetInstance() != null)
        {
            Destroy(SingletonGetInstance().gameObject);
        }

        SingletonSetInstance(this, true);

        manager = new MatchManager(player1Deck.text, player2Deck.text);
        
        manager.Player1.OnDeath += () => SceneManager.LoadScene("Loss");
        manager.Player2.OnDeath += () => SceneManager.LoadScene("Victory");
    }

    void Update ()
    {
        manager.Tick(Time.deltaTime);
    }
}
