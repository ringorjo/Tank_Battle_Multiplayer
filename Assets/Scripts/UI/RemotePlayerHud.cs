using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RemotePlayerHud : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar;
    [SerializeField]
    private TextMeshProUGUI _playerLabel;
    [SerializeField]
    private Player _player;


    private void Start()
    {
        _player.CurrentHealth.OnValueChanged += OnHealthUpdated;
        _player.PlayerName.OnValueChanged += OnPlayerUpdated;
        OnHealthUpdated(0, _player.CurrentHealth.Value);
        OnPlayerUpdated(string.Empty, _player.PlayerName.Value);
        if (_player.IsOwner)
            gameObject.SetActive(false);
    }
    private void OnDestroy()
    {

        _player.CurrentHealth.OnValueChanged -= OnHealthUpdated;
        _player.PlayerName.OnValueChanged -= OnPlayerUpdated;
    }

    private void OnHealthUpdated(int previousValue, int newValue)
    {
        _healthBar.fillAmount = (float)newValue / _player.MaxHealth;
    }

    private void OnPlayerUpdated(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        _playerLabel.text = newValue.ToString();
    }
}
