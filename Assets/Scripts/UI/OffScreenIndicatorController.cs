using System.Collections.Generic;

using UnityEngine;

public class OffScreenIndicatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject _offsecreenPrefab;
    [SerializeField]
    private RectTransform _indicatorsParent;
    [SerializeField]
    private HashSet<Indicator2D> _indicators = new HashSet<Indicator2D>();
    private SessionManagerService _sessionManagerService;




    private void Start()
    {
        _sessionManagerService = ServiceLocator.Instance.Get<SessionManagerService>();
        if (_sessionManagerService != null)
        {
            _sessionManagerService.OnPlayerJoined += HandlePlayerJoined;
            _sessionManagerService.OnPlayerLeft += HandlePlayerLeft;

        }
    }
    private void Update()
    {
        foreach (var indicator in _indicators)
        {
            indicator.UpdateIndicator();
        }
    }

    private void HandlePlayerLeft(Player player)
    {
        if (player == _sessionManagerService.GetLocalPlayer())
            return;
        Indicator2D indicator = GetIndicatorByPlayer(player);
        if (indicator != null)
            _indicators.Remove(indicator);
    }

    private void OnDestroy()
    {
        if (_sessionManagerService != null)
            _sessionManagerService.OnPlayerJoined -= HandlePlayerJoined;
    }

    private Indicator2D GetIndicatorByPlayer(Player player)
    {
        foreach (var indicator in _indicators)
        {
            if (indicator.Player == player)
                return indicator;
        }
        return null;
    }

    private void HandlePlayerJoined(Player player)
    {
        if (player == _sessionManagerService.GetLocalPlayer())
            return;

        Indicator2D indicator = Instantiate(_offsecreenPrefab, _indicatorsParent).GetComponent<Indicator2D>();
        indicator.ReceivePlayerInfo(player);
        _indicators.Add(indicator);
    }
}
