using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField]
    private InputReader _inputReader;
    [SerializeField]
    private BulletBase _clientBullet;
    [SerializeField]
    private BulletType _bulletType;
    [SerializeField]
    private Transform _bulletSpawn;
    [SerializeField]
    private GameObject _muzzleObject;
    [SerializeField]
    private int _initBulletPoolCapacity;
    [SerializeField]
    private int _maxBulletsCapacity;
    private ObjectPool<BulletBase> _bulletPool;
    private ServerNetworkSpawnerBulletService _serverBulletService;
    private Player _player;
    private NetworkObject _serverBulletNetworkObject;
    private BulletSelectorFactory _bulletSelectorFactory;
    private IProjectileInfo _currentProjectileInfoUsed;
    public NetworkVariable<ProjectileContext> BulletInfo = new NetworkVariable<ProjectileContext>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    public IProjectileInfo CurrentProjectileInfoUsed
    {
        get => _currentProjectileInfoUsed;
    }
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
            _bulletSelectorFactory = new BulletSelectorFactory(5);
            _currentProjectileInfoUsed = _bulletSelectorFactory.GetBulletByType(_bulletType);
            _inputReader.OnFireEvent += OnSpawnBullet;

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
        _player = GetComponent<Player>();
    }

    private void OnSpawnBullet(bool ispressed)
    {

        BulletInfo.Value = new ProjectileContext
        {
            PlayerOwner = _player.PlayerName.Value.ToString(),
            Damage = _currentProjectileInfoUsed.Damage,
            Lifetime = _currentProjectileInfoUsed.LifeTime,
            BulletSpeed = _currentProjectileInfoUsed.BulletSpeed
        };
        ActionFire(ispressed);
        SpawnBulletServerRpc(ispressed);
           
    }
    private void ActionFire(bool ispressed)
    {
        if (ispressed)
        {
            _bulletPool.GetObject();
            UpdateRainingBullet();
        }
            
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
        MuzzleUpdateViewClientRpc(isPressed);
        SpawnDummyBulletClientRpc(isPressed);
        SpawnServerBullet(BulletInfo.Value, isPressed);
    }

    private void UpdateRainingBullet()
    {
        if (!IsOwner)
            return;
        _currentProjectileInfoUsed?.UpdateRemaingAmmount();
    }

    public void ReloadAmmo()
    {
        if(_currentProjectileInfoUsed==null)
        {
            Debug.LogError("CurrentProjectile Info is NUll");
            return;
        }
        _currentProjectileInfoUsed.ReloadAmmo();
    }



    private void SpawnServerBullet(ProjectileContext bulletInfo, bool isPressed)
    {
        if (!isPressed)
            return;

        _serverBulletNetworkObject = _serverBulletService.GetServerBullet(_bulletSpawn.position, _bulletSpawn.rotation);
        ServerBullet serverBullet = _serverBulletNetworkObject.GetComponent<ServerBullet>();
        serverBullet?.SetPlayerOwner(bulletInfo);
        _serverBulletNetworkObject?.Spawn(true);

    }
}
