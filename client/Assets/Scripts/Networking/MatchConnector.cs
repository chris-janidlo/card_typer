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
    NetPeer server;

    void Start ()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        client = new NetManager(listener);

        listener.PeerConnectedEvent += peer => Debug.Log("connected to server");
        listener.PeerDisconnectedEvent += handleDisconnect;
        listener.NetworkReceiveEvent += handlePacket;

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
        PacketType type;

        try
        {
            type = PacketUtils.GetType(reader);
        }
        catch (ArgumentException e)
        {
            Debug.LogError($"received malformed packet; error: {e.Message}");
            throw;
        }

        switch (type)
        {
            case PacketType.ServerReadyToReceiveDeck:
                Debug.Log("sending server our deck");

                ClientDeckRegistrationPacket pkt =
                    new ClientDeckRegistrationPacket().WithDeck(DeckAsset.text);
                server.Send(pkt.ToWriter(), DeliveryMethod.ReliableOrdered);
                break;
            
            default:
                Debug.LogError($"unexpected packet type {type}");
                break;
        }

        reader.Recycle();
    }

    void handleDisconnect (NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"disconnected from server: {disconnectInfo.Reason.ToString()}. socket error: {disconnectInfo.SocketErrorCode.ToString()}");
    }
}
