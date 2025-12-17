using Unity.Collections;
using Unity.Netcode;

public struct BulletInfo : INetworkSerializable
{
    public FixedString32Bytes PlayerOwner;
    public int Damage;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        serializer.SerializeValue(ref PlayerOwner);
        serializer.SerializeValue(ref Damage);
    }
}
     
