using System;
using System.Reflection;
using CTShared.Networking;
using LiteNetLib.Utils;

namespace CTShared
{
public abstract partial class Card : Packet
{
    public delegate void CastEvent (Card card, Agent caster);
    public static event CastEvent BeforeCast, AfterCast;
    public static bool CastLock;

    // the actual word in the poem that this is shadowing
    public string Word;

    // the name of the class
    public string Name { get; private set; }

    public Agent Owner { get; private set; }

    public abstract string PartOfSpeech { get; }
    public abstract string Definition { get; }
    public abstract string EffectText { get; }

    public abstract int Burn { get; }

    protected MatchManager manager => Owner.Manager;

    protected Card () {}

    internal static Card FromName (string name, Agent owner)
    {
        Type t = namesToCards[name];
        Card card = (Card) t.GetConstructor(Type.EmptyTypes).Invoke(null);

        card.Owner = owner;
        card.Name = name;
        card.initialize();

        return card;
    }

    internal void DoBehavior (Agent caster)
    {
        if (BeforeCast != null) BeforeCast(this, caster);

        if (!CastLock) behaviorImplementation();

        if (AfterCast != null) AfterCast(this, caster);
    }

    internal override void Deserialize (NetDataReader reader)
    {
        // assume most cards have no internal state to serialize
    }

    internal override void Serialize (NetDataWriter writer)
    {
        // assume most cards have no internal state to serialize
    }

    protected abstract void behaviorImplementation ();

    protected virtual void initialize () {}

    internal virtual void OnTypePhaseStart () {}
    internal virtual void OnTypePhaseEnd () {}
    internal virtual void OnDrawPhaseEnd () {}
    internal virtual void OnTypePhaseTick (float dt) {}
    internal virtual void OnAgentHealthChanged (Agent agent, int delta) {}
    internal virtual void BeforeCardCast (Card card, Agent caster) {}
    internal virtual void AfterCardCast (Card card, Agent caster) {}
}
}
