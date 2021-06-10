using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesPool : MonoBehaviour
{
    [SerializeField] private ObstacleContainer _obstacle;
    private List<ObstacleContainer> _obstaclesInThePool = new List<ObstacleContainer>();

    public ObstacleContainer GetObstacle()
    {
        ObstacleContainer obstacle;
        if (_obstaclesInThePool.Count == 0)
        {
            obstacle = Instantiate(_obstacle, transform);
            obstacle.Init();
        }
        else
        {
            obstacle = _obstaclesInThePool[0];
            obstacle.gameObject.SetActive(true);
            _obstaclesInThePool.RemoveAt(0);
        }
        return obstacle;
    }

    public void PutToThePool(ObstacleContainer obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetParent(transform);
        obstacle.transform.position = transform.position;
        _obstaclesInThePool.Add(obstacle);
    }
}
