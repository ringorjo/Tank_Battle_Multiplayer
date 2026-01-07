using System;

public interface IProjectileInfo
{
    public event Action<int> OnBulletRoundsChanged;
    public BulletType Type { get; }
    public int RoundsRemaining { get; }
    public int LifeTime { get; }

    public int Damage { get; }
    public int BulletSpeed { get; }
    public int MaxAmmoCount { get; }
    public bool HasAmmo { get; }
    public void UpdateRemaingAmmount();
    public void ReloadAmmo();

}
