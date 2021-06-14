using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridObstacle
{
    public List<Vector2Int> OccupiedGridCells { get;} 
}
