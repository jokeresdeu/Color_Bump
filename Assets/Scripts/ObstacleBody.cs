using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleBody", menuName = "LvlCreator/ObstacleBody", order = 1)]
public class ObstacleBody : ScriptableObject
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private ObstacleShape _shape;

    public Mesh Mesh => _mesh;
    public Vector3 Offset => _offset;
    public Vector3 Rotation => _rotation;
    public ObstacleShape Shape => _shape;


}
