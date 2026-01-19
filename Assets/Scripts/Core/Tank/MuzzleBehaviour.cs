using System.Collections;
using UnityEngine;

public class MuzzleBehaviour : MonoBehaviour, IVisualable
{
    [SerializeField]
    private ParticleSystem _muzzleParticleSystem;
    [SerializeField]
    private AudioSource _audioSource;

   
    [ContextMenu("Perform Visuals")]
    public void PerformVisuals()
    {
       _muzzleParticleSystem.Play();
        _audioSource?.Play();
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
    }

    
}
