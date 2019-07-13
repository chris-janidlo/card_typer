using LiteNetLib.Utils;

namespace CTShared.Networking
{
public abstract class IPacket
{
	internal abstract void Deserialize (NetDataReader reader);
	internal abstract void Serialize (NetDataWriter writer);
}
}
