using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChestRoomPanel : Panel
{
    [Header("ChestRoom")]
    [SerializeField] private GameObject _chestRoomPanel;
    [SerializeField] private TMP_Text _coinsAmount;
    [SerializeField] private TMP_Text _chooseYourChest;
    [SerializeField] private GameObject _chestGrid;
    [SerializeField] private GameObject _keysGrid;
    [SerializeField] private Button _noThanks;
    [SerializeField] private Sprite _spesialSprite;

    private Image[] _chestKeys;
    private ChestButton[] _chestButtons;
    private int _keysUsed;

    public override void Init(UiContainer container)
    {
        base.Init(container);
        _noThanks.onClick.AddListener(() =>
        {
            UiContainer.OpenLvlPanel();
            OnChestroomClosed();
        });

        _chestButtons = GetComponentsInChildren<ChestButton>();
        _chestKeys = _keysGrid.GetComponentsInChildren<Image>();
    }

    public void OnChestRoomOpened()
    {
        _noThanks.gameObject.SetActive(false);
        int luckyButton = Random.Range(0, _chestButtons.Length - 1);
        for (int i = 0; i < _chestButtons.Length; i++)
        {
            if (i == luckyButton)
            {
                int coinsAmount = Random.Range(5, 10) * 10;
                _chestButtons[i].Button.onClick.AddListener(() =>
                {
                    _chestButtons[i].OnCoinsButtonClicked(coinsAmount);
                    OnChestButtonClicked();
                });

                return;
            }

            _chestButtons[i].Button.onClick.AddListener(() =>
            {
                _chestButtons[i].OnSpesialButtonClicked(_spesialSprite);
                OnChestButtonClicked();
            });
        }
    }

    private void OnChestButtonClicked()
    {
        _keysUsed++;
        if(_keysUsed > _chestKeys.Length)
        {
            _noThanks.gameObject.SetActive(true);
        }

        for(int i = 0; i <= _keysUsed; i++)
        {
            _chestKeys[_chestKeys.Length - i].color = new Color(1, 1, 1, 0.5f);
        }
    }


    public void OnChestroomClosed()
    {
        _keysUsed = 0;
        for (int i = 0; i < _chestButtons.Length; i++)
        {
            _chestButtons[i].Reset();
        }
    }
}
