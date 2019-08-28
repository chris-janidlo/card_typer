using LiteNetLib.Utils;

namespace CTShared.Networking
{
// packet that doesn't have data. useful for various communication messages, and comes with some garbage savings in PacketProcessor
public abstract class SignalPacket : Packet
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
}