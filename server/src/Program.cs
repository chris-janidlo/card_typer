﻿using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using CTShared;
using CTShared.Networking;

public class Program
{
    NetManager server;

    NetPeer player1Peer, player2Peer;
    string player1DeckStaging, player2DeckStaging;

    MatchManager manager;

    static void Main (string[] args)
    {
        Program main = new Program();
        main.initializeServer();
        main.serverLoop();
    }

    void initializeServer ()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        server = new NetManager(listener);

        listener.ConnectionRequestEvent += handleConnectionRequest;
        listener.PeerConnectedEvent += handlePeerConnected;
        listener.PeerDisconnectedEvent += handlePeerDisconnected;

        server.Start(NetworkConstants.ServerPort);
        Console.WriteLine($"Server started on port {NetworkConstants.ServerPort}. Press ctrl-c to stop it.");
    }

    void serverLoop ()
    {
        Console.CancelKeyPress += closeServer;

        while (true)
        {
            server.PollEvents();
            Thread.Sleep(15);
        }
    }

    void closeServer (object sender, ConsoleCancelEventArgs args)
    {
        Console.WriteLine("Cleaning up server resources...");

        server.Stop();

        Console.WriteLine("Server stopped.");
    }

    void handleConnectionRequest (ConnectionRequest request)
    {
        Console.Write($"attempted connection from {request.RemoteEndPoint.Address.ToString()} was ");

        if (player1Peer == null || player2Peer == null)
        {
            request.AcceptIfKey(NetworkConstants.VersionNumberConnectionKey);

            if (request.Result == ConnectionRequestResult.Accept)
            {
                Console.WriteLine("accepted");
            }
            else
            {
                Console.WriteLine($"rejected because the key did not match \"{NetworkConstants.VersionNumberConnectionKey}\"");
            }
        }
        else
        {
            request.Reject();
            Console.WriteLine("rejected because the server was full");
        }
    }

    void handlePeerConnected (NetPeer peer)
    {
        if (player1Peer == null)
        {
            player1Peer = peer;
        }
        else if (player2Peer == null)
        {
            player2Peer = peer;
        }
        else
        {
            Console.Error.WriteLine($"unexpected third peer connected at endpoint {peer.EndPoint}");
            peer.Disconnect();
            return;
        }
        Console.WriteLine(nicePeerString(peer) + " has connected");

        NetDataWriter writer = new NetDataWriter();
        writer.Put((int) PacketType.ServerReadyToReceiveDeck);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);

        Console.WriteLine("now waiting for their deck...");
    }

    void handlePeerDisconnected (NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Console.WriteLine(nicePeerString(peer) + " has disconnected for reason " + disconnectInfo.Reason.ToString());

        // in the future, would probably prefer trying to reconnect here or do something with the disconnection. for prototyping's sake, we're just gonna clear out the peer so we can keep the server running while testing
        if (peer == player1Peer)
        {
            player1Peer = null;
        }
        else
        {
            player2Peer = null;
        }
    }

    string nicePeerString (NetPeer peer)
    {
        return $"player {(peer == player1Peer ? "1" : "2")}: ID {peer.Id}, endpoint: {peer.EndPoint}";
    }
}