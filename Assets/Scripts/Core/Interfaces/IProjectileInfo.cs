
using System;

public interface IProjectileInfo
{
    public BulletType Type { get; }
    public int LifeTime { get; }
    public int Damage { get; }
    public int BulletSpeed { get; }

    public float CoolDown { get; }

}

public interface IAmmoProvider
{
    event Action<int> OnAmmoChanged;
    int CurrentAmmo { get; }
    int MaxAmmo { get; }
    bool HasAmmo { get; }


    void SetMaxAmmo(int maxAmmo);
    public void ReloadAmmo();
}
