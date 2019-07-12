using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using CTShared.Networking;

public class MatchConnector : MonoBehaviour
{
    public string IP;
    public TextAsset DeckAsset;

    NetManager client;
    NetPacketProcessor packetProcessor;
    NetPeer server;

    void Start ()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        client = new NetManager(listener);
        packetProcessor = PacketUtils.CreateNetPacketProcessor();

        listener.PeerConnectedEvent += peer => Debug.Log("connected to server");
        listener.PeerDisconnectedEvent += handleDisconnect;
        listener.NetworkReceiveEvent += handlePacket;

        packetProcessor.SubscribeReusable<ServerReadyToReceiveDeckPacket>(handleServerReadyToReceiveDeck);
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

    void handlePacket (NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        packetProcessor.ReadAllPackets(reader);
    }

    void handleServerReadyToReceiveDeck (ServerReadyToReceiveDeckPacket packet)
    {
        packetProcessor.Send(server, new ClientDeckRegistrationPacket(DeckAsset.text), DeliveryMethod.ReliableOrdered);
    }

    void handleDisconnect (NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"disconnected from server: {disconnectInfo.Reason.ToString()}. socket error: {disconnectInfo.SocketErrorCode.ToString()}");
    }
}
