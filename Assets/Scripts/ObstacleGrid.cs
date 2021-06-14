using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
 
    public Vector3 GridStartPos { get; private set; }
    public Vector3 GridEndPos => GetCellStartPos(Width, Height);
    
    private IGridObstacle[,] _gridCells;

    public ObstacleGrid(int width, int height, Vector3 gridStartPos)
    {
        Width = width;
        Height = height; 
        GridStartPos = gridStartPos;
        _gridCells = new IGridObstacle[width, height];
    } 

    public void DrawGrid()
    {
        Gizmos.color = Color.red;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Gizmos.DrawLine(GetCellStartPos(x, y), GetCellStartPos(x, y + 1));
                Gizmos.DrawLine(GetCellStartPos(x, y), GetCellStartPos(x + 1, y));
            }
        }
        Gizmos.DrawLine(GetCellStartPos(0, Height), GetCellStartPos(Width, Height));
        Gizmos.DrawLine(GetCellStartPos(Width, 0), GetCellStartPos(Width, Height));
    }

    public void ClearGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _gridCells[x, y] = null;
            }
        }
    }

    public Vector3 GetCellStartPos(float x, float y)
    {
        return new Vector3(x, y, 0) * Constants.GridCellSize + GridStartPos;
    }

    public void SetValue(int x, int y, IGridObstacle obstacle)
    {
        _gridCells[x, y] = obstacle;
    }

    public void SetValue(IGridObstacle  obstacle)
    {
        for (int i = 0; i < obstacle.OccupiedGridCells.Count; i++)
        {
            SetValue(obstacle.OccupiedGridCells[i].x, obstacle.OccupiedGridCells[i].y, obstacle);
        }
    }

    public Vector2Int CountGridSize(Vector3 size)
    {
        Vector2Int gridSize = new Vector2Int();

        gridSize.x = Mathf.CeilToInt((size.x) / Constants.GridCellSize);
        gridSize.y = Mathf.CeilToInt((size.y) / Constants.GridCellSize);
        return gridSize;
    }

    public Vector3 CorrectSize(Vector3 size)
    {
        size = new Vector3(RoundValue(size.x), RoundValue(size.y), RoundValue(size.z));

        Vector2Int gridSize = CountGridSize(size);
        float maxSizePerCell = Constants.GridCellSize - Constants.GridCellSize / 20;
        Vector3 maxSize = new Vector3(gridSize.x * maxSizePerCell, gridSize.y * maxSizePerCell, 0);

        if (size.x > maxSize.x)
            size.x = maxSize.x;

        if (size.y > maxSize.y)
            size.y = maxSize.y;

        return size;
    }

    private float RoundValue(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

    public IGridObstacle GetValue(int x, int y)
    {
        return _gridCells[x, y];
    }

    public void RemoveValue(int x, int y)
    {
        SetValue(x, y, null);
    }

    public IGridObstacle GetValueAtPos(Vector3 pos)
    {
        if(CanGetSellByPos(pos, out Vector2Int cellPos))
        {
            return GetValue(cellPos.x, cellPos.y);
        }
        return null;
    }

    public bool CanGetSellByPos(Vector3 position, out Vector2Int cellPos)
    {
        cellPos = new Vector2Int();
        if (position.x < GridStartPos.x || position.y < GridStartPos.y || position.x >= GridEndPos.x || position.y >= GridEndPos.y)
        {
            return false;
        }

        Vector3 positionInGrid = position - GridStartPos; 

        cellPos.x = Mathf.FloorToInt(positionInGrid.x / Constants.GridCellSize);
        cellPos.y = Mathf.FloorToInt(positionInGrid.y / Constants.GridCellSize);

        return true;
    }

    public bool IfCanPlaceGetInfo(Vector3 worldPos, Vector2Int gridSize, out Vector3 position, out List<Vector2Int> occupiedCells)
    {
        position = new Vector3();
        occupiedCells = new List<Vector2Int>();
        if (!CanGetSellByPos(worldPos, out Vector2Int cellPos))
        {
            return false;
        }

        return IfCanPlaceGetInfo(cellPos, gridSize, out position, out occupiedCells);
    }

    public bool IfCanPlaceGetInfo(Vector2Int cellPos, Vector2Int gridSize, out Vector3 position, out List<Vector2Int> occupiedCells)
    {
        position = new Vector3();
        occupiedCells = new List<Vector2Int>();

        if (cellPos.x >= Width || cellPos.y >= Height)
            return false;

        position = GetCellStartPos(cellPos.x, cellPos.y);
        if (gridSize == Vector2.one)
        {
            if (GetValue(cellPos.x, cellPos.y) == null)
            {
                occupiedCells.Add(new Vector2Int(cellPos.x, cellPos.y));
                return true;
            }
        }
        else if (IfCanSetFormCellToCell(new Vector2Int(cellPos.x, cellPos.y), gridSize, out List<Vector2Int> cells))
        {
            occupiedCells = cells;
            return true;
        }

        return false;
    }

    private bool IfCanSetFormCellToCell(Vector2Int cellPos, Vector2Int gridSize,  out List<Vector2Int> occupiedCells)
    {
        occupiedCells = new List<Vector2Int>();

        if (cellPos.x >= Width || cellPos.y >= Height)
            return false;

        for (int j = cellPos.y; j < cellPos.y + gridSize.y; j++)
        {
            for (int i = cellPos.x; i < cellPos.x + gridSize.x; i++)
            {
                if (i >= Width  || j >= Height  || _gridCells[i, j] != null)
                {
                    occupiedCells = null;
                    return false;
                }
                occupiedCells.Add(new Vector2Int(i, j));
            }
        }
        return true;
    }
}


