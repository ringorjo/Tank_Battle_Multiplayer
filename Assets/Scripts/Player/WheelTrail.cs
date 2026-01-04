using System.Collections;
using UnityEngine;

public class WheelTrail : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private float _lifeSpan = 2f;
    public GameObject GameObject => gameObject;
    private ObjectPool<WheelTrail> _trailPool;
    private WaitForSeconds _lifeSpanExecution;
    private bool _isActive = false;

    private void Start()
    {
        _lifeSpanExecution = new WaitForSeconds(_lifeSpan);
    }
    public void Initialize<TPool>(ObjectPool<TPool> poolManager) where TPool : IPoolObject
    {
        _trailPool = poolManager as ObjectPool<WheelTrail>;
    }

    public void OnDespawn(Transform parent)
    {
        transform.SetParent(parent);
        _isActive = false;
    }

    public void OnSpawn()
    {
        transform.SetParent(null);
        if (!_isActive)
        {
            _isActive = true;
            StartCoroutine(PerformLifeSpanWheel());
        }
    }

    private IEnumerator PerformLifeSpanWheel()
    {
        yield return _lifeSpanExecution;
        _trailPool.RecycleObject(this);
    }
}
