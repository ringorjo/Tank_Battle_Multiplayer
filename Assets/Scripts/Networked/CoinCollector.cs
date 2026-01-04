using Unity.Netcode;
using UnityEngine;

public class CoinCollector : NetworkBehaviour
{
    private Player _player;
    private void Start()
    {
        _player = GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Coin coin))
        {
            return;
        }
        if (coin.IsCollected)
            return;

        coin.Collect();
        if (IsServer)
        {
            HandleCoinCollector(coin);
        }
    }

    private void HandleCoinCollector(Coin coin)
    {
        switch (coin.CoinType)
        {
            case CoinType.Ammo:
                BulletCoin bulletCoin = (BulletCoin)coin;
                _player.ReloadAmmo(bulletCoin.BulletAmount);
                break;
            case CoinType.Life:
                break;

        }
    }
}
