using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LvlObjects _lvlObjects;
    [SerializeField] private ObstaclesPool _obstaclePool;

    //public LvlController LvlController { get; private set; }
    public ObstaclesPool ObstaclesPool => _obstaclePool;
    public LvlObjects LvlObjects => _lvlObjects;
    public LvlController LvlController { get; private set; }
    public bool GamePaused { get; private set; }

    #region Singleton
    public static ServiceLocator Instance; 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public event Action UpdateCalled = delegate { };
    public event Action FixedUpdateCalled = delegate { };
    public event Action LateUpdateCalled = delegate { };
    public event Action ServiceLocatorDestroyed = delegate { };

    void Start()
    {
        LvlController = new LvlController(_player, this);

        //LvlController.LoadLvl(1);
        LvlController.LvlStarted += OnLvlstarted;
        LvlController.LvlEnded += OnLvlEnded;
        GamePaused = false;
    }

    private void OnLvlstarted()
    {
        StartCoroutine(LvlController.SpawnObjects());
        LvlController.LvlStarted -= OnLvlstarted;
        LvlController.LvlEnded += OnLvlEnded;
    }

    private void OnLvlEnded(bool successfully)
    {
        StopCoroutine(LvlController.SpawnObjects());
        LvlController.LvlEnded -= OnLvlEnded;
        LvlController.LvlStarted += OnLvlstarted;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GamePaused)
            return;

        UpdateCalled();
    }

    private void FixedUpdate()
    {
        if (GamePaused)
            return;

        FixedUpdateCalled();
    }

    private void LateUpdate()
    {
        if (GamePaused)
            return;

        LateUpdateCalled();
    }

    private void OnDestroy()
    {
        ServiceLocatorDestroyed();
    }

    private void OnApplicationPause(bool pause)
    {
        GamePaused = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        GamePaused = false;
    }
}
