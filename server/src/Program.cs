using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using CTShared;
using CTShared.Networking;

public class Program
{
    public const int Port = 9050;

    NetManager server;

    NetPeer player1Peer, player2Peer;
    string player1DeckStaging, player2DeckStaging;

    MatchManager manager;

    static void Main (string[] args)
    {
        Program main = new Program();
        main.initializeServer();
        main.serverLoop();
        main.closeServer();
    }

    void initializeServer ()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        server = new NetManager(listener);

        listener.ConnectionRequestEvent += handleConnectionRequest;
        listener.PeerConnectedEvent += handlePeerConnected;

        server.Start(Port);
        Console.WriteLine($"Server started on port {Port}. Press any key to stop it.");
    }

    void serverLoop ()
    {
        while (!Console.KeyAvailable)
        {
            server.PollEvents();
            Thread.Sleep(15);
        }
    }

    void closeServer ()
    {
        server.Stop();

        Console.ReadKey(true);
        Console.WriteLine("Server stopped.");
    }

    void handleConnectionRequest (ConnectionRequest request)
    {
        Console.Write($"attempted connection from {request.RemoteEndPoint.Address.ToString()} was ");

        if (player1Peer == null || player2Peer == null)
        {
            request.Accept();
            Console.WriteLine("accepted");
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
            Console.WriteLine($"player 1 (ID: {peer.Id}) has connected");
        }
        else if (player2Peer == null)
        {
            player2Peer = peer;
            Console.WriteLine($"player 2 (ID: {peer.Id}) has connected");
        }
        else
        {
            Console.Error.WriteLine($"unexpected third peer connected at endpoint {peer.EndPoint}");
            peer.Disconnect();
            return;
        }

        NetDataWriter writer = new NetDataWriter();
        writer.Put((int) PacketType.ServerReadyToReceiveDeck);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);

        Console.WriteLine("now waiting for their deck...");
    }
}
