using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace CTShared.Networking
{
public static class PacketUtils
{
	// creates processor and registers anything that needs to be registered
	public static NetPacketProcessor CreateNetPacketProcessor ()
	{
		var proc = new NetPacketProcessor();

		return proc;
	}
}



public class ErrorMessagePacket : INetSerializable
{
	public string Message { get; set; }

	public ErrorMessagePacket () {}

	public ErrorMessagePacket (string message)
	{
		Message = message;
	}

	public void Serialize (NetDataWriter writer)
	{
		writer.Put(Message);
	}

	public void Deserialize (NetDataReader reader)
	{
		Message = reader.GetString();
	}
}



public class ServerReadyToReceiveDeckPacket : INetSerializable
{
	public void Serialize (NetDataWriter writer)
	{
		// packet has no data
	}

	public void Deserialize (NetDataReader reader)
	{
		// packet has no data
	}
}



public class ClientDeckRegistrationPacket : INetSerializable
{
	public string DeckText { get; set; }

	public ClientDeckRegistrationPacket() {}

	public ClientDeckRegistrationPacket (string text)
	{
		DeckText = text;
	}

	public void Serialize (NetDataWriter writer)
	{
		writer.Put(DeckText);
	}

	public void Deserialize (NetDataReader reader)
	{
		DeckText = reader.GetString();
	}
}
}
