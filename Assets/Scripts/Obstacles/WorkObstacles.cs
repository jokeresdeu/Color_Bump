using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkObstacle", menuName = "LvlCreator/WorkObstacles", order = 1)]
public class  WorkObstacles: ScriptableObject
{
    [SerializeField] private List<WorkObstacle> _workObstacles;
    public List<WorkObstacle> Obstacles => _workObstacles;
}

[System.Serializable]
public class WorkObstacle
{
    [SerializeField] private PlacedObstacle _obstaclePrefab;
    [SerializeField] private Sprite _hotbarImage;

    public PlacedObstacle ObstaclePrefab => _obstaclePrefab;
    public Sprite HotbarImage => _hotbarImage;
}
