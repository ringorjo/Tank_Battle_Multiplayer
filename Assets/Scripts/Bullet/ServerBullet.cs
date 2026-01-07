using Unity.Netcode;
using UnityEngine;

public class ServerBullet : NetworkBehaviour, IPoolObject
{
    [SerializeField]
    protected Rigidbody2D _rb;
    [SerializeField]
    protected ProjectileData _projectileData;
    private ProjectileContext _bulletInfo;
    private ObjectPool<ServerBullet> _objectPool;
    public GameObject GameObject => gameObject;

    public void Initialize<TPool>(ObjectPool<TPool> poolManager) where TPool : IPoolObject
    {
        _objectPool = poolManager as ObjectPool<ServerBullet>;
    }

    public void OnSpawn()
    {
        // transform.SetParent(null);
    }

    public void SetPlayerOwner(ProjectileContext bulletInfo)
    {
        Debug.Log($"Owner: {bulletInfo.PlayerOwner}");

        _bulletInfo = bulletInfo;
    }

    public void OnDespawn(Transform parent)
    {
        _bulletInfo = new ProjectileContext();
    }

    public override void OnNetworkDespawn()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;


        Invoke(nameof(OnDestroyBullet), _projectileData.LifeTime);
        _rb.linearVelocity = transform.up * _projectileData.Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<IDamage>(out var component))
        {
            component.TakeDamage(_bulletInfo);
            OnDestroyBullet();
            CancelInvoke(nameof(OnDestroyBullet));
        }
    }

    private void OnDestroyBullet()
    {
        if (!IsServer)
            return;
        NetworkObject.Despawn();
    }
}
