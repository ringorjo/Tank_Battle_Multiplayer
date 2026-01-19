using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{

    [SerializeField]
    private float _maxHealth;

    public NetworkVariable<int> Lives = new(
        3,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(
       100,
        NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server
       );

    public float MaxHealth
    {
        get => _maxHealth;
    }
  
}
