using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CTShared;
using crass;

public class LocalManagerContainer : ManagerContainer
{
    [SerializeField]
    private AgentHealth playerHealth, enemyHealth;

    [SerializeField]
    private UIKeyboard playerKeyboard, enemyKeyboard;

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
    private TypingDisplay playerTypeBar, enemyTypeBar;

    [SerializeField]
    private CardQueue playerCards, enemyCards;

    public Agent Player => manager.Player1;
    public Agent Enemy => manager.Player2;

    IEnumerator Start ()
    {
        // wait one frame so that other MonoBehaviours can subscribe in their Start methods
        yield return null;
        manager.Start();
    }

    void Update ()
    {
        if (manager != null) manager.Tick(Time.deltaTime);
    }

    protected override void initialize ()
    {
        manager = new MatchManager(playerDeck.text, enemyDeck.text);

        playerHealth.Initialize(Player);
        enemyHealth.Initialize(Enemy);

        playerKeyboard.Initialize(Player);
        enemyKeyboard.Initialize(Enemy);

        playerDrawer.Initialize(Player.Deck);
        enemyDrawer.Initialize(Enemy.Deck);

        playerTyper.Initialize(Player);
        enemyTyper.Initialize(Enemy);

        playerTypeBar.Initialize(Player);
        enemyTypeBar.Initialize(Enemy);

        playerCards.Initialize(Player);
        enemyCards.Initialize(Enemy);
    }
}
