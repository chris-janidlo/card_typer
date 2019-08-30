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


// one tick's worth of typing
public class TypingFramePacket : Packet
{
	public List<KeyboardKey> KeysPressedThisFrame { get; private set; }
	public bool Shift { get; private set; }

	public TypingFramePacket (List<KeyboardKey> pressed, bool shift)
	{
		KeysPressedThisFrame = pressed;
		Shift = shift;
	}

	internal override void Serialize (NetDataWriter writer)
	{
		writer.Put((byte) KeysPressedThisFrame.Count);

		foreach (var press in KeysPressedThisFrame)
		{
			writer.Put((byte) press);
		}

		writer.Put(Shift);
	}

	internal override void Deserialize (NetDataReader reader)
	{
		KeysPressedThisFrame = new List<KeyboardKey>(reader.GetByte());

		for (int i = 0; i < KeysPressedThisFrame.Count; i++)
		{
			KeysPressedThisFrame.Add((KeyboardKey) reader.GetByte());
		}

		Shift = reader.GetBool();
	}
}

public class ServerReadyToReceiveDeckSignalPacket : SignalPacket {}
public class Player1SignalPacket : SignalPacket {}
public class Player2SignalPacket : SignalPacket {}
}
