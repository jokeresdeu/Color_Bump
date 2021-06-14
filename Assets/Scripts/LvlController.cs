using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LvlController 
{
    private Transform _cameraTransform;
    private Player _player;
    private ServiceLocator _serviceLocator;
    public LvlGenerator LvlGenerator { get; private set; }
    private LvlData _currentLvlvData;

    private List<ObstacleContainer> _friendlyObstacles;
    private List<ObstacleContainer> _enemyObstacles;
    private List<ObstacleInfo> _friendlyObstaclesPerLvl;
    private List<ObstacleInfo> _enemyObstaclesPerLvl;

    private bool _obstaclesRemoved = false;
    private bool _obstaclesPlaced = false;
    public int CurrentLvl { get; private set; }
    public bool IsLvlStarted { get; private set; }
    public float LvlLength { get; private set; }
    public float LvlWidth { get; private set; }

    public Action LvlStarted = delegate { };
    public Action<bool> LvlEnded = delegate { };

    public LvlController(Player player, ServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
        _player = player;

        LvlLength = _serviceLocator.LvlObjects.EndPoint.position.y - _serviceLocator.LvlObjects.StartPoint.position.y;
        LvlWidth = _serviceLocator.LvlObjects.EndPoint.position.x - _serviceLocator.LvlObjects.StartPoint.position.x;

        LvlGenerator = new LvlGenerator(this);

        _friendlyObstacles = new List<ObstacleContainer>();
        _enemyObstacles = new List<ObstacleContainer>();
        _cameraTransform = Camera.main.transform;
    }

    public void LoadNextLvl()
    {

    }

    public void LoadLvl(int lvl)
    {
        _currentLvlvData = LvlGenerator.GetLvlData(lvl);
        PrepareLvl();
    }

    public void RestartLvl()
    {
        RemoveObstaclesTo(_serviceLocator.LvlObjects.EndPoint.position.y);
        PrepareLvl();
    }

    public void PrepareLvl()
    {
        Time.timeScale = 1f;
        _player.MeshRenderer.material = LvlGenerator.DefaultMaterial;
        _player.PlayerMoved += OnPlayerMoved;
        float endPos = _serviceLocator.LvlObjects.StartPoint.position.y + _serviceLocator.LvlObjects.RenderDistance; 
        _friendlyObstaclesPerLvl = _currentLvlvData.FriendlyObstacles;
        _enemyObstaclesPerLvl = _currentLvlvData.EnemyObstacles;
        PlaceObstaclesTo(endPos);
    }

    public IEnumerator SpawnObjects()
    {
        while (true)
        {
            _obstaclesRemoved = false;
            RemoveObstaclesTo(_cameraTransform.position.y);
            yield return new WaitUntil(() => _obstaclesRemoved);
            _obstaclesPlaced = false;
            PlaceObstaclesTo(_player.transform.position.y + _serviceLocator.LvlObjects.RenderDistance);
            yield return new WaitUntil(() => _obstaclesPlaced);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void PlaceObstaclesTo(float endPos)
    {
        PlaceObstaclesTo(endPos, _friendlyObstacles, _friendlyObstaclesPerLvl, LvlGenerator.DefaultMaterial);
        PlaceObstaclesTo(endPos, _enemyObstacles, _enemyObstaclesPerLvl, LvlGenerator.AlternativeMaterial);
        _obstaclesPlaced = true;
    }

    private void RemoveObstaclesTo(float toPos)
    {
        RemoveObstaclesTo(toPos, _friendlyObstacles);
        RemoveObstaclesTo(toPos, _enemyObstacles);
        _obstaclesRemoved = true;
    }


    private void PlaceObstaclesTo(float endPosition, List<ObstacleContainer> obstacles, List<ObstacleInfo> obstacleInfos, Material material)
    {
        while (obstacleInfos.Count != 0 && obstacleInfos[0].Position.y <= endPosition)
        {
            ObstacleContainer obstacle = _serviceLocator.ObstaclesPool.GetObstacle();
            obstacle.transform.SetParent(_serviceLocator.LvlObjects.ObstaclesGrid);
            obstacle.OnPutToScene(obstacleInfos[0], material);
            obstacles.Add(obstacle);
            obstacleInfos.RemoveAt(0);
        }
    }

    private void RemoveObstaclesTo(float endPos, List<ObstacleContainer> obstacles)
    {
        while (obstacles.Count != 0 && obstacles[0].transform.position.y < endPos)
        {
            ObstacleContainer obstacle = obstacles[0];
            obstacle.OnReturnToPool();
            _serviceLocator.ObstaclesPool.PutToThePool(obstacle);
            obstacles.RemoveAt(0);
        }
    }

    private void OnPlayerMoved()
    {
        LvlStarted();
        IsLvlStarted = true;
        LvlStarted();
        _player.PlayerMoved -= OnPlayerMoved;
        _player.PlayerEndedLvl += OnPlayerEndedLvl;
    }

    private void OnPlayerEndedLvl(bool successfully)
    {
         LvlEnded(successfully);
         Time.timeScale = 0.5f;
         IsLvlStarted = false;
        _player.PlayerEndedLvl -= OnPlayerEndedLvl;
    }

}
