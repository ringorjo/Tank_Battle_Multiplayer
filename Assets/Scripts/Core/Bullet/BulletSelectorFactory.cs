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

// TODO CREAR EL REPOCITORIO PARA LAS BALAS
// PENSAR EN LA POOL CUANDO SE QUIERA CAMBIAR EL TIPO DE BALA PARA QUE CADA BALA PUEDA INJECTAR 
// LA INFROMACION ASI COMO CAMBIAR VISUALMENTE EL SPRITE
// IDEAS: EL SCRIPTABLE TENDRA UN SPRITE JUNTO CON EL ENUM PARA IR CAMBIANDO VISUALMENTE SU SPRITE
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BulletRepository", order = 1)]
public class BulletRepository:ScriptableObject
{

}
