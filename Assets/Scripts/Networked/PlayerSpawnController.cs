using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerSpawnController : NetworkBehaviour
{
    [SerializeField]
    private List<Transform> _spawnPoints;

    private Queue<Transform> _spawnAvailables;

    private Dictionary<ulong, Transform> _playerContentMap;


    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        _spawnAvailables = new Queue<Transform>(_spawnPoints);
        _playerContentMap = new Dictionary<ulong, Transform>();
    }

    public Transform AddPlayerSpawn(ulong playerId)
    {
        if (_spawnAvailables.Count == 0 || _playerContentMap.ContainsKey(playerId))
            return null;

        Transform transform = _spawnAvailables.Dequeue();
        _playerContentMap[playerId] = transform;
        return RespawnPlayer(playerId);
    }

    public Transform RespawnPlayer(ulong playerId)
    {
        Transform result = null;
        _playerContentMap.TryGetValue(playerId, out result);
        return result;
    }

    
}


