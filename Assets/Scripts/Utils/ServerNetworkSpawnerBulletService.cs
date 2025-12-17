using Unity.Netcode;
using UnityEngine;

public class ServerNetworkSpawnerBulletService : MonoBehaviour, IService
{
    [SerializeField]
    private NetworkObject _serverBulletPrefab;
    [SerializeField]
    private int _initBulletPoolCapacity;
    private NetworkObjectPool<ServerBullet> _networkPool;
    private ObjectPool<ServerBullet> _bulletPool;

    private void Awake()
    {
        Register();
    }

    private void Start()
    {
        if (NetworkManager.Singleton)
            NetworkManager.Singleton.OnServerStarted += Init;
    }
    private void OnDestroy()
    {
        Unregister();
        if (NetworkManager.Singleton)
            NetworkManager.Singleton.OnServerStarted -= Init;
    }
    private void Init()
    {

        _bulletPool = new ObjectPool<ServerBullet>(_serverBulletPrefab.gameObject, transform);
        _networkPool = new NetworkObjectPool<ServerBullet>(_bulletPool, _initBulletPoolCapacity);
        NetworkManager.Singleton.PrefabHandler.AddHandler(_serverBulletPrefab, _networkPool);
    }

    public NetworkObject GetServerBullet(Vector3 pos, Quaternion rot)
    {
        return _networkPool.GetNetworkObject(pos, rot);

    }

    public void Register()
    {
        ServiceLocator.Instance.Register(this);
    }

    public void Unregister()
    {
        ServiceLocator.Instance?.Unregister(this);
    }
}
