using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField]
    private InputReader _inputReader;
    [SerializeField]
    private BulletBase _clientBullet;
    [SerializeField]
    private ProjectileData _projectileData;
    [SerializeField]
    private Transform _bulletSpawn;
    [SerializeField]
    private GameObject _muzzleObject;
    [SerializeField]
    private int _initBulletPoolCapacity;
    private ObjectPool<BulletBase> _bulletPool;
    private ServerNetworkSpawnerBulletService _serverBulletService;
    private Player _player;
    private NetworkObject _serverBulletNetworkObject;

    public NetworkVariable<BulletInfo> BulletInfo = new NetworkVariable<BulletInfo>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    /// <summary>
    /// hay que instanciar una pool local la cual mostrara solo efectos visuales y otra en el servidor que manejara la logica de las balas y colisiones.
    /// </summary>

    /// <summary>
    /// Notes : Only the Server must spawn and despawn Netcode objects. in this case bullets. Only must be handled by the server on the server authoritative networking model  .
    /// ClientRpc are used to update visual effects on clients, UI Stuff, when a bullet is fired or stuff that no have NetworkObject.
    /// 
    /// Spawn Networkobject only sould be used if all the player must see fisically the same object in the same position example object that usea physcs o collisions.
    /// Instanciate local object only when the object is visual only and dont need to be synchronized with the server example, dummy bullet to show shooting direction.
    /// </summary>

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _inputReader.OnFireEvent += OnSpawnBullet;
            _player = ServiceLocator.Instance.Get<Player>();
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            _inputReader.OnFireEvent -= OnSpawnBullet;
        }
    }

    private void Start()
    {
        _bulletPool = new ObjectPool<BulletBase>(_clientBullet.gameObject, _bulletSpawn);
        _bulletPool.InitPool(_initBulletPoolCapacity, "ClientBullet");
        _serverBulletService = ServiceLocator.Instance.Get<ServerNetworkSpawnerBulletService>();
    }

    private void OnSpawnBullet(bool ispressed)
    {
        BulletInfo.Value = new BulletInfo
        {
            PlayerOwner = _player.PlayerName.Value.ToString(),
            Damage = _projectileData.DamageAmount
        };
        Debug.Log($"Owner2: {BulletInfo.Value.PlayerOwner} ");
        ActionFire(ispressed);
        SpawnBulletServerRpc(ispressed);
    }
    private void ActionFire(bool ispressed)
    {
        if (ispressed)
            _bulletPool.GetObject();
    }

    [ClientRpc]
    private void MuzzleUpdateViewClientRpc(bool isVisible)
    {
        _muzzleObject.SetActive(isVisible);
    }

    [ClientRpc]
    private void SpawnDummyBulletClientRpc(bool isPressed)
    {
        if (IsOwner)
            return;
        ActionFire(isPressed);
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(bool isPressed)
    {
        SpawnDummyBulletClientRpc(isPressed);
        MuzzleUpdateViewClientRpc(isPressed);
        SpawnServerBullet(BulletInfo.Value, isPressed);

    }



    private void SpawnServerBullet(BulletInfo bulletInfo, bool isPressed)
    {
        if (!isPressed)
            return;

        _serverBulletNetworkObject = _serverBulletService.GetServerBullet(_bulletSpawn.position, _bulletSpawn.rotation);
        ServerBullet serverBullet = _serverBulletNetworkObject.GetComponent<ServerBullet>();
        serverBullet?.SetPlayerOwner(bulletInfo);
        _serverBulletNetworkObject?.Spawn(true);

    }
}
