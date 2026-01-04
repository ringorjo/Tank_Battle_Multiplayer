using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField]
    private CoinType coinType;
    [SerializeField]
    protected SpriteRenderer _coin;
    [SerializeField]
    protected CircleCollider2D _collider;
    [SerializeField]
    protected AudioSource _audioSource;
    [SerializeField]
    protected bool _isCollected = false;

    public CoinType CoinType
    {
        get => coinType;
    }
    public bool IsCollected
    {
        get => _isCollected;
    }

    public abstract void Collect();

    protected virtual void ChangeVisbillity(bool show)
    {
        _coin.enabled = show;
        _collider.enabled = show;
        if (!show)
        {
            _audioSource.Play();
            Debug.Log("Coin Collected");
        }
    }
}

public enum CoinType
{
    Ammo,
    Life
}
