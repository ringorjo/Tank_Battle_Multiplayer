using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TanksVisualController : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private List<SpriteRenderer> _sprites;
    [Header("Damage visual Settings")]
    [SerializeField]
    [ColorUsage(true, true)]
    private Color _flashDamageColor;
    private Color _targetColor;
    [SerializeField]
    private float _flashSpeed;
    [SerializeField]
    private AnimationCurve _flashCurve;
    [SerializeField]
    [Range(0f, .4f)]
    private float _t;
    private bool _isPerformingDamage = false;

    [SerializeField]
    private List<Color> _colors;

    private float _previousValue;
    private bool _wasRising;

    private void Start()
    {
        _colors = new List<Color>(_sprites.Select(s => s.color));
        _player.PlayerStats.CurrentHealth.OnValueChanged += OnHealthChanged;
    }
    [ContextMenu(nameof(setcolor))]
    private void setcolor()
    {
        _colors = new List<Color>(_sprites.Select(s => s.color));
    }
    private void OnDestroy()
    {
        _player.PlayerStats.CurrentHealth.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float previousValue, float newValue)
    {
        PerformFlashDamageCorutine();

    }

    private IEnumerator PerformFlashDamage()
    {
        float time = 0;
        float value = 0;
        _isPerformingDamage = true;
        while (time < _flashSpeed)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _flashSpeed);
            value = _flashCurve.Evaluate(t);
            InterpolateColor(value);
            yield return null;
        }
        _isPerformingDamage = false;
        for (int i = 0; i < _sprites.Count; i++)
            _sprites[i].color = _colors[i];
    }

    private void PerformFlashDamageCorutine()
    {
        if (_isPerformingDamage)
            return;
        StartCoroutine(PerformFlashDamage());
    }

    private void InterpolateColor(float time)
    {
        bool isRising = time > _previousValue;

        if (_wasRising && !isRising)
        {
            _targetColor = _colors[0];
        }

        if (!_wasRising && isRising)
        {
            _targetColor = _flashDamageColor;
        }
        foreach (var sprite in _sprites)
            sprite.color = Color.Lerp(sprite.color, _targetColor, time);

        _previousValue = time;
        _wasRising = isRising;
    }
}
