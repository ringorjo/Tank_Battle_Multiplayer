public class StandardBullet : IProjectileInfo
{

    private int _currentAmmo;
    private int _totalAmmo;

    public StandardBullet(int totalAmmo)
    {
        _totalAmmo = totalAmmo;
        _currentAmmo = _totalAmmo;
    }

    public BulletType Type => BulletType.Standard;

    public int Damage => 10;

    public int BulletSpeed => 10;
    public int MaxAmmoCount => _totalAmmo;

    public bool HasAmmo => _currentAmmo > 0;

    public int RoundsRemaining => _currentAmmo;

    public int LifeTime => 2;

    public float CoolDown => 1.2f;

    public void UpdateRemaingAmmount(int currentAmmo)
    {
        _currentAmmo = currentAmmo;
    }
}