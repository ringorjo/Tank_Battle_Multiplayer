using System.Collections.Generic;
using UnityEngine;

public class BulletSelectorFactory
{
    private Dictionary<BulletType, IProjectileInfo> _bulletFactory;

    public BulletSelectorFactory(int maxBulletCapacity)
    {
        _bulletFactory = new Dictionary<BulletType, IProjectileInfo>
        {
            {BulletType.Standard,new StandardBullet(maxBulletCapacity)}
        };
    }

    public IProjectileInfo GetBulletByType(BulletType type)
    {
        if (!_bulletFactory.TryGetValue(type, out var value))
            return null;

        Debug.Log("Bullet Selectd: " + type);
        return value;
    }

}
