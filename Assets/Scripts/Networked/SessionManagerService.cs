using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionManagerService : MonoBehaviour, IService
{
    public event Action<Player> OnPlayerJoined;
    public event Action<Player> OnPlayerLeft;

    private Dictionary<ulong, Player> _players = new Dictionary<ulong, Player>();


    private void Awake()
    {
        Register();
    }
 

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
        Debug.Log("Added Player: " + player.PlayerName.Value.ToString());

        if (!_players.ContainsKey(player.OwnerId))
        {
            _players.Add(player.OwnerId, player);
            OnPlayerJoined?.Invoke(player);
        }
        Debug.Log("Players: " + _players.Count);

    }

    public void RemovePlayer(Player player)
    {
        if (_players.ContainsKey(player.OwnerId))
        {
            _players.Remove(player.OwnerId);
            OnPlayerLeft?.Invoke(player);
        }
    }


    private void OnDestroy()
    {
        Unregister();
    }

    public void Register() => ServiceLocator.Instance.Register(this);
    public void Unregister() => ServiceLocator.Instance.Unregister(this);

}
