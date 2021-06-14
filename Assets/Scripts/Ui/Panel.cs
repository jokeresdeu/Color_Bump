using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    protected UiContainer UiContainer;
    protected ServiceLocator ServiceLocator;
    public virtual void Init(UiContainer container)
    {
        UiContainer = container;
        ServiceLocator = ServiceLocator.Instance;
    }
}
