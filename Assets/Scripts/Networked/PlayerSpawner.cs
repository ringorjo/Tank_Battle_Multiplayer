using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        /// This callback is invoked on the server when a new client connects or Local Client is Connected.
        /// This Callback is NOT invoked on the client side. example to Know when other clients connect
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private async void OnClientConnected(ulong ownerClienId)
    {
        Player player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(ownerClienId).GetComponent<Player>();
        if (player == null)
            return;

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            player.SetupPlayer(ownerClienId + 1);
        }
        await Task.Delay(200); // Wait for the PlayerName to be set
        player.name = player.PlayerName.Value.ToString();
    }
}
