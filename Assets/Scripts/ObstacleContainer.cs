using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleContainer: MonoBehaviour
{
    [SerializeField] protected Obstacle _obstacle;
    public float CurrentPosY { get; private set; }
    private Renderer _renderer;
    private Quaternion _startRotation;
    public void Init()
    {
        _obstacle.Init();
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void OnPutToScene(ObstacleInfo info, Material material)
    {
        transform.position = info.Position;
        transform.localScale = info.Size;
        _startRotation = transform.rotation;
        _obstacle.MoveToScene(material);
        CurrentPosY = info.Position.y;
    }

    public void OnReturnToPool()
    {
        _obstacle.OnReturnToPool();
        transform.rotation = _startRotation;
    }

   
}


