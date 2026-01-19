using System.Collections;
using UnityEngine;

public class WheelTrail : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private float _lifeSpan = 2f;
    public GameObject GameObject => gameObject;
    private IResettablePool _trailPool;
    private WaitForSeconds _lifeSpanExecution;
    private bool _isActive = false;

    private void Start()
    {
        _lifeSpanExecution = new WaitForSeconds(_lifeSpan);
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
        _trailPool.ReturnObjectToPool(this);
    }

    public void Initialize(IResettablePool pool)
    {
        _trailPool = pool;
    }
}
