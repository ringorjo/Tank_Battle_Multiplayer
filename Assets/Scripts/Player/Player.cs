using System;
using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IDamage, IHealeable, IService
{

    public Action OnDie;

    public NetworkVariable<ulong> PlayerOwnerId = new(
    99,
    NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Server
    );
    [HideInInspector]
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

    [Header("PlayerCamera")]
    [SerializeField]
    private GameObject _camera;

    private SessionManagerService _sessionManagerService;
    private BulletSpawner _bulletProvider;

    private PlayerStats _playerStats;
    public PlayerStats PlayerStats
    {
        get => _playerStats;
    }
    public BulletSpawner BulletProvider
    {
        get => _bulletProvider;
    }

    private void Awake()
    {
        _sessionManagerService = ServiceLocator.Instance.Get<SessionManagerService>();
        _playerStats = GetComponent<PlayerStats>();
        _bulletProvider = GetComponent<BulletSpawner>();
    }

    public override void OnNetworkSpawn()
    {
        StartCoroutine(WaitForSetup());
        if (IsOwner)
        {
            Register();
            return;
        }
        Destroy(_camera);
    }

    private IEnumerator WaitForSetup()
    {
        yield return new WaitUntil(() => PlayerOwnerId.Value != 99);
        AddLocalPlayer();
    }

    public void AddLocalPlayer()
    {
        gameObject.name = PlayerName.Value.ToString();
        _sessionManagerService?.AddPlayer(this);
    }

    //Only Server or Host SetPlayer
    public void SetupPlayer(ulong clientId)
    {
        PlayerOwnerId.Value = clientId;
        PlayerName.Value = $"Player_{PlayerOwnerId.Value}";
        _playerStats.CurrentHealth.Value = _playerStats.MaxHealth;
    }

    public override void OnNetworkDespawn()
    {
        _sessionManagerService?.RemovePlayer(this);

        if (IsOwner)
        {
            Unregister();
        }
    }

    public void TakeDamage(ProjectileContext bulletInfo)
    {
        if (!IsServer || bulletInfo.PlayerOwner == PlayerName.Value)
        {
            return;
        }

        UpdateHealth(bulletInfo.Damage, true);
        if (_playerStats.CurrentHealth.Value <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        if (!IsServer)
        {
            return;
        }
        UpdateHealth(healAmount);
    }

    private void UpdateHealth(int newHealth, bool istakeDamage = false)
    {
        float value = istakeDamage ? _playerStats.CurrentHealth.Value - newHealth : _playerStats.CurrentHealth.Value + newHealth;
        _playerStats.CurrentHealth.Value = Mathf.Clamp(value, 0, _playerStats.MaxHealth);
    }

    public void Register() => ServiceLocator.Instance.Register(this);

    public void Unregister() => ServiceLocator.Instance.Unregister(this);

}
