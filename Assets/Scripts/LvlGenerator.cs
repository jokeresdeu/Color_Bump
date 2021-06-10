using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class LvlGenerator
{
    private string _lvlsPath = "GeneratedLvls";
    private int[] _awailableLength = { 1, 5, 10 };

    private const int _minFriendlyLineLength = 4;

    private List<Vector3> _awailableObstacleSize;
    public Material DefaultMaterial { get; private set; }
    public Material AlternativeMaterial { get; private set; }
    public ObstacleContainer Obstacle { get; private set; }

    public ObstacleGrid ObstacleGrid { get; private set; }
   
    public Dictionary<ObstacleShape, ObstacleBody> ObstacleBodies { get; private set; }

    public  LvlGenerator(LvlController lvlController)
    {
        AlternativeMaterial = Resources.Load<Material>("Materials/AlternativeMaterial");
        DefaultMaterial = Resources.Load<Material>("Materials/DefaultMaterial"); 
        ObstacleBodies = new Dictionary<ObstacleShape, ObstacleBody>();
        ObstacleBody[] obstacles = Resources.LoadAll<ObstacleBody>("Obstacles");
        for(int i = 0; i < obstacles.Length; i++)
        {
            ObstacleBodies.Add(obstacles[i].Shape, obstacles[i]);
        }
        Obstacle = Resources.Load<ObstacleContainer>("Obstacle");
        CreatePosibleSizes();
        int gridHeight = Mathf.FloorToInt(lvlController.LvlLength / Constants.GridCellSize);
        int girdWidth = Mathf.FloorToInt(lvlController.LvlWidth / Constants.GridCellSize);
        ObstacleGrid = new ObstacleGrid(girdWidth, gridHeight, ServiceLocator.Instance.LvlObjects.StartPoint.position);
    }
    


    public LvlData GetLvlData(int currentLvl)
    {
        currentLvl = 2; //wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
        string path = Path.Combine(_lvlsPath, currentLvl.ToString());
        TextAsset text = Resources.Load<TextAsset>(path);
        if(text == null)
        {
            return RandomlyGenerateLvl();

        }
        return JsonUtility.FromJson<LvlData>(text.text);
    }

    public LvlData RandomlyGenerateLvl()
    {
        ObstacleGrid.ClearGrid();
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
        while (currentCreedCell < ObstacleGrid.Height)
        {
            int length = _awailableLength[Random.Range(0, _awailableLength.Length)];
            int friendlyObstaclesWidth = Mathf.RoundToInt(ObstacleGrid.Width * Random.Range(0.4f, 0.8f));
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

                friendlyLineStartPositions[i] = Random.Range(lastPosition, ObstacleGrid.Width - friendlyLineLengths[i] - remainingLengthCount * (_minFriendlyLineLength + 1));
                lastPosition = friendlyLineStartPositions[i] + friendlyLineLengths[i] + 1;
                awailableLineLentgh -= friendlyLineLengths[i];
            }

            obstacleSize = _awailableObstacleSize[Random.Range(0, _awailableObstacleSize.Count - 1)];

            for (int i = 0; i < friendlyLinesCount; i++)
            {
                CreateAndAddInfo(friendlyLineStartPositions[i], friendlyLineStartPositions[i] + friendlyLineLengths[i], currentCreedCell, currentCreedCell + length, ref lvlData.FriendlyObstacles, obstacleSize);
            }

            obstacleSize = _awailableObstacleSize[Random.Range(0, _awailableObstacleSize.Count - 1)];

            CreateAndAddInfo(0, ObstacleGrid.Width, currentCreedCell, currentCreedCell + length, ref lvlData.EnemyObstacles, obstacleSize); 
            CreateAndAddInfo(0, ObstacleGrid.Width, currentCreedCell, currentCreedCell + length, ref lvlData.EnemyObstacles, Vector3.one * Constants.DefaultObstacleSize); //fill in empty slots

            currentCreedCell += length + Random.Range(6, 12);
        }

        lvlData.EnemyObstacles.OrderBy(ob => ob.Position.y);
        lvlData.EnemyObstacles.OrderBy(ob => ob.Position.y);
        return lvlData;
    }

    private void CreatePosibleSizes()
    {
        _awailableObstacleSize = new List<Vector3>();
        AddToListWithProbability(new Vector3(0.25f, 0.25f, 0.25f), 8);
        AddToListWithProbability(new Vector3(0.25f, 0.25f, 0.5f), 5);
        AddToListWithProbability(new Vector3(0.25f, 0.25f, 1f), 5);

        AddToListWithProbability(new Vector3(0.5f, 0.25f, 0.25f), 5);
        AddToListWithProbability(new Vector3(0.5f, 0.25f, 0.5f), 5);
        AddToListWithProbability(new Vector3(0.5f, 0.25f, 1f), 5);

        AddToListWithProbability(new Vector3(0.5f, 0.5f, 0.25f), 2);
        AddToListWithProbability(new Vector3(0.5f, 0.5f, 0.5f), 2);
        AddToListWithProbability(new Vector3(0.5f, 0.5f, 1f), 2);
        AddToListWithProbability(new Vector3(0.5f, 1f, 1f), 1);

        AddToListWithProbability(new Vector3(0.5f, 1f, 0.25f), 1);
        AddToListWithProbability(new Vector3(0.5f, 1f, 0.5f), 2);
        AddToListWithProbability(new Vector3(0.5f, 1f, 1f), 3);

        AddToListWithProbability(new Vector3(1f, 0.25f, 0.25f), 2);
        AddToListWithProbability(new Vector3(1f, 0.25f, 0.5f), 3);
        AddToListWithProbability(new Vector3(1f, 0.25f, 1f), 4);

        AddToListWithProbability(new Vector3(1f, 0.5f, 0.25f), 2);
        AddToListWithProbability(new Vector3(1f, 0.5f, 0.5f), 2);
        AddToListWithProbability(new Vector3(1f, 0.5f, 1f), 2);

        AddToListWithProbability(new Vector3(1f, 1f, 0.25f), 1);
        AddToListWithProbability(new Vector3(1f, 1f, 0.5f), 1);
        AddToListWithProbability(new Vector3(1f, 1f, 1f), 1);
    }

    private void AddToListWithProbability(Vector3 size, int probability)
    {
        for (int i = 0; i < probability; i++)
        {
            _awailableObstacleSize.Add(size);
        }
    }

    private void CreateAndAddInfo(int iStart, int iEnd, int jStart, int jEnd, ref List<ObstacleInfo> infos, Vector3 size)
    {
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                if( ObstacleGrid.IfCanPlaceGetInfo(new Vector2Int(i, j), ObstacleGrid.CountGridSize(size), out Vector3 position, out List<Vector2Int> occupiedCells))
                {
                    ObstacleInfo info = new ObstacleInfo();
                    info.Size = ObstacleGrid.CorrectSize(size);
                    info.Position = position;
                    info.Shape = ObstacleShape.Cube;
                    infos.Add(info);
                    ObstacleGrid.SetValue(new ObstacleDummy(occupiedCells));
                }
            }
        }
    }


}
