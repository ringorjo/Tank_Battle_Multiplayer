using Unity.Collections;
using Unity.Netcode;

[System.Serializable]
public struct ProjectileContext : INetworkSerializable
{
    public FixedString32Bytes ProjectileOwner;
    public int Damage;
    public int Lifetime;
    public int BulletSpeed;

   

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ProjectileOwner);
        serializer.SerializeValue(ref Damage);
        serializer.SerializeValue(ref Lifetime);
        serializer.SerializeValue(ref BulletSpeed);
    }
}
