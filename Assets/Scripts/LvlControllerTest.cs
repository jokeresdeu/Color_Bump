using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LvlControllerTest : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Transform _cameraTransform;
    [SerializeField] private ServiceLocator _serviceLocator;
    public PlacedObstacle Obstacle;
    private LvlGenerator _lvlGenerator;
    private LvlData _currentLvlvData;
    private List<ObstacleContainer> _friendlyObstacles;
    private List<ObstacleContainer> _enemyObstacles;
    private List<ObstacleInfo> _friendlyObstaclesPerLvl;
    private List<ObstacleInfo> _enemyObstaclesPerLvl;

    public bool LvlStarted { get; private set; }
    public float LvlLength { get; private set; }

    

    //public Action PlayerMaterialChanged = delegate { };
    public void Start()
    {
        _player.Init();
        LvlLength = _serviceLocator.LvlObjects.EndPoint.position.y - _serviceLocator.LvlObjects.StartPoint.position.y;
       
       
        //_lvlGenerator = new LvlGenerator(girdWidth, gridHeight);
        //_currentLvlvData = _lvlGenerator.GenerateLvl(1);
        _friendlyObstacles = new List<ObstacleContainer>();
        _enemyObstacles = new List<ObstacleContainer>();
        _cameraTransform = Camera.main.transform;

        _serviceLocator.ServiceLocatorDestroyed += OnDestroy;
        PrepareLvl();
        StartCoroutine(SpawnObjects());
       
    }
    private bool _obstaclesRemoved = false;
    private bool _obstaclesPlaced = false;

    private IEnumerator SpawnObjects()
    {
        while(!LvlStarted)
        {
            yield return new WaitForSeconds(0.02f);
        }
        while(true)
        {
            _obstaclesRemoved = false;
            RemoveObstaclesTo(_cameraTransform.position.y);
            yield return new WaitUntil(() => _obstaclesRemoved);
            _obstaclesPlaced = false;
            PlaceObstaclesTo(_player.transform.position.y + 40);
            yield return new WaitUntil(() => _obstaclesPlaced);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnDestroy()
    {
        _serviceLocator.ServiceLocatorDestroyed -= OnDestroy;
    }


    public void RestartLvl()
    {
        PrepareLvl();
        RemoveObstaclesTo(_serviceLocator.LvlObjects.EndPoint.position.y);
    }

    private void RemoveObstaclesTo(float position)
    {
        RemoveObstaclesTo(position, _friendlyObstacles);
        RemoveObstaclesTo(position, _enemyObstacles);
        _obstaclesRemoved = true;
    }
  
    public void PrepareLvl()
    {
        _player.MeshRenderer.material = _lvlGenerator.DefaultMaterial;
        _player.PlayerMoved += OnPlayerMoved;
        _friendlyObstaclesPerLvl = _currentLvlvData.FriendlyObstacles;
        _enemyObstaclesPerLvl = _currentLvlvData.EnemyObstacles;
        PlaceObstaclesTo(_serviceLocator.LvlObjects.StartPoint.position.y + 20);
    }

    public void PlaceObstaclesTo(float endPoint)
    {
        PlaceObstaclesTo(endPoint, _friendlyObstacles, _friendlyObstaclesPerLvl, _lvlGenerator.DefaultMaterial);
        PlaceObstaclesTo(endPoint, _enemyObstacles, _enemyObstaclesPerLvl, _lvlGenerator.AlternativeMaterial);
        _obstaclesPlaced = true;

    }

    private void PlaceObstaclesTo(float endPosition, List<ObstacleContainer> obstacles, List<ObstacleInfo> obstacleInfos, Material material)
    {
        while (obstacleInfos.Count != 0 && obstacleInfos[0].Position.y <= endPosition)
        {
            try
            {
                ObstacleContainer obstacle = _serviceLocator.ObstaclesPool.GetObstacle();
                obstacle.transform.SetParent(_serviceLocator.LvlObjects.ObstaclesGrid);
                obstacle.OnPutToScene(obstacleInfos[0], material);
                obstacles.Add(obstacle);
                obstacleInfos.RemoveAt(0);
            }
            catch
            {
                Debug.LogError("Wtf");
            }
            
        }
    }


    private void RemoveObstaclesTo(float endPos, List<ObstacleContainer> obstacles)
    {
        while (obstacles.Count != 0 && obstacles[0].CurrentPosY < endPos)
        {
            ObstacleContainer obstacle = obstacles[0];
            obstacle.OnReturnToPool();
            _serviceLocator.ObstaclesPool.PutToThePool(obstacle);
            obstacles.RemoveAt(0);
        }
    }

    private void OnPlayerMoved()
    {
        LvlStarted = true;
        _player.PlayerMoved -= OnPlayerMoved;
        //_player.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        LvlStarted = false;
        //_player.PlayerDied -= OnPlayerDied;
        PrepareLvl();
    }

    //public void OnPlayerMaterialChange()
    //{
    //    PlayerMaterialChanged();
    //}
}
public interface ITest
{

}
