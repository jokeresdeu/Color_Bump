using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlacedObstacle : MonoBehaviour, IGridObstacle
{
    private const float maxXSize = 2f;
    private const float maxYSize = 2f;
    private const float maxZSize = 1f;

    private Vector3 _startSize = new Vector3(Constants.DefaultObstacleSize, Constants.DefaultObstacleSize, Constants.DefaultObstacleSize);

    [SerializeField] private ObstacleShape _shape;
    [SerializeField] private MeshRenderer _meshRenderer;
    private ObstacleGrid _occupiedGrid;

    public List<Vector2Int> OccupiedGridCells { get; set; }

    public ObstacleShape Shape => _shape;

    public Material ObstacleMaterial
    { 
        get
        {
            return _meshRenderer.sharedMaterial;
        }
        set
        {
            _meshRenderer.sharedMaterial = value;
            ObstaclePropertiesChanged();
        }
    }

    public Vector3 ObstacleSize
    {
        get
        {
            return transform.localScale;
        }
        private set
        {
            transform.localScale = value;
            ObstaclePropertiesChanged();
        }
    }

    public Vector2Int GridSize
    {
        get
        {
            return CountGridSize(ObstacleSize);
        }
    }

   

    public event Action ObstaclePropertiesChanged = delegate { };

    private void OnDestroy()
    {
        if (_occupiedGrid == null)
            return;

        for(int i = 0; i < OccupiedGridCells.Count; i++)
        {
            _occupiedGrid.RemoveValue(OccupiedGridCells[i].x, OccupiedGridCells[i].y);
        }
    }

    public void Init(Material material, bool onGrid = false)
    {
        ObstacleMaterial = material;
        if(!onGrid)
            ObstacleSize = _startSize;
    }

    public void Init(Material material, Vector3 size, List<Vector2Int> occupiedGridCells, ObstacleGrid occupiedGrid)
    {
        Init(material, true);

        ObstacleSize = occupiedGrid.CorrectSize(size);
        OccupiedGridCells = occupiedGridCells;
        _occupiedGrid = occupiedGrid;
    }

    public void ChangeSize(int x = 0, int y = 0, int z = 0)
    {
        if (CannotChangeSize(x, y, z))
            return;

        ObstacleSize +=  new Vector3(x * Constants.SizeDelta, y* Constants.SizeDelta, z* Constants.SizeDelta);
        ObstaclePropertiesChanged();
    }

    public void ResetSize()
    {
        ObstacleSize = _startSize;
    }

    private bool CannotChangeSize(int x, int y, int z)
    {
        return x > 0 && ObstacleSize.x > maxXSize
            || x < 0 && ObstacleSize.x < Constants.DefaultObstacleSize
            || y > 0 && ObstacleSize.y > maxYSize
            || y < 0 && ObstacleSize.y < Constants.DefaultObstacleSize
            || z > 0 && ObstacleSize.z > maxZSize
            || z < 0 && ObstacleSize.z < Constants.DefaultObstacleSize
            || (x == 0 && y == 0 && z == 0);
    }

    private Vector2Int CountGridSize(Vector3 size)
    {
        Vector2Int gridSize = new Vector2Int();

        gridSize.x = Mathf.CeilToInt((size.x) / Constants.GridCellSize);
        gridSize.y = Mathf.CeilToInt((size.y) / Constants.GridCellSize);
        return gridSize;
    }

}
