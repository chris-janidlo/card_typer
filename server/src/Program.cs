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
        listener.NetworkReceiveEvent += handlePacket;

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

        ServerReadyToReceiveDeckPacket pkt = new ServerReadyToReceiveDeckPacket();
        peer.Send(pkt.ToWriter(), DeliveryMethod.ReliableOrdered);

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

    void handlePacket (NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        PacketType type;

        try
        {
            type = PacketUtils.GetType(reader);
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine($"received malformed packet from {nicePeerString(peer)}; error: {e.Message}");
            return;
        }

        switch (type)
        {
            case PacketType.ClientDeckRegistration:
                Console.Write("got deck from ");

                ClientDeckRegistrationPacket pkt = ClientDeckRegistrationPacket.FromReader(reader);

                // TODO: check that deck is valid
                if (peer == player1Peer)
                {
                    player1DeckStaging = pkt.DeckText;
                    Console.WriteLine("player 1");
                }
                else
                {
                    player2DeckStaging = pkt.DeckText;
                    Console.WriteLine("player 2");
                }

                if (player1DeckStaging != null && player2DeckStaging != null)
                {
                    startMatch();
                }

                break;

            default:
                Console.Error.WriteLine($"unexpected packet type {type}");
                break;
        }

        reader.Recycle();
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

            player1Peer.Send(message.ToWriter(), DeliveryMethod.ReliableOrdered);
            player1Peer.Disconnect();

            player2Peer.Send(message.ToWriter(), DeliveryMethod.ReliableOrdered);
            player2Peer.Disconnect();
        }
    }
}
