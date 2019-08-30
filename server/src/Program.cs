using System;
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
        listener.NetworkReceiveEvent += PacketProcessor.ReadAllPackets;

        PacketProcessor.Subscribe<ClientDeckRegistrationPacket>(handleClientDeck);

        Console.CancelKeyPress += closeServer;

        server.Start(NetworkConstants.ServerPort);
        Console.WriteLine($"Server started on port {NetworkConstants.ServerPort}. Press ctrl-c to stop it.");
    }

    void serverLoop ()
    {
        while (true)
        {
            server.PollEvents();

            // TODO: give this an actual delta time
            if (manager != null)
            {
                manager.Tick(15);

                if (manager.InTypingPhase) sendToBoth(manager, DeliveryMethod.Sequenced);
            }

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

        PacketProcessor.Send<ServerReadyToReceiveDeckSignalPacket>(peer, DeliveryMethod.ReliableOrdered);

        Console.WriteLine("now waiting for their deck...");
    }

    void handlePeerDisconnected (NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Console.WriteLine(nicePeerString(peer) + " has disconnected for reason " + disconnectInfo.Reason.ToString());

        // in the future, would probably prefer trying to reconnect here or do something with the disconnection. for prototyping's sake, we're just gonna clear out the peer so we can keep the server running while testing
        if (peer == player1Peer)
        {
            player1Peer = null;
            player1DeckStaging = null;
        }
        else
        {
            player2Peer = null;
            player2DeckStaging = null;
        }
    }

    void handleClientDeck (ClientDeckRegistrationPacket packet, NetPeer peer)
    {
        if (manager != null)
        {
            Console.WriteLine("unexpected deck registration from " + nicePeerString(peer));
            return;
        }

        Console.Write("got deck from ");

        if (peer == player1Peer)
        {
            player1DeckStaging = packet.DeckText;
            Console.WriteLine("player 1");
        }
        else
        {
            player2DeckStaging = packet.DeckText;
            Console.WriteLine("player 2");
        }

        if (player1DeckStaging != null && player2DeckStaging != null)
        {
            startMatch();
        }
    }

    string nicePeerString (NetPeer peer)
    {
        return $"player {(peer == player1Peer ? "1" : "2")}: ID {peer.Id}, endpoint: {peer.EndPoint}";
    }

    void startMatch ()
    {
        try
        {
            manager = new MatchManager(player1DeckStaging, player2DeckStaging);
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine(e.Message);

            var message = new ErrorMessagePacket("server received invalid deck");

            sendToBoth(message, DeliveryMethod.ReliableOrdered);

            player1Peer.Disconnect();
            player2Peer.Disconnect();
        }

        manager.Start();

        sendToBoth(manager, DeliveryMethod.ReliableOrdered);

        PacketProcessor.Send<Player1SignalPacket>(player1Peer, DeliveryMethod.ReliableOrdered);
        PacketProcessor.Send<Player2SignalPacket>(player2Peer, DeliveryMethod.ReliableOrdered);
    }

    void sendToBoth<T> (T packet, DeliveryMethod deliveryMethod) where T : Packet
    {
        PacketProcessor.Send(player1Peer, packet, deliveryMethod);
        PacketProcessor.Send(player2Peer, packet, deliveryMethod);
    }

    void sendToBoth<T> (DeliveryMethod deliveryMethod) where T : SignalPacket
    {
        PacketProcessor.Send<T>(player1Peer, deliveryMethod);
        PacketProcessor.Send<T>(player2Peer, deliveryMethod);
    }
}
