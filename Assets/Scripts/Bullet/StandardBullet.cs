using System;
using UnityEngine;

public class StandardBullet : IProjectileInfo
{
    public event Action<int> OnBulletRoundsChanged;

    private int _loadedAmmo;
    private int _totalAmmo;

    public StandardBullet(int totalAmmo)
    {
        _totalAmmo = totalAmmo;
        _loadedAmmo = _totalAmmo;
    }

    public BulletType Type => BulletType.Standard;

    public int Damage => 10;

    public int BulletSpeed => 10;
    public int MaxAmmoCount => _totalAmmo;

    public bool HasAmmo => _loadedAmmo > 0;

    public int RoundsRemaining => _loadedAmmo;

    public int LifeTime => 2;


    public void ReloadAmmo()
    {
        _loadedAmmo = _totalAmmo;
        OnBulletRoundsChanged?.Invoke(_loadedAmmo);
    }

    public void UpdateRemaingAmmount()
    {
        if (_loadedAmmo > 0)
        {
            _loadedAmmo--;
            Debug.Log($"Bullet Count: {_loadedAmmo}");
            OnBulletRoundsChanged?.Invoke(_loadedAmmo);
        }
    }
}