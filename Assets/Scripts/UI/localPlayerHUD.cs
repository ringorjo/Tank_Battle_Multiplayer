using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class localPlayerHUD : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private TextMeshProUGUI _playerLabel;

    [Header("Health UI")]
    [SerializeField]
    private TextMeshProUGUI _lifeCounter;
    [SerializeField]
    private Image _healthBar;

    [Header("Ammo UI")]

    [SerializeField]
    private Image _ammoBar;
    [SerializeField]
    private Image _cooldownFireImage;
    private float _ammofill;

    private EventBusService _eventBus;
    private IAmmoProvider _ammoProvider;

    public void SetupLocalPlayer(Player player)
    {
        _player = player;
        _eventBus = ServiceLocator.Instance.Get<EventBusService>();
        if (_eventBus != null)
        {
            _eventBus.Subscribe<int>(nameof(GameplayEvents.ON_LIFE_COUNT_UPDATED), OnLifesUpdated);
            _eventBus.Subscribe<float>(nameof(GameplayEvents.ON_HEALH_UPDATED), OnHealthUpdated);
            _eventBus.Subscribe<float>(nameof(GameplayEvents.ON_FIRE_COOLDOWN_UPDATED), (cooldown) =>
            {
                _cooldownFireImage.fillAmount = cooldown;
            });
        }

        _player.PlayerName.OnValueChanged += OnPlayerUpdated;
        _ammoProvider = _player.PlayerAmmoProvider;
        _ammoProvider.OnAmmoChanged += OnAmmoLenghUpdated;
        OnBulletChanged(null);
        InitViewData(player);
    }


    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.PlayerName.OnValueChanged -= OnPlayerUpdated;
            _player.PlayerAmmoProvider.OnAmmoChanged -= OnAmmoLenghUpdated;
        }
        if (_eventBus != null)
        {
            _eventBus.UnSusbcribe<int>(nameof(GameplayEvents.ON_LIFE_COUNT_UPDATED), OnLifesUpdated);
            _eventBus.UnSusbcribe<IProjectileInfo>(nameof(GameplayEvents.ON_BULLET_TYPE_CHANGED), OnBulletChanged);
            _eventBus.UnSusbcribe<float>(nameof(GameplayEvents.ON_HEALH_UPDATED), OnHealthUpdated);
        }
    }


    private void OnBulletChanged(IProjectileInfo info)
    {
        if (_ammoProvider == null)
        {
            Debug.LogError("IAmmoProvider is null");
            return;
        }
        OnAmmoLenghUpdated(_ammoProvider.CurrentAmmo);
    }

    private void InitViewData(Player player)
    {
        OnPlayerUpdated(string.Empty, player.PlayerName.Value);
        OnLifesUpdated(player.PlayerStats.Lives.Value);
        OnHealthUpdated(player.PlayerStats.CurrentHealth.Value);
    }

    private void OnPlayerUpdated(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        _playerLabel.text = newValue.ToString();
    }

    private void OnLifesUpdated(int lifeCount)
    {
        _lifeCounter.text = lifeCount.ToString();
    }


    private void OnAmmoLenghUpdated(int reamainingAmmo)
    {
        _ammofill = (float)reamainingAmmo / _ammoProvider.MaxAmmo;
        _ammoBar.fillAmount = _ammofill;
    }

    private void OnHealthUpdated(float newValue) => _healthBar.fillAmount = (float)newValue / _player.PlayerStats.MaxHealth;

}
