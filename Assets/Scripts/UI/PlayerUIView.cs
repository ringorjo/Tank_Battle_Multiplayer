using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIView : NetworkBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private TextMeshProUGUI _playerLabel;

    [SerializeField]
    private Image _healthBar;


    override public void OnNetworkSpawn()
    {
        if (!IsClient)
        {
            return;
        }
        _player.CurrentHealth.OnValueChanged += UpdateHealthBar;
        _player.PlayerName.OnValueChanged += OnPlayerNameUpdated;
        UpdateHealthBar(_player.CurrentHealth.Value, _player.MaxHealth);
        OnPlayerNameUpdated(string.Empty, _player.PlayerName.Value);



    }

    private void OnPlayerNameUpdated(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        _playerLabel.text = newValue.Value;
       // Debug.Log("PlayerName: " + newValue.Value);
    }
    private void UpdateHealthBar(int previoudHealth, int currentHealth)
    {
        _healthBar.fillAmount = (float)currentHealth / _player.MaxHealth;
       // Debug.Log("Heath: " + _healthBar.fillAmount);
    }
    public override void OnNetworkDespawn()
    {
        if (!IsClient)
        {
            return;
        }
        //Debug.Log($"HealthUIView : HealthUIView Disposed: ");
    }



    
}
