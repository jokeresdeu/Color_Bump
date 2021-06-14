using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LvlPanel : Panel
{
    [SerializeField] private GameObject _mainPanel;

    [Header("Start lvl Panel")]
    [SerializeField] private GameObject _startLvlPanel;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _noAdsButton;
    [SerializeField] private Button _settingsButton;

    [Header("Settings Pannel")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Toggle _vibration;
    [SerializeField] private Toggle _sound;

    [Header("Restart lvl panel")]
    [SerializeField] private Button _restartLvlPanel;
    [SerializeField] private TMP_Text _lvlText;

    [Header("NextLvlPanel")]
    [SerializeField] private GameObject _endLvlPanel;
    [SerializeField] private Button _actionButton;

    [Header("TopUI")]
    [SerializeField] private TMP_Text _coinsAmount;
    [SerializeField] private GameObject _keysContainer;
    [SerializeField] private Slider _lvlProgres;
    [SerializeField] private TMP_Text _currentLvl;
    [SerializeField] private TMP_Text _nextLvl;

    private Image[] _keyImages;
    public override void Init(UiContainer container)
    {
        base.Init(container);
        _settingsButton.onClick.AddListener(() => _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy));
        _noAdsButton.onClick.AddListener(() => Debug.LogError("NoAds"));
        _shopButton.onClick.AddListener(() => Debug.LogError("OpenCloseShopPanel"));
        _restartLvlPanel.onClick.AddListener(OnRestartClicked);
    }

    public void LoadLvl()
    {
        _currentLvl.text = ServiceLocator.LvlController.CurrentLvl.ToString();
        _nextLvl.text = (ServiceLocator.LvlController.CurrentLvl + 1).ToString();
        _lvlProgres.maxValue = ServiceLocator.LvlController.LvlLength;
        _coinsAmount.text = PlayerPrefs.GetInt("CoinsAmount").ToString();
        for (int i = 0; i < _keyImages.Length; i++)
        {
            if (i <= PlayerPrefs.GetInt("KeysAmount"))
                _keyImages[i].color = Color.white;
            else
                _keyImages[i].color = new Color(1, 1, 1, 0.5f);
        }

        _startLvlPanel.SetActive(true);

        ServiceLocator.LvlController.LvlStarted += OnLvlStarted;
    }

    public void OnLvlStarted()
    {
        _startLvlPanel.SetActive(false);
        ServiceLocator.LvlController.LvlStarted -= OnLvlStarted;
        ServiceLocator.LvlController.LvlEnded += OnLvlEnded;
    }

    public void OnLvlEnded(bool sucsessfully)
    {
        ServiceLocator.LvlController.LvlEnded -= OnLvlEnded;
        if (sucsessfully)
            OnLvlEndedSucsessfully();
        else
            OnPlayerDied();
    }

    private void OnLvlEndedSucsessfully()
    {
        _endLvlPanel.gameObject.SetActive(true);
        if (PlayerPrefs.GetInt("KeysAmount") < 3)
            _actionButton.onClick.AddListener(OnNextLvlClicked);
        else
            _actionButton.onClick.AddListener(UiContainer.OpenChestPanel);
    }

    private void OnNextLvlClicked()
    {
        ServiceLocator.LvlController.LoadNextLvl();
        LoadLvl();
        _actionButton.onClick.RemoveListener(OnNextLvlClicked);
    }


    private void OnPlayerDied()
    {
        _restartLvlPanel.gameObject.SetActive(true);
    }

    private void OnRestartClicked()
    {
        ServiceLocator.LvlController.RestartLvl();
        LoadLvl();

    }
}
