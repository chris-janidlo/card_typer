using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using CTShared;
using crass;

public class LocalManagerContainer : ManagerContainer
{
    [SerializeField]
    private AgentHealth playerHealth, enemyHealth;

    [SerializeField]
    private TextAsset playerDeck, enemyDeck;

    [SerializeField]
    private DeckDisplay playerDrawer;

    [SerializeField]
    private EnemyDrawer enemyDrawer;

    [SerializeField]
    private PlayerTyper playerTyper;

    [SerializeField]
    private EnemyTyper enemyTyper;

    [SerializeField]
    private CardQueue playerCards, enemyCards;

    public Agent Player => manager.Player1;
    public Agent Enemy => manager.Player2;

    void Start ()
    {
        manager = new MatchManager(playerDeck.text, enemyDeck.text);

        playerHealth.Initialize(Player);
        enemyHealth.Initialize(Enemy);

        playerDrawer.Initialize(Player.Deck);
        enemyDrawer.Initialize(Enemy.Deck);

        playerTyper.Initialize(Player);
        enemyTyper.Initialize(Enemy);

        playerCards.Initialize(Player);
        enemyCards.Initialize(Enemy);
    }
}
