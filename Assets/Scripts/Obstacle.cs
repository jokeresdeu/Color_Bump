using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{ 
    private bool _friendly;
    private MeshRenderer _meshRenderer;
    private ServiceLocator _serviceLocator;
    private Rigidbody _rigidBody;

    private Vector3 _localPos;
    private Quaternion _localRot;

    public void Init()
    {
        _serviceLocator = ServiceLocator.Instance;
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void MoveToScene(Material material)
    {
        //_serviceLocator.LvlController.PlayerMaterialChanged += OnPlayerMaterialChaged;
         //_friendly = material  == _serviceLocator.LvlController.PlayerMaterial;
        _meshRenderer.sharedMaterial = material;
        _localPos = transform.localPosition;
        _localRot = transform.localRotation;
    }

    public void OnReturnToPool()
    {
        //_serviceLocator.LvlController.PlayerMaterialChanged -= OnPlayerMaterialChaged;
        _rigidBody.velocity = Vector3.zero;
        transform.localPosition = _localPos;
        transform.localRotation = _localRot;
    }

    private void OnPlayerMaterialChaged()
    {
        _friendly = !_friendly;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_friendly)
            return;

        //if (!ServiceLocator.Instance.LvlController.LvlStarted)
        //    return;

        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
            player.OnDeath();
    }

    //private void OnDestroy()
    //{
    //    _serviceLocator.LvlController.PlayerMaterialChanged += OnPlayerMaterialChaged;
    //}
}


