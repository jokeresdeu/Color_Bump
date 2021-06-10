using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LvlCreatorUI : MonoBehaviour
{
    [Header("Work objects properties")]
    [SerializeField] private Image _obstacleColor;
    [SerializeField] private TMP_Text _obstacleGridSizeX;
    [SerializeField] private TMP_Text _obstacleGridSizeY;
    [SerializeField] private TMP_Text _obstacleSizeX;
    [SerializeField] private TMP_Text _obstacleSizeY;
    private Button _changeObjectCollorButton;

    [Header("World Properies")]
    [SerializeField] private Image _defaultColor;
    [SerializeField] private Image _alternativeColor;
    [SerializeField] private TMP_Text _worldGridSizeX;
    [SerializeField] private TMP_Text _worldGridSizeY;
    [SerializeField] private TMP_Text _worldSizeX;
    [SerializeField] private TMP_Text _worldSizeY;
    [SerializeField] private TMP_Text _friendlyObstaclesCount;
    [SerializeField] private TMP_Text _enemyObstaclesCount;

    [Header("Creator Staf")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _clearLvlButton;
    [SerializeField] private Button _eraserButton;
    [SerializeField] private Transform _obstaclesGrid;
    [SerializeField] private ObstaclesGridButton _obstacleButtonPrefab;

    private LvlCreator _lvlCreator;
    // Start is called before the first frame update

    private void Update()
    {
        if (_lvlCreator == null)
            return;
        _friendlyObstaclesCount.text = _lvlCreator.FriendlyObstacles.Count.ToString();
        _enemyObstaclesCount.text = _lvlCreator.EnemyObstacles.Count.ToString();
    }
    private void OnDestroy()
    {
        if(_lvlCreator.WorkObstacle!=null)
            _lvlCreator.WorkObstacle.ObstaclePropertiesChanged -= OnWorkObstaclePropertiesChanged;

        _lvlCreator.AlternativeColorChanged -= OnAlternativeColorChanged;
        _lvlCreator.DefaultColorChanged -= OnDefaultColorChanged;

        _changeObjectCollorButton.onClick.RemoveListener(_lvlCreator.ChangeWorkObstacleMaterial);
        _eraserButton.onClick.RemoveListener(_lvlCreator.DestroyWorkObstacle);
        _saveButton.onClick.RemoveListener(_lvlCreator.OnSaveClicked);
        _clearLvlButton.onClick.RemoveListener(_lvlCreator.DestroyGeneratedObstacles);
    }

    public void InitUI(LvlCreator lvlCreator)
    {
        _changeObjectCollorButton = _obstacleColor.GetComponent<Button>();
        #region WorldPropertes
        _lvlCreator = lvlCreator;
        _worldGridSizeX.text = _lvlCreator.Grid.Width.ToString();
        _worldGridSizeY.text = _lvlCreator.Grid.Height.ToString();
        _worldSizeX.text = "5";
        _worldSizeY.text = _lvlCreator.LvlLength.ToString();
        _friendlyObstaclesCount.text = "0";
        _enemyObstaclesCount.text = "0";
        _defaultColor.color = _lvlCreator.DefaultColor;
        _alternativeColor.color = _lvlCreator.AlternativeColor;
        _lvlCreator.AlternativeColorChanged += OnAlternativeColorChanged;
        _lvlCreator.DefaultColorChanged += OnDefaultColorChanged;
       
        AddButtonsToGrid();
        #endregion

        _changeObjectCollorButton.onClick.AddListener(_lvlCreator.ChangeWorkObstacleMaterial);
        _lvlCreator.WorkObstacleCreated += OnWorkObstacleCreated;
        _eraserButton.onClick.AddListener(_lvlCreator.DestroyWorkObstacle);
        _saveButton.onClick.AddListener(_lvlCreator.OnSaveClicked);
        _clearLvlButton.onClick.AddListener(lvlCreator.DestroyGeneratedObstacles);
    }

    private void AddButtonsToGrid()
    {
        for(int i = 0; i < _lvlCreator.WorkObstacles.Obstacles.Count; i++ )
        {
            ObstaclesGridButton gridButton = Instantiate(_obstacleButtonPrefab, _obstaclesGrid);
            gridButton.Init(_lvlCreator, _lvlCreator.WorkObstacles.Obstacles[i], i);
        }
    }

    private void OnWorkObstacleCreated()
    {
        _obstacleGridSizeX.text = _lvlCreator.WorkObstacle.GridSize.x.ToString();
        _obstacleGridSizeY.text = _lvlCreator.WorkObstacle.GridSize.y.ToString();
        _obstacleSizeX.text = _lvlCreator.WorkObstacle.ObstacleSize.x.ToString();
        _obstacleSizeY.text = _lvlCreator.WorkObstacle.ObstacleSize.y.ToString();
        _obstacleColor.color = _lvlCreator.WorkObstacle.ObstacleMaterial.color;
        _lvlCreator.WorkObstacle.ObstaclePropertiesChanged += OnWorkObstaclePropertiesChanged;
        _lvlCreator.WorkObstacleDestroyed += OnWorkObstacleDestroyed;
        _lvlCreator.WorkObstacleCreated -= OnWorkObstacleCreated;
    }

    private void OnWorkObstacleDestroyed()
    {
        _lvlCreator.WorkObstacleCreated += OnWorkObstacleCreated;
        _lvlCreator.WorkObstacleDestroyed -= OnWorkObstacleDestroyed;
        _lvlCreator.WorkObstacle.ObstaclePropertiesChanged -= OnWorkObstaclePropertiesChanged;
    }

    private void OnAlternativeColorChanged(Color color)
    {
        _alternativeColor.color = _lvlCreator.AlternativeColor;

        if (_lvlCreator.WorkObstacle != null)
            _obstacleColor.color = _lvlCreator.WorkObstacle.ObstacleMaterial.color;
    }

    private void OnDefaultColorChanged(Color color)
    {
        _defaultColor.color = _lvlCreator.DefaultColor;

        if (_lvlCreator.WorkObstacle != null)
            _obstacleColor.color = _lvlCreator.WorkObstacle.ObstacleMaterial.color;
    }

    private void OnWorkObstaclePropertiesChanged()
    {
        _obstacleGridSizeX.text = _lvlCreator.WorkObstacle.GridSize.x.ToString();
        _obstacleGridSizeY.text = _lvlCreator.WorkObstacle.GridSize.y.ToString();
        _obstacleSizeX.text = _lvlCreator.WorkObstacle.ObstacleSize.x.ToString();
        _obstacleSizeY.text = _lvlCreator.WorkObstacle.ObstacleSize.y.ToString();
        _obstacleColor.color = _lvlCreator.WorkObstacle.ObstacleMaterial.color;
    }

   
}
