using System.Collections;
using System.Collections.Generic;
using LiteNetLib.Utils;

namespace CTShared.Networking
{
// this file is for basic message packets; packets that don't represent code/gameplay objects, but instead represent client-server communication


public class ErrorMessagePacket : Packet
{
	public string Message { get; private set; }

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


public class ClientDeckRegistrationPacket : Packet
{
	public string DeckText { get; private set; }

	public ClientDeckRegistrationPacket (string text)
	{
		DeckText = text;
	}

	internal ClientDeckRegistrationPacket () {}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put(DeckText);
	}

	internal override void Deserialize (NetDataReader reader)
	{
		DeckText = reader.GetString();
	}
}


public class PlaySelectionPacket : Packet
{
	public List<int> SelectionIndices { get; private set; }

	public PlaySelectionPacket (List<int> indices)
	{
		SelectionIndices = indices;
	}

	internal PlaySelectionPacket () {}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put((byte) SelectionIndices.Count);

		foreach (var index in SelectionIndices)
		{
			writer.Put((byte) index);
		}
	}

	internal override void Deserialize (NetDataReader reader)
	{
		SelectionIndices = new List<int>(reader.GetByte());

		for (int i = 0; i < SelectionIndices.Count; i++)
		{
			SelectionIndices.Add(reader.GetByte());
		}
	}
}

public class ServerReadyToReceiveDeckSignalPacket : SignalPacket {}
public class Player1SignalPacket : SignalPacket {}
public class Player2SignalPacket : SignalPacket {}
}
