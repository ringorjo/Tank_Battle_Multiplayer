using Unity.Collections;
using Unity.Netcode;

public struct ProjectileContext : INetworkSerializable
{
    public FixedString32Bytes PlayerOwner;
    public int Damage;
    public int Lifetime;
    public int BulletSpeed;

    public void UpdateOwner(FixedString32Bytes playerOwner)
    {
        PlayerOwner = playerOwner;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerOwner);
        serializer.SerializeValue(ref Damage);
        serializer.SerializeValue(ref Lifetime);
        serializer.SerializeValue(ref BulletSpeed);
    }
}
