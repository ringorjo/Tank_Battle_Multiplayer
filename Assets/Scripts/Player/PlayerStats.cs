using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
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

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(
       100,
        NetworkVariableReadPermission.Everyone,
       NetworkVariableWritePermission.Server
       );
}
