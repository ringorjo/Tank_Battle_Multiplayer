using UnityEngine;

public class PlayerHUDSpawner : MonoBehaviour
{
    [SerializeField]
    private localPlayerHUD _playerHudPrefab;
    [SerializeField]
    private Canvas _canvas;
    private SessionManagerService _sessionManagerService;

    private void Start()
    {
        _sessionManagerService = ServiceLocator.Instance.Get<SessionManagerService>();
        if (_sessionManagerService != null)
        {
            _sessionManagerService.OnPlayerJoined += OnPlayerJoined;
        }
    }

    private void OnDestroy()
    {
        if (_sessionManagerService != null)
            _sessionManagerService.OnPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(Player player)
    {
        if (player.IsOwner)
        {
            localPlayerHUD hud = Instantiate(_playerHudPrefab, _canvas.transform);
            hud.SetupLocalPlayer(_sessionManagerService.GetLocalPlayer());
        }
       
    }
}
