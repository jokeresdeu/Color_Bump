using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclesGridButton : MonoBehaviour
{
    private Image _image;
    private Button _button;
    public void Init(LvlCreator lvlCreator, WorkObstacle workObstacle, int index)
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        _image.sprite = workObstacle.HotbarImage;
        _button.onClick.AddListener(() => lvlCreator.SetWorkObstacle(workObstacle.ObstaclePrefab, index));
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
