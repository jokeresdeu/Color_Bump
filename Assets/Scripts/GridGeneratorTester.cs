using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridGeneratorTester : MonoBehaviour
{
    [SerializeField] private PlacedObstacle _preffab;
    public Transform StartPoint;
    public Transform EndPoint;
    public Material DefaultMaterial;
    public Material Alternative;
    private int[] _awailableLength = { 1, 5, 8, 10, 12 };
    private Vector3[] _awailableObstacleSize =
    {
        new Vector3(0.25f, 0.25f, 0.25f),
        new Vector3(0.25f, 0.25f, 0.25f),
        new Vector3(0.25f, 0.25f, 0.25f),
        new Vector3(0.25f, 0.25f, 0.25f),
        new Vector3(0.25f, 0.25f, 0.25f),
        new Vector3(0.25f, 0.25f, 0.25f),
        new Vector3(0.25f, 0.25f, 0.5f),
        new Vector3(0.25f, 0.25f, 1f),
        new Vector3(0.25f, 0.5f, 0.25f),
        new Vector3(0.25f, 0.5f, 1f),
        new Vector3(0.25f, 1f, 0.25f),
        new Vector3(0.25f,1f, 0.5f),
        new Vector3(0.25f, 1f, 1f),
        new Vector3(0.5f, 0.25f, 0.25f),
        new Vector3(0.5f, 0.25f, 0.25f),
        new Vector3(0.5f, 0.25f, 0.25f),
        new Vector3(0.5f, 0.25f, 0.25f),
        new Vector3(0.5f, 0.25f, 0.5f),
        new Vector3(0.5f, 0.25f, 0.5f),
        new Vector3(0.5f, 0.25f, 0.5f),
        new Vector3(0.5f, 0.25f, 1f),
        new Vector3(1f, 0.25f, 0.25f),
        new Vector3(1f, 0.25f, 0.5f),
        new Vector3(1f, 0.25f, 1f),
    };

    private const int _minFriendlyLineLength = 4;
    private int Height = 320;
    private int Width = 20;

    private ObstacleGrid Grid;

    // Start is called before the first frame update
    void Start()
    {
        int gridHeight = Mathf.FloorToInt((EndPoint.position.y - StartPoint.position.y) / Constants.GridCellSize);
        int gridWidth = Mathf.FloorToInt((EndPoint.position.y - StartPoint.position.y) / Constants.GridCellSize);
        Grid = new ObstacleGrid(gridWidth, gridHeight, StartPoint.position);
        Generate();
    }

    private void OnDrawGizmos()
    {
        if (Grid == null)
            return;

        Grid.DrawGrid();
    }

    public LvlData Generate()
    {
        LvlData lvlData = new LvlData();
        lvlData.DefaultColor = Constants.MaterialColors[Random.Range(0, Constants.MaterialColors.Length)];
        do
        {
            lvlData.AlternativeColor = Constants.MaterialColors[Random.Range(0, Constants.MaterialColors.Length)];
        }
        while (lvlData.DefaultColor == lvlData.AlternativeColor);

        lvlData.FriendlyObstacles = new List<ObstacleInfo>();
        lvlData.EnemyObstacles = new List<ObstacleInfo>();

        Vector3 obstacleSize;
        int currentCreedCell = 0;
        while (currentCreedCell < Height)
        {
            int length = _awailableLength[Random.Range(0, _awailableLength.Length)];
            int friendlyObstaclesWidth = Mathf.RoundToInt(Grid.Width * Random.Range(0.4f, 0.8f));
            int maxLinesCount = friendlyObstaclesWidth / _minFriendlyLineLength;
            int friendlyLinesCount = Random.Range(1, maxLinesCount);

            int[] friendlyLineLengths = new int[friendlyLinesCount];
            int[] friendlyLineStartPositions = new int[friendlyLinesCount];
            int awailableLineLentgh = friendlyObstaclesWidth;
            int lastPosition = 0;
            for (int i = 0; i < friendlyLinesCount; i++)
            {
                int remainingLengthCount = friendlyLinesCount - (i + 1);
                if (remainingLengthCount == 0)
                {
                    friendlyLineLengths[i] = awailableLineLentgh;
                }
                else
                {
                    friendlyLineLengths[i] = Random.Range(_minFriendlyLineLength, awailableLineLentgh - _minFriendlyLineLength * remainingLengthCount);
                }

                friendlyLineStartPositions[i] = Random.Range(lastPosition, Width - friendlyLineLengths[i] - remainingLengthCount * (_minFriendlyLineLength + 1));
                lastPosition = friendlyLineStartPositions[i] + friendlyLineLengths[i] + 1;
                awailableLineLentgh -= friendlyLineLengths[i];
            }

            obstacleSize = _awailableObstacleSize[Random.Range(0, _awailableObstacleSize.Length - 1)];

            for (int i = 0; i < friendlyLinesCount; i++)
            {
                CreateAndAddInfo(friendlyLineStartPositions[i], friendlyLineStartPositions[i] + friendlyLineLengths[i], currentCreedCell, currentCreedCell + length, lvlData.FriendlyObstacles, obstacleSize);
            }

            obstacleSize = _awailableObstacleSize[Random.Range(0, _awailableObstacleSize.Length - 1)];

            CreateAndAddInfo(0, Width, currentCreedCell, currentCreedCell + length, lvlData.EnemyObstacles, obstacleSize);

            currentCreedCell += length + Random.Range(4, 8);
        }
        return lvlData;
    }

    private void CreateAndAddInfo(int iStart, int iEnd, int jStart, int jEnd, List<ObstacleInfo> infos, Vector3 size)
    {
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                bool canPlace;
                Vector3 position;
                List<Vector2Int> occupiedCells;
                canPlace = Grid.IfCanPlaceGetInfo(new Vector2Int(i, j), Grid.CountGridSize(size), out position, out occupiedCells);

                if (!canPlace)
                {
                    size = Vector3.one * Constants.GridCellSize;
                    canPlace = Grid.IfCanPlaceGetInfo(new Vector2Int(i, j), Grid.CountGridSize(size), out position, out occupiedCells);
                }
                if (canPlace)
                {
                    //ObstacleInfo info = new ObstacleInfo();
                    //info.Size = size;
                    //info.Position = position;
                    //info.Shape = ObstacleShape.Cube;
                    //infos.Add(info);
                    //Grid.SetValue(new ObstacleDummy(occupiedCells));
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
