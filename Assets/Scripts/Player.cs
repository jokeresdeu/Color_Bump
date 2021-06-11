using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speedModifier;
    [SerializeField] private float _smoothVal;
    [SerializeField] private Rigidbody _parrent;
    private Vector2 _prevMousePosition;
    private Vector2 _delta;
    private bool _controled;

    private Rigidbody _rigidbody;
    public MeshRenderer MeshRenderer { get; private set; }
    
    public event Action PlayerMoved = delegate { };
    public event Action<bool> PlayerEndedLvl = delegate { };

    public void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
#if UNITY_EDITOR
        if (Input.GetButtonDown("Fire1"))
        {
           if(!ServiceLocator.Instance.LvlController.IsLvlStarted)
           {
                PlayerMoved();
           }

            _controled = true;
            _prevMousePosition = Input.mousePosition;
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            if (_delta != Vector2.zero)
                return;
            Vector2 currentMousePosition = Input.mousePosition;
            _delta = currentMousePosition - _prevMousePosition;
            _prevMousePosition = currentMousePosition;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _controled = false;
            _delta = Vector2.zero;
        }

#elif UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            if(!ServiceLocator.LvlController.LvlStarted)
            {
                PlayerMoved();
            }

            if (_delta != Vector2.yero)
                return;

            Touch touch = Input.GetTouch(0);

            _delta = touch.deltaPosition;
        }
#endif
    }

    protected void FixedUpdate()
    {
        if (_delta != Vector2.zero)
        {
            if (_delta.y < 0)
            {
                if (transform.position.y < Camera.main.transform.position.y + 2)
                    _delta.y = 0;
                else
                    _delta *= 2;
            }

            Vector3 refVel = Vector3.zero;
            _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, _rigidbody.velocity + new Vector3(_delta.x, _delta.y, 0) * _speedModifier, ref refVel, _smoothVal);
            _delta = Vector2.zero;
        }

        if (!_controled)
        {
            Vector3 balancingVelocity = new Vector3();

            if(_rigidbody.velocity.y < _parrent.velocity.y)
                balancingVelocity.y = (_parrent.velocity.y - _rigidbody.velocity.y )* 0.8f;

            if (_rigidbody.velocity.x != 0)
                balancingVelocity.x = -_rigidbody.velocity.x / 10;

            _rigidbody.velocity += balancingVelocity;
        }
    }

    public void OnDeath()
    {
        //PlayerDied();
    }

    public void OnFinishLineCrossing()
    {

    }
}
