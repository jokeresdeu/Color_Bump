using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

[Serializable]
public class LvlCreator : MonoBehaviour
{
    private const string SerializationPath = "Assets/Resources/GeneratedLvls";

    [SerializeField] private LvlCreatorUI _creatorUI;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _worldObjecs;
    [SerializeField] private WorkObstacles _workObstacles;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _alternativeMaterial;

    private Vector2Int _prevCell;
    private Transform _startPoint;
    private Camera _camera;

    private int _currentMode;

    public WorkObstacles WorkObstacles => _workObstacles;

    public float LvlLength { get; private set; }
    public float LvlWidth { get; private set; }
    public ObstacleGrid Grid { get; private set; }
    public PlacedObstacle WorkObstacle { get; private set; }
    public List<PlacedObstacle> EnemyObstacles { get; private set; }
    public List<PlacedObstacle> FriendlyObstacles { get; private set; }

    public Color DefaultColor
    {
        get
        {
            return _defaultMaterial.color;
        }
        set
        {
            _defaultMaterial.color = value;
            DefaultColorChanged(value);
        }
    }

    public Color AlternativeColor
    {
        get
        {
            return _alternativeMaterial.color;
        }
        set
        {
            _alternativeMaterial.color = value;
            AlternativeColorChanged(value);
        }
    }

    public event Action<Color> DefaultColorChanged = delegate { };
    public event Action<Color> AlternativeColorChanged = delegate { };
    public event Action WorkObstacleCreated = delegate { };
    public event Action WorkObstacleDestroyed = delegate { };

    #region UnityMethods
    private void Start()
    {
        FriendlyObstacles = new List<PlacedObstacle>();
        EnemyObstacles = new List<PlacedObstacle>();
        _defaultMaterial = Instantiate(_defaultMaterial);
        _alternativeMaterial = Instantiate(_alternativeMaterial);
        DefaultColor = GetNewColor(Constants.MaterialColors[Random.Range(0, Constants.MaterialColors.Length)]);
        do
        {
            AlternativeColor = GetNewColor(Constants.MaterialColors[Random.Range(0, Constants.MaterialColors.Length)]);
        }
        while (DefaultColor == AlternativeColor);

        _camera = Camera.main;
        _startPoint = transform;
        LvlLength = _endPoint.position.y - _startPoint.position.y;
        LvlWidth = _endPoint.position.x - _startPoint.position.x;
        GenerateGrid();

        _creatorUI.InitUI(this);
    }

    private void OnDrawGizmos()
    {
        if (Grid == null)
            return;
        Grid.DrawGrid();
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            TryToPlaceObstacle(_camera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 currPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Grid.CanGetSellByPos(currPos, out Vector2Int currCell);
            if (currCell != _prevCell)
            {
                _prevCell = currCell;
                TryToPlaceObstacle(currPos);
            }
        }

        if (WorkObstacle == null)
            return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(WorkObstacle.transform.localScale.x/2, WorkObstacle.transform.localScale.y/2, 0);
        pos.z = 0;

        WorkObstacle.transform.position = pos;

        if (Input.GetKeyDown(KeyCode.R))
        {
            WorkObstacle.ResetSize();
            return;
        }
    }

    private void FixedUpdate()
    {
        if (WorkObstacle == null)
            return;

        if (Input.GetKey(KeyCode.X))
        {
            WorkObstacle.ChangeSize(1, 1, 1);
            return;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            WorkObstacle.ChangeSize(-1, -1, -1);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            WorkObstacle.ChangeSize(x: 1);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            WorkObstacle.ChangeSize(x: -1);
        }

        if (Input.GetKey(KeyCode.W))
        {
            WorkObstacle.ChangeSize(y: 1);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            WorkObstacle.ChangeSize(y: -1);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            WorkObstacle.ChangeSize(z: -1);

        }
        else if (Input.GetKey(KeyCode.E))
        {
            WorkObstacle.ChangeSize(z: 1);
        }
    }
    #endregion

    public void DestroyWorkObstacle()
    {
        WorkObstacleDestroyed();
        Destroy(WorkObstacle.gameObject);
        WorkObstacle = null;
    }

    public void DestroyGeneratedObstacles()
    {
        for (int i = 0; i < EnemyObstacles.Count; i++)
            Destroy(EnemyObstacles[i].gameObject);
        for (int i = 0; i < FriendlyObstacles.Count; i++)
            Destroy(FriendlyObstacles[i].gameObject);
        EnemyObstacles.Clear();
        FriendlyObstacles.Clear();
    }

