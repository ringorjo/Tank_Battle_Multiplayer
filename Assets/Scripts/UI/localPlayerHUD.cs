using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class localPlayerHUD : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _playerLabel;

    [SerializeField]
    private TextMeshProUGUI _lifeCounter;

    [SerializeField]
    private Image _ammoBar;
    [SerializeField]
    private Image _healthBar;

    private Player _player;


    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.CurrentHealth.OnValueChanged -= OnHealthUpdated;
            _player.Lives.OnValueChanged -= OnLifesUpdated;
            _player.Bullets.OnValueChanged -= OnAmmoLenghUpdated;
            _player.PlayerName.OnValueChanged -= OnPlayerUpdated;
        }
    }

    public void SetupLocalPlayer(Player player)
    {
        _player = player;
        _player.CurrentHealth.OnValueChanged += OnHealthUpdated;
        _player.Lives.OnValueChanged += OnLifesUpdated;
        _player.Bullets.OnValueChanged += OnAmmoLenghUpdated;
        _player.PlayerName.OnValueChanged += OnPlayerUpdated;
        InitViewData(player);
    }

    private void InitViewData(Player player)
    {
        OnPlayerUpdated(string.Empty, player.PlayerName.Value);
        OnLifesUpdated(0, player.Lives.Value);
        OnAmmoLenghUpdated(0, player.Bullets.Value);
        OnHealthUpdated(0, player.CurrentHealth.Value);
    }

    private void OnPlayerUpdated(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        _playerLabel.text = newValue.ToString();
    }

    private void OnLifesUpdated(int previousValue, int newValue)
    {
        _lifeCounter.text = newValue.ToString();
    }

    float ammofill;
    private void OnAmmoLenghUpdated(int previousValue, int newValue)
    {
        ammofill = (float)newValue / _player.MaxBullets;
        _ammoBar.fillAmount = ammofill;
    }

    private void OnHealthUpdated(int previousValue, int newValue) => _healthBar.fillAmount = (float)newValue / _player.MaxHealth;

}
