using UnityEngine;

public class DestroyUtils : MonoBehaviour
{
    [SerializeField]
    private float _lifeTimeObject;
    [SerializeField]
    private bool _destroyWhenHit;
    [SerializeField]
    private string _tagHitDetection;



    private void Start()
    {
        Invoke(nameof(DestroyAfterTime), _lifeTimeObject);
    }

    private void DestroyAfterTime()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_destroyWhenHit)
            return;
        if (collision.CompareTag(_tagHitDetection))
        {
            DestroyAfterTime();
        }
    }

}
