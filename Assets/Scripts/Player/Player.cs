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
    [SerializeField]
    private int _maxBullets = 5;


    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(
        100,
         NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );

    public NetworkVariable<ulong> PlayerOwnerId = new(
   99,
   NetworkVariableReadPermission.Everyone,
   NetworkVariableWritePermission.Server
);

    public NetworkVariable<int> Bullets = new(
       5,
       NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server
   );

    public NetworkVariable<int> Lives = new(
        3,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );


    public bool CanShoot
    {
        get => Bullets.Value > 0;
    }

    public void UpdateAmmoLenght()
    {
        if (Bullets.Value > 0)
            Bullets.Value--;
    }
    public void ReloadAmmo(int ammoCount)
    {
        if (!IsServer)
            return;
        Bullets.Value += ammoCount;
    }

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
    public int MaxBullets
    {
        get => _maxBullets;
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
        CurrentHealth.Value = _maxHealth;
        Bullets.Value = 5;
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
