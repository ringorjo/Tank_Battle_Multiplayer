using System;
using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IDamage, IHealeable, IService
{

    public Action OnDie;

    [Header("Player Settings")]

    [SerializeField]
    private int _maxHealth = 100;
    private ulong _ownerId = 0;

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    [HideInInspector]
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

    [Header("PlayerCamera")]
    [SerializeField]
    private GameObject _camera;

    private SessionManagerService _sessionManagerService;


    public int MaxHealth
    {
        get => _maxHealth;
    }
    public ulong OwnerId
    {
        get => _ownerId;
    }
    private void Awake()
    {
        _sessionManagerService = ServiceLocator.Instance.Get<SessionManagerService>();
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
        yield return new WaitUntil(() => PlayerName.Value.Length > 0);
        gameObject.name = PlayerName.Value.ToString();
        _sessionManagerService?.AddPlayer(this);
    }

    public void SetupPlayer(ulong clientId)
    {
        _ownerId = clientId;
        PlayerName.Value = $"Player_{_ownerId}";
        CurrentHealth.Value = _maxHealth;
    }

    public override void OnNetworkDespawn()
    {
        _sessionManagerService?.RemovePlayer(this);

        if (IsOwner)
        {
            Unregister();
        }
    }

    public void TakeDamage(BulletInfo bulletInfo)
    {
        if (!IsServer || bulletInfo.PlayerOwner == PlayerName.Value)
        {
            return;
        }

        UpdateHealth(bulletInfo.Damage, true);
        if (CurrentHealth.Value <= 0)
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
        int value = istakeDamage ? CurrentHealth.Value - newHealth : CurrentHealth.Value + newHealth;
        CurrentHealth.Value = Mathf.Clamp(value, 0, _maxHealth);
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
