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
            Destroy(gameObject);
        }
        else
        {
            SingletonSetInstance(this, false);
            DontDestroyOnLoad(gameObject);

            manager = new MatchManager(player1Deck.text, player2Deck.text);
            
            manager.Player1.OnDeath += () => SceneManager.LoadScene("Loss");
            manager.Player2.OnDeath += () => SceneManager.LoadScene("Victory");
        }
    }
}
