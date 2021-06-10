using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMover : MonoBehaviour
{
    protected const float ConstantForwardAcseleration = 50f;
    protected const float MinYSpeed = 3f;
    public virtual float Accseleration => ConstantForwardAcseleration;

    protected ServiceLocator ServiceLocator;

    protected Rigidbody Rigidbody;

    protected virtual void Start()
    {
        Init();
    }
    
    public virtual void Init()
    {
        ServiceLocator = ServiceLocator.Instance;
        Rigidbody = GetComponent<Rigidbody>();
        Subscribe();
    }

    protected virtual void OnFixedUpdate()
    {
        //if (!ServiceLocator.Instance.LvlController.LvlStarted)
        //    return;
        if(Rigidbody.velocity.y < MinYSpeed)
            Rigidbody.AddForce(new Vector3(-Rigidbody.velocity.x, Accseleration, 0), ForceMode.Acceleration);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    protected virtual void Subscribe()
    {
        ServiceLocator.FixedUpdateCalled += OnFixedUpdate;
    }

    protected virtual void Unsubscribe()
    {
        ServiceLocator.FixedUpdateCalled -= OnFixedUpdate;
    }

}
