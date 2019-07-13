using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
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
        // TODO: handle bad deck disconnection

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

    void handleDisconnect (NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"disconnected from server: {disconnectInfo.Reason.ToString()}. socket error: {disconnectInfo.SocketErrorCode.ToString()}");
    }
}
