using Unity.Netcode;
using UnityEngine;

public class ServerBullet : NetworkBehaviour, IPoolObject
{
    [SerializeField]
    protected Rigidbody2D _rb;
    private ProjectileContext _bulletInfo;
    private IResettablePool _objectPool;
    public GameObject GameObject => gameObject;

    public void OnSpawn()
    {
        // transform.SetParent(null);
    }

    public void Initialize(IResettablePool pool) => _objectPool = pool;
    public void SetBulletOwnerInfo(ProjectileContext bulletInfo)
    {
        Debug.Log($"Owner: {bulletInfo.ProjectileOwner}");
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


        Invoke(nameof(OnDestroyBullet), _bulletInfo.Lifetime);
        _rb.linearVelocity = transform.up * _bulletInfo.BulletSpeed;
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
