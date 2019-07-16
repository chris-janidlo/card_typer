using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;


namespace CTShared.Networking
{
// generously lifted from LiteNetLib's NetPacketProcessor, but made to work better with my architecture
public static class PacketProcessor
{
	// from https://github.com/RevenantX/LiteNetLib/blob/df368c04d98960ab591acd7e505fc204ffba6311/LiteNetLib/Utils/NetPacketProcessor.cs#L8
	static class hashCache<T>
	{
		public static bool Initialized;
		public static ulong ID;
	}

	static class constructorCache<T> where T : Packet
	{
		public static bool Initialized;
		public static Func<T> Constructor;
	}

	public delegate void PacketReceivedEvent<T> (T packet, NetPeer sender) where T : Packet;

	delegate void callbackDelegate (NetPeer peer, NetDataReader reader);

	static readonly Dictionary<ulong, callbackDelegate> callbacks = new Dictionary<ulong, callbackDelegate>();
	static readonly NetDataWriter writer = new NetDataWriter();

	public static void SetConstructor<T> (Func<T> constructor) where T : Packet
	{
		constructorCache<T>.Constructor = constructor;
		constructorCache<T>.Initialized = true;
	}

	public static void Subscribe<T> (PacketReceivedEvent<T> onReceive) where T : Packet
	{
		Func<T> constructor;

		if (!constructorCache<T>.Initialized)
		{
			tryGetConstructor<T>();
		}

		constructor = constructorCache<T>.Constructor;

		callbacks[getHash<T>()] = (peer, reader) => {
			var reference = constructor();
			reference.Deserialize(reader);
			onReceive(reference, peer);
		};
	}

	public static void ReadAllPackets (NetPeer peer, NetPacketReader reader)
	{
		while (reader.AvailableBytes > 0)
		{
			getCallbackFromData(reader)(peer, reader);
		}		
		reader.Recycle();
	}

	// for easy subscription to INetEventListener.NetworkReceiveEvent
	public static void ReadAllPackets (NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
	{
		ReadAllPackets(peer, reader);
	}

	public static void Send<T> (NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : Packet
	{
		writer.Reset();
		writer.Put(getHash<T>());
		packet.Serialize(writer);
		peer.Send(writer, deliveryMethod);
	}

	// FNV-1 64 bit hash
	static ulong getHash<T> ()
	{
		if (hashCache<T>.Initialized)
		{
			return hashCache<T>.ID;
		}

		ulong hash = 14695981039346656037UL; // offset
		string typeName = typeof(T).FullName;

		for (var i = 0; i < typeName.Length; i++)
		{
			hash = hash ^ typeName[i];
			hash *= 1099511628211UL; // prime
		}

		hashCache<T>.ID = hash;
		hashCache<T>.Initialized = true;
		return hash;
	}

	static callbackDelegate getCallbackFromData (NetDataReader reader)
	{
		var hash = reader.GetULong();
		callbackDelegate action;

		if (!callbacks.TryGetValue(hash, out action))
		{
			throw new ParseException("undefined packet in NetDataReader");
		}

		return action;
	}

	static void tryGetConstructor<T> () where T : Packet
	{
		var type = (typeof (T));

		var constructor = type.GetConstructors
		(
			BindingFlags.Instance | // all constructors are instance
			BindingFlags.Public |   // public constructors
			BindingFlags.NonPublic  // internal constructors
		)
		.FirstOrDefault(c => c.GetParameters().Length == 0);

		if (constructor == null)
		{
			throw new ArgumentException($"given type {type} has no parameterless constructor; either create one, or give PacketProcessor a method to call using SetConstructor");
		}
		else
		{
			SetConstructor<T>(() => (T) constructor.Invoke(null));
		}
	}
}
}
