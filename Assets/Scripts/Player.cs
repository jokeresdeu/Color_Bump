using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ForwardMover
{
    [SerializeField] private float _speedModifier;
    [SerializeField] private float _smoothVal;

    private Vector2 _prevMousePosition;
    private Vector2 _delta;
    private bool _controled;


    public MeshRenderer MeshRenderer { get; private set; }
    
    public event Action PlayerMoved = delegate { };
    public event Action<bool> PlayerEndedLvl = delegate { };

    public override void Init()
    {
        base.Init();
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void Subscribe()
    {
        base.Subscribe();
        ServiceLocator.UpdateCalled += OnUpdate;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
        ServiceLocator.UpdateCalled -= OnUpdate;
    }
    private bool go;

    protected void OnUpdate()
    {
#if UNITY_ANDROID
        if (Input.GetButtonDown("Fire1"))
        {
            if(!go)
            {
                PlayerMoved();
                go = true;
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

    protected override void OnFixedUpdate()
    {
        if (_delta != Vector2.zero)
        {
            Vector3 refVel = Vector3.zero;
            Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, Rigidbody.velocity + new Vector3(_delta.x, _delta.y, 0) * _speedModifier, ref refVel, _smoothVal);
            _delta = Vector2.zero;
        }

        if (_controled)
            return;

        base.OnFixedUpdate();

    }

    public void OnDeath()
    {
        //PlayerDied();
    }

    public void OnFinishLineCrossing()
    {

    }
}
