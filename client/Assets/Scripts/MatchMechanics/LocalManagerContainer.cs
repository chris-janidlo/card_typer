using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using CTShared;
using crass;

public class LocalManagerContainer : ManagerContainer
{
    [SerializeField]
    private TextAsset player1Deck, player2Deck;

    MatchManager manager;

	public override MatchManager Manager => manager;

    void Start ()
    {
        manager = new MatchManager(player1Deck.text, player2Deck.text);
    }
}
