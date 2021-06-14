using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacle : MonoBehaviour
{ 
    private bool _friendly;
    private MeshRenderer _meshRenderer;
    private ServiceLocator _serviceLocator;
    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    private Vector3 _localPos;
    private Quaternion _localRot;

    public void Init()
    {
        _serviceLocator = ServiceLocator.Instance;
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        //_audioSource.clip = _serviceLocator.SoundMap.Sounds.First(s => s.SoundId == SoundId.FriendlyHit).AudioClip;
    }

    public void MoveToScene(Material material)
    {
        _friendly = material == _serviceLocator.LvlController.LvlGenerator.DefaultMaterial;
        _meshRenderer.sharedMaterial = material;
        _localPos = transform.localPosition;
        _localRot = transform.localRotation;
    }

    public void OnReturnToPool()
    {
        _rigidBody.velocity = Vector3.zero;
        transform.localPosition = _localPos;
        transform.localRotation = _localRot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_friendly)
            return;

        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            if (_friendly)
            {
                PlayHit();
                return;
            }
               
            player.OnDeath();
        }
            
    }

    private void PlayHit()
    {
        if (_audioSource.isPlaying)
            return;

        _audioSource.Play();
    }
}


