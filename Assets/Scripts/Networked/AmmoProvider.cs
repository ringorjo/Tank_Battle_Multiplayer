using System;
using UnityEngine;

public class AmmoProvider : MonoBehaviour, IAmmoProvider
{
    [SerializeField]
    private BulletNetcodeHandler _bulletnetcodeHandler;
    private int _maxAmmo;

    public event Action<int> OnAmmoChanged;

    private void Start()
    {
        _bulletnetcodeHandler.Ammo.OnValueChanged += OnAmmoUpdated;
    }

    private void OnAmmoUpdated(int previousValue, int newValue)
    {
        OnAmmoChanged?.Invoke(newValue);
    }

    public void ReloadAmmo()
    {
        _bulletnetcodeHandler.Ammo.Value = _maxAmmo;
    }

    public void SetMaxAmmo(int maxAmmo)
    {
        _maxAmmo = maxAmmo;
    }

    public int CurrentAmmo => _bulletnetcodeHandler.Ammo.Value;

    public int MaxAmmo => _maxAmmo;

    public bool HasAmmo => _bulletnetcodeHandler.Ammo.Value > 0;
}



