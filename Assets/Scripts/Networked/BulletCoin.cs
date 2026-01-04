using UnityEngine;

public class BulletCoin : Coin
{
    [SerializeField]
    private int _bulletAmount = 5;

    public int BulletAmount 
    { 
        get => _bulletAmount; 
    }

    public override void Collect()
    {
        if (_isCollected)
            return;

        if (!IsServer)
        {
            ChangeVisbillity(false);
            return;
        }
        _isCollected = true;
    }


}
