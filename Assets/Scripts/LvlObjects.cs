using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlObjects : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _obstaclesGrid;
    [SerializeField] private float _renderDistance;
    public PlacedObstacle Obstacle;

    public Transform ObstaclesGrid => _obstaclesGrid;
    public Transform StartPoint => _startPoint;
    public Transform EndPoint => _endPoint;
    public float RenderDistance => _renderDistance;

}

