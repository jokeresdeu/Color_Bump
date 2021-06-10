using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDummy : IGridObstacle
{
    public List<Vector2Int> OccupiedGridCells { get; private set; }
    public ObstacleDummy(List<Vector2Int> cells)
    {
        OccupiedGridCells = cells;
    }
}
