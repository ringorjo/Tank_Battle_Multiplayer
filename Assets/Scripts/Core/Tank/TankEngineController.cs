using System;
using UnityEngine;

public class TankEngineController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _engineAudioSource;
    [SerializeField]
    private AudioClip _neutroClip;
    [SerializeField]
    private AudioClip _movementClip;
    [SerializeField]
    private Vector2 _limitPitch;
    [SerializeField]
    private TankMovement _tankMovement;

    private void Start()
    {
        _tankMovement.OnSpeedChanged += OnSpeedChanged;
        _engineAudioSource.clip = _neutroClip;
        _engineAudioSource.pitch = 1;
        _engineAudioSource.Play();


    }
    private void OnDestroy()
    {
        _tankMovement.OnSpeedChanged -= OnSpeedChanged;
    }

    private void OnSpeedChanged(float speed)
    {
        if (speed == 0 && _engineAudioSource.clip != _neutroClip)
        {
            _engineAudioSource.clip = _neutroClip;
            _engineAudioSource.pitch = 1;
            PerformPlay();
        }
        else if (speed > 0)
        {
            _engineAudioSource.clip = _movementClip;
            _engineAudioSource.pitch = Mathf.Lerp(_limitPitch.x, _limitPitch.y, speed);
            PerformPlay();
        }

    }

    private void PerformPlay()
    {
        if (!_engineAudioSource.isPlaying)
        {
            _engineAudioSource.Play();
        }
    }
}
