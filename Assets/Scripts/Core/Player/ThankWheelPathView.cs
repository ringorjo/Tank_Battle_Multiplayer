using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ThankWheelPathView : MonoBehaviour
{
    [SerializeField]
    private GameObject _trailPrefab;
    [SerializeField]
    private Transform _leftWheel;
    [SerializeField]
    private Transform _rightWheel;
    [Header("Settings")]
    [SerializeField]
    private float _pathDrawTolerance;
    [SerializeField, Range(0, 2)]
    private float _updateInterval = 0.1f;
    [SerializeField]
    private int _initPool = 10;
    private ObjectPool<WheelTrail> _trailPool;
    private Transform _wheelParent;
    private Vector3 _lastParentPos;
    private WaitForSeconds _updateWait;

    private void Awake()
    {
        _updateWait = new WaitForSeconds(_updateInterval);
        _wheelParent = transform;
        _lastParentPos = _wheelParent.position;

    }
    private void Start()
    {
        _trailPool = new ObjectPool<WheelTrail>(_trailPrefab, transform);
        _trailPool.InitPool(_initPool, "WheelTrail");
        StartCoroutine(SpawnWheelTrails());
    }

    private IEnumerator SpawnWheelTrails()
    {
        while (true)
        {
            if (Vector3.Distance(_lastParentPos, _wheelParent.position) < _pathDrawTolerance)
            {
                yield return null;
                continue;
            }
            yield return _updateWait;
            WheelTrail leftTrail = _trailPool.GetObject(_leftWheel.position, _wheelParent.rotation);
            leftTrail.OnSpawn();
            WheelTrail rightTrail = _trailPool.GetObject(_rightWheel.position, _wheelParent.rotation);
            rightTrail.OnSpawn();
            _lastParentPos = _wheelParent.position;
        }
    }

}
