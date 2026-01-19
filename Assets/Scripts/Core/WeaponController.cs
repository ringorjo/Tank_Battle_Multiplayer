using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private InputReader _inputReader;
    [Header("Weapon Settings")]
    [SerializeField]
    private BulletType _bulletType;
    [SerializeField]
    private int _maxAmmo = 5;

    private float _fireRateCoolDown;
    private float _coolDown;
    private bool _isOnCoolDown = false;

    private BulletSelectorFactory _bulletSelectorFactory;
    private IProjectileInfo _currentProjectile;
    private IAmmoProvider _ammoProvider;
    private EventBusService _eventBus;
    private Player _player;

    public IProjectileInfo CurrentProjecticle => _currentProjectile;

    private void Awake()
    {
        if (!ServiceLocator.Instance.Exists<EventBusService>())
        {
            _eventBus = new EventBusService();
        }
        else
        {
            _eventBus = ServiceLocator.Instance.Get<EventBusService>();
        }
    }

    private void Start()
    {
        _player = GetComponent<Player>();
        _ammoProvider = GetComponent<IAmmoProvider>();
        _ammoProvider.SetMaxAmmo(_maxAmmo);
        _bulletSelectorFactory = new BulletSelectorFactory(_maxAmmo);

        ChangeBullet(_bulletType);
        if (_player.IsOwner)
        {
            _inputReader.OnFireEvent += HandleShoot;
        }

    }

    [ContextMenu("Test Shoot")]
    private void HandleShoot()
    {
        if (_isOnCoolDown)
            return;

        if (!CanShoot())
            return;
        _eventBus?.Broadcast(nameof(GameplayEvents.ON_PERFORM_SHOOT), _currentProjectile);
        StartCoroutine(FireRateCoolDown());
    }


    private bool CanShoot()
    {
        return _currentProjectile != null && _ammoProvider.HasAmmo;
    }

    private IEnumerator FireRateCoolDown()
    {
        _isOnCoolDown = true;
        _coolDown = _fireRateCoolDown;
        while (_coolDown > 0)
        {
            float remappedCoolDown = MathUtils.Remap(_coolDown, 0, _fireRateCoolDown, 0, 1);
            _eventBus?.Broadcast(nameof(GameplayEvents.ON_FIRE_COOLDOWN_UPDATED), remappedCoolDown);
            _coolDown -= Time.deltaTime;
            yield return null;
        }
        _eventBus?.Broadcast(nameof(GameplayEvents.ON_FIRE_COOLDOWN_UPDATED),0);
        _isOnCoolDown = false;
    }
    public void ChangeBullet(BulletType bulletType)
    {
        _currentProjectile = _bulletSelectorFactory.GetBulletByType(bulletType);
        _fireRateCoolDown = _currentProjectile.CoolDown;
        _eventBus?.Broadcast(nameof(GameplayEvents.ON_BULLET_TYPE_CHANGED), _currentProjectile);
    }
}
