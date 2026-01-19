using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Notes : Only the Server must spawn and despawn Netcode objects. in this case bullets. Only must be handled by the server on the server authoritative networking model  .
/// ClientRpc are used to update visual effects on clients, UI Stuff, when a bullet is fired or stuff that no have NetworkObject.
/// 
/// Spawn Networkobject only sould be used if all the player must see fisically the same object in the same position example object that usea physcs o collisions.
/// Instanciate local object only when the object is visual only and dont need to be synchronized with the server example, dummy bullet to show shooting direction.
/// </summary>


public class BulletNetcodeHandler : NetworkBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Transform _bulletspawn;

    //TODO Change the write permission to Server
    public NetworkVariable<ProjectileContext> BulletInfo = new NetworkVariable<ProjectileContext>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    public NetworkVariable<int> Ammo = new NetworkVariable<int>(
       5,
       NetworkVariableReadPermission.Owner,
       NetworkVariableWritePermission.Server
   );

    private NetworkServerBulletPool _serverBulletPoolHandler;
    private EventBusService _eventBus;

    private void Start()
    {
        _serverBulletPoolHandler = ServiceLocator.Instance.Get<NetworkServerBulletPool>();
    }

    public override void OnNetworkSpawn()
    {
        _eventBus = ServiceLocator.Instance.Get<EventBusService>();

        if (!IsOwner)
            return;

        if (_eventBus != null)
        {
            _eventBus.Subscribe<IProjectileInfo>(nameof(GameplayEvents.ON_PERFORM_SHOOT), HandleShoot);
        }
    }


    public void UpdateAmmo(int ammo = 0)
    {
        Ammo.Value = ammo;
    }

    private void HandleShoot(IProjectileInfo projectileInfo)
    {
        UpdateProjectileContext(projectileInfo);
        PerformLocalShoot(_bulletspawn.position);
        SpawnBulletServerRpc();

    }

    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        ServerBulletSpawn();
        SendEventsToClientRpc(_bulletspawn.position);
    }

    [ClientRpc]
    private void SendEventsToClientRpc(Vector2 projectilepos)
    {
        if (IsOwner)
            return;

        _eventBus?.Broadcast(nameof(NetworkEvents.ON_SERVER_BULLET_FIRED), BulletInfo.Value, projectilepos);
    }

    private void UpdateProjectileContext(IProjectileInfo projectileInfo)
    {
        BulletInfo.Value = new ProjectileContext
        {
            ProjectileOwner = _player.PlayerName.Value.ToString(),
            Damage = projectileInfo.Damage,
            Lifetime = projectileInfo.LifeTime,
            BulletSpeed = projectileInfo.BulletSpeed
        };
    }

    private void ServerBulletSpawn()
    {

        NetworkObject networkObj = _serverBulletPoolHandler.GetServerBullet(_bulletspawn.position, _bulletspawn.rotation);
        ServerBullet serverBullet = networkObj.GetComponent<ServerBullet>();
        if (Ammo.Value > 0)
            UpdateAmmo(Ammo.Value - 1);

        serverBullet?.SetBulletOwnerInfo(BulletInfo.Value);
        networkObj?.Spawn(true);
    }



    private void PerformLocalShoot(Vector2 projectilepos)
    {
        _eventBus?.Broadcast(nameof(NetworkEvents.ON_LOCAL_BULLET_FIRED), BulletInfo.Value, projectilepos);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;
        if (_eventBus != null)
        {
            _eventBus.UnSusbcribe<IProjectileInfo>(nameof(NetworkEvents.ON_SERVER_BULLET_FIRED), HandleShoot);
        }
    }
}