    public void OnSaveClicked()
    {
        LvlData lvlData = new LvlData();
        lvlData.DefaultColor = GetNewColor(DefaultColor);
        lvlData.AlternativeColor = GetNewColor(AlternativeColor);
        lvlData.EnemyObstacles = new List<ObstacleInfo>();
        EnemyObstacles = EnemyObstacles.OrderBy(obstacle => obstacle.transform.position.y).ToList();
        FriendlyObstacles = FriendlyObstacles.OrderBy(obstacle => obstacle.transform.position.y).ToList();
        for (int i =0; i < EnemyObstacles.Count; i++)
        {
            ObstacleInfo info = new ObstacleInfo();
            info.Position = EnemyObstacles[i].transform.position;
            info.Size = EnemyObstacles[i].ObstacleSize;
            info.Shape = EnemyObstacles[i].Shape;
            lvlData.EnemyObstacles.Add(info);
        }

        lvlData.FriendlyObstacles = new List<ObstacleInfo>();
        for (int i = 0; i < FriendlyObstacles.Count; i++)
        {
            ObstacleInfo info = new ObstacleInfo();
            info.Position = FriendlyObstacles[i].transform.position;
            info.Size = FriendlyObstacles[i].ObstacleSize;
            info.Shape = FriendlyObstacles[i].Shape;
            lvlData.FriendlyObstacles.Add(info);
        }

        if (!PlayerPrefs.HasKey(Constants.LvlsCreated))
            PlayerPrefs.SetInt(Constants.LvlsCreated, 0);

        int lvlsGenerated = PlayerPrefs.GetInt(Constants.LvlsCreated);
        lvlsGenerated++;
        
        string path = Path.Combine(SerializationPath, lvlsGenerated.ToString() + ".json");
        string json = JsonUtility.ToJson(lvlData);
        File.WriteAllText(path, json);
        PlayerPrefs.SetInt(Constants.LvlsCreated, lvlsGenerated);
    }

    public void SetWorkObstacle(PlacedObstacle obstacle, int i)
    {
        if (WorkObstacle != null)
        {
            DestroyWorkObstacle();
        }
        _currentMode = i;
        WorkObstacle = Instantiate(obstacle, _worldObjecs);
        WorkObstacle.Init(_defaultMaterial);
        WorkObstacleCreated();
    }

    public void ChangeWorkObstacleMaterial()
    {
        WorkObstacle.ObstacleMaterial = WorkObstacle.ObstacleMaterial == _defaultMaterial ? _alternativeMaterial : _defaultMaterial;
    }

    private void GenerateGrid()
    {
        int gridHeight = Mathf.FloorToInt(LvlLength / Constants.GridCellSize);
        int gridWidth = Mathf.FloorToInt(LvlWidth / Constants.GridCellSize);
        Grid = new ObstacleGrid(gridWidth, gridHeight, _startPoint.position);
    }

    private void TryToPlaceObstacle(Vector3 posToPlace)
    {
        if(WorkObstacle == null)
        {
            PlacedObstacle obstacle = Grid.GetValueAtPos(posToPlace) as PlacedObstacle;
            if(obstacle != null)
            {
                if (obstacle.ObstacleMaterial == _defaultMaterial)
                    FriendlyObstacles.Remove(obstacle);
                else
                    EnemyObstacles.Remove(obstacle);
                Destroy(obstacle.gameObject);
            }
                
            return;
        }
        if (Grid.IfCanPlaceGetInfo(posToPlace, WorkObstacle.GridSize, out Vector3 position, out List<Vector2Int> occupiedCells))
        {
            PlacedObstacle obstacle = Instantiate(_workObstacles.Obstacles[_currentMode].ObstaclePrefab, position, Quaternion.identity, _worldObjecs);
            obstacle.Init(WorkObstacle.ObstacleMaterial, WorkObstacle.ObstacleSize, occupiedCells, Grid);
            if (obstacle.ObstacleMaterial == _defaultMaterial)
                FriendlyObstacles.Add(obstacle);
            else
                EnemyObstacles.Add(obstacle);
            Grid.SetValue(obstacle);
        }
    }

    private Color GetNewColor(Color color)
    {
        return new Color(color.r, color.g, color.b, 1);
    }
}
