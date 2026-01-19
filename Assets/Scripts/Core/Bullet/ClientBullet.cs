using UnityEngine;

public class ClientBullet : BulletBase
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnDestroyBullet();
    }
}
