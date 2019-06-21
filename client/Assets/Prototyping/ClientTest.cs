using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

public class ClientTest : MonoBehaviour
{
    public string TargetAddress;
    public int TargetPort = 9050;

    NetManager client;
    bool connected;

    void Update ()
    {
        if (!connected) return;

        client.PollEvents();
    }

    [ContextMenu("Connect")]
    public void ConnectToServer ()
    {
        if (connected) return;

        EventBasedNetListener listener = new EventBasedNetListener();
        client = new NetManager(listener);
        client.Start();
        client.Connect(TargetAddress, TargetPort, "");

        listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
        {
            Debug.Log("We got: " + dataReader.GetString(100));
            dataReader.Recycle();
        };

        connected = true;

        Debug.Log("Connected to server.");
    }

    [ContextMenu("Disconnect")]
    public void DisconnectFromServer ()
    {
        if (!connected) return;

        client.Stop();
        connected = false;

        Debug.Log("Disconnected from server.");
    }
}
