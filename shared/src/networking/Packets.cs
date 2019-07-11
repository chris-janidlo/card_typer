using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace CTShared.Networking
{
public enum PacketType
{
	ErrorMessage,
	ServerReadyToReceiveDeck,
	ClientDeckRegistration
}

public static class PacketUtils
{
	// returns a PacketType if the packet is of a known type; throws an ArgumentException otherwise
	public static PacketType GetType (NetPacketReader reader)
	{
		PacketType type;

		try
		{
			type = (PacketType) reader.GetInt();
		}
		catch (ArgumentException)
		{
			throw new ArgumentNullException("packet does not contain a type");
		}

		if (!Enum.IsDefined(typeof(PacketType), type))
		{
			throw new ArgumentOutOfRangeException($"packet contains unexpected type {type}");
		}

		return type;
	}
}

public abstract class IPacket<T> where T : IPacket<T>, new()
{
	public static T FromReader (NetPacketReader reader)
	{
		T t = new T();
		t.getPacketData(reader);
		return t;
	}

	public NetDataWriter ToWriter ()
	{
		NetDataWriter writer = new NetDataWriter();
		writer.Put((int) Type);
		writePacketData(writer);
		return writer;
	}

	public abstract PacketType Type { get; }

	protected abstract void getPacketData (NetPacketReader reader);
	protected abstract void writePacketData (NetDataWriter writer);
}



public class ErrorMessagePacket : IPacket<ErrorMessagePacket>
{
	public override PacketType Type => PacketType.ErrorMessage;

	public string Message { get; private set; }

	public ErrorMessagePacket () {}

	public ErrorMessagePacket (string message)
	{
		Message = message;
	}

	protected override void getPacketData (NetPacketReader reader)
	{
		Message = reader.GetString();
	}

	protected override void writePacketData (NetDataWriter writer)
	{
		writer.Put(Message);
	}
}



public class ServerReadyToReceiveDeckPacket : IPacket<ServerReadyToReceiveDeckPacket>
{
	public override PacketType Type => PacketType.ServerReadyToReceiveDeck;

	protected override void getPacketData (NetPacketReader reader)
	{
		// packet has no data
	}

	protected override void writePacketData (NetDataWriter writer)
	{
		// packet has no data
	}
}



public class ClientDeckRegistrationPacket : IPacket<ClientDeckRegistrationPacket>
{
	public override PacketType Type => PacketType.ClientDeckRegistration;

	public string DeckText { get; private set; }

	public ClientDeckRegistrationPacket () {}

	public ClientDeckRegistrationPacket (string text)
	{
		DeckText = text;
	}

	protected override void getPacketData (NetPacketReader reader)
	{
		DeckText = reader.GetString();
	}

	protected override void writePacketData (NetDataWriter writer)
	{
		writer.Put(DeckText);
	}
}
}
