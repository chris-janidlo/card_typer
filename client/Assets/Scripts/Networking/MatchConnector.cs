using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using CTShared;
using CTShared.Networking;

public class MatchConnector : MonoBehaviour
{
    public string IP;
    public TextAsset DeckAsset;

    NetManager client;
    NetPeer server;

    void Start ()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        client = new NetManager(listener);

        listener.PeerConnectedEvent += peer => Debug.Log("connected to server");
        listener.PeerDisconnectedEvent += handleDisconnect;
        listener.NetworkReceiveEvent += PacketProcessor.ReadAllPackets;

        PacketProcessor.Subscribe<ServerReadyToReceiveDeckPacket>(handleServerReadyToReceiveDeck);
        PacketProcessor.Subscribe<MatchManager>(handleManagerPacket);

        client.Start();
        server = client.Connect(IP, NetworkConstants.ServerPort, NetworkConstants.VersionNumberConnectionKey);
    }

    void Update ()
    {
        client.PollEvents();
    }

    void OnDestroy ()
    {
        client.Stop();
    }

    void handleServerReadyToReceiveDeck (ServerReadyToReceiveDeckPacket packet, NetPeer peer)
    {
        PacketProcessor.Send(server, new ClientDeckRegistrationPacket(DeckAsset.text), DeliveryMethod.ReliableOrdered);
    }

    void handleManagerPacket (MatchManager packet, NetPeer peer)
    {
        Debug.Log("got MatchManager");
        Debug.Log(packet.Player1.Deck.BracketedText);
        Debug.Log(packet.Player2.Deck.BracketedText);
    }

    void handleDisconnect (NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"disconnected from server: {disconnectInfo.Reason.ToString()}. socket error: {disconnectInfo.SocketErrorCode.ToString()}");
        // TODO: handle bad deck disconnection
    }
}
