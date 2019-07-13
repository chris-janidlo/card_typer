using LiteNetLib.Utils;

namespace CTShared.Networking
{
// this file is for basic message packets; packets that don't represent code/gameplay objects, but instead represent client-server communication


public class ErrorMessagePacket : IPacket
{
	public string Message { get; set; }

	public ErrorMessagePacket (string message)
	{
		Message = message;
	}

	internal ErrorMessagePacket () {}

	internal override void Deserialize (NetDataReader reader)
	{
		Message = reader.GetString();
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(Message);
	}
}


public class ServerReadyToReceiveDeckPacket : IPacket
{
	internal override void Serialize (NetDataWriter writer)
	{
		// packet has no data
	}

	internal override void Deserialize (NetDataReader reader)
	{
		// packet has no data
	}
}


public class ClientDeckRegistrationPacket : IPacket
{
	public string DeckText { get; set; }

	public ClientDeckRegistrationPacket (string text)
	{
		DeckText = text;
	}

	internal ClientDeckRegistrationPacket() {}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(DeckText);
	}

	internal override void Deserialize (NetDataReader reader)
	{
		DeckText = reader.GetString();
	}
}
}
