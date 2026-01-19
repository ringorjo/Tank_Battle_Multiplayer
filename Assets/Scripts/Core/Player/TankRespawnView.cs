using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRespawnView : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> _sprites;
    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField, Range(0, 1)]
    private float _slideValue;


    private void OnValidate()
    {
        if (_sprites.Count == 0)
            return;
        _sprites.ForEach(s => s.color = new Color(1, 1, 1, _slideValue));
    }

    private void Start()
    {
        StartCoroutine(BlinkValue());
    }
    [ContextMenu(nameof(FillCollection))]
    private void FillCollection()
    {
        _sprites = new List<SpriteRenderer>(transform.GetComponentsInChildren<SpriteRenderer>());
    }

    private IEnumerator BlinkValue()
    {
        while(true)
        {
            _slideValue = Mathf.Lerp(0, 1, Mathf.PingPong(_speed*Time.deltaTime, _maxSpeed));
            _sprites.ForEach(s => s.color = new Color(1, 1, 1, _slideValue));
            yield return null;
        }
    }
}

  
