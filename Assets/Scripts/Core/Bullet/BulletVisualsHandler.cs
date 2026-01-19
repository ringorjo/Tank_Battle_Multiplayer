using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletVisualsHandler : MonoBehaviour
{
    [SerializeField]
    private BulletBase _clientBullet;
    [SerializeField]
    private Transform _bulletSpawn;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private List<GameObject> _visualsComponents;
    private ObjectPool<BulletBase> _clientBulletPool;
    private EventBusService _eventBus;
    private List<IVisualable> _visualablesComponents = new List<IVisualable>();

    private void Awake()
    {
        _visualsComponents.ForEach(component =>
        {
            if (component.TryGetComponent(out IVisualable visualable))
            {
                _visualablesComponents.Add(visualable);
            }
        });
    }

    private async void Start()
    {
        await Task.Delay(500);
        _clientBulletPool = new ObjectPool<BulletBase>(_clientBullet.gameObject, _bulletSpawn);
        _clientBulletPool.InitPool(10, "Client_Bullet");
        _eventBus = ServiceLocator.Instance.Get<EventBusService>();
        if (_eventBus != null)
        {


            if (_player.IsOwner)
            {
                _eventBus.Subscribe<ProjectileContext, Vector2>(nameof(NetworkEvents.ON_LOCAL_BULLET_FIRED), OnShoot);
                Debug.Log("escucha1 Local: " + _player.PlayerName.Value);
            }
            else
            {
                _eventBus.Subscribe<ProjectileContext, Vector2>(nameof(NetworkEvents.ON_SERVER_BULLET_FIRED), OnShoot);
                Debug.Log("escucha2 Remote: " + _player.PlayerName.Value);


            }
        }
    }

    private void OnShoot(ProjectileContext projectile, Vector2 pos)
    {
        BulletBase bullet = _clientBulletPool?.GetObject(pos, _bulletSpawn.rotation);

        bullet?.InjectBulletInfo(projectile);
        bullet?.OnSpawn();
        _visualablesComponents.ForEach(visualable =>
        {
            visualable.PerformVisuals();
        });
    }
}
