using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionManagerService : MonoBehaviour, IService
{
    public event Action<Player> OnPlayerJoined;
    public event Action<Player> OnPlayerLeft;

    private Dictionary<ulong, Player> _players = new Dictionary<ulong, Player>();


    private void Awake() => Register();
    private void OnDestroy() => Unregister();
    public void Register() => ServiceLocator.Instance.Register(this);
    public void Unregister() => ServiceLocator.Instance.Unregister(this);

    public Player GetPlayerByOwnerId(ulong ownerId)
    {
        if (_players.ContainsKey(ownerId))
        {
            return _players[ownerId];
        }
        return null;
    }

    public Player GetLocalPlayer()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;
        return GetPlayerByOwnerId(localClientId + 1);
    }

    public void AddPlayer(Player player)
    {
        if (!_players.ContainsKey(player.PlayerOwnerId.Value))
        {
            _players.Add(player.PlayerOwnerId.Value, player);
            OnPlayerJoined?.Invoke(player);
            Debug.Log($"Player Joined; {player.PlayerName.Value}");

            return;
        }

        Debug.Log($"Key already Added: {player.PlayerOwnerId.Value} ");
    }

    public void RemovePlayer(Player player)
    {
        if (_players.ContainsKey(player.PlayerOwnerId.Value))
        {
            _players.Remove(player.PlayerOwnerId.Value);
            OnPlayerLeft?.Invoke(player);
        }
    }
}
