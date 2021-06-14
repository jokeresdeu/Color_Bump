using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UiContainer : MonoBehaviour
{
    [SerializeField] private ChestRoomPanel _chestRoomPanel;
    [SerializeField] private LvlPanel _lvlPanel;

    public Dictionary<UiElement, string> Localization { get; private set; }

    private Languages _currentLanguage;
    private readonly string _localizationPath = "Localization/";

    private void Awake()
    {
        _currentLanguage = Languages.English; // Posibility yo be choozen
        UiLocalization _localization = Resources.Load<UiLocalization>(_localizationPath + _currentLanguage.ToString());
        //Localization = _localization.UiDatas.ToDictionary(l => ke) to Dictionary
        _lvlPanel.Init(this);
        _chestRoomPanel.Init(this);
        
        if (ProjectPrefs.KeysAmount.GetValue() < 3)
            OpenLvlPanel();
        else
            OpenChestPanel();
    }

    public void OpenChestPanel()
    {
        _lvlPanel.gameObject.SetActive(false);
        _chestRoomPanel.gameObject.SetActive(true);
    }

    public void OpenLvlPanel()
    {
        _chestRoomPanel.gameObject.SetActive(false);
        _lvlPanel.gameObject.SetActive(true);
        _lvlPanel.LoadLvl();
    }
}
