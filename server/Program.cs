using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

class Program
{
    public static int Port = 9050;

    static void Main (string[] args)
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        NetManager server = new NetManager(listener);
        server.Start(Port);

        listener.ConnectionRequestEvent += request => request.Accept();

        listener.PeerConnectedEvent += peer =>
        {
            Console.WriteLine("We got connection: {0}", peer.EndPoint);
            NetDataWriter writer = new NetDataWriter();
            writer.Put("Hello client!");
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        };

        Console.WriteLine($"Server started on port {Port}. Press any key to stop it.");

        while (!Console.KeyAvailable)
        {
            server.PollEvents();
            Thread.Sleep(15);
        }
        Console.ReadKey(true);

        server.Stop();

        Console.WriteLine("Server stopped.");
    }
}
