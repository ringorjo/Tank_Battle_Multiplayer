using Unity.Netcode;
using UnityEngine;
public class NetworkObjectPool<T> : INetworkPrefabInstanceHandler
    where T : NetworkBehaviour, IPoolObject
{


    private ObjectPool<T> _objectPool;


    public NetworkObjectPool(ObjectPool<T> pool, int capacity)
    {
        _objectPool = pool;
        _objectPool.InitPool(capacity, "ServerBullet");
    }

    public NetworkObject GetNetworkObject( Vector3 position, Quaternion rotation)
    {
        T obj = _objectPool.GetObject(position, rotation);
        return obj.GetComponent<NetworkObject>();
    }

    public void Destroy(NetworkObject networkObject)
    {
        _objectPool.RecycleObject(networkObject.GetComponent<T>());
    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        T instance = _objectPool.GetObject(position, rotation);
        var no = instance.NetworkObject;
        return no;
    }
}
