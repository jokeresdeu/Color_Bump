using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChestButton : MonoBehaviour
{
    [SerializeField] private Button _chestButton;
    [SerializeField] private TMP_Text _coinsAmount;
    [SerializeField] private Image _spesialIamge;

    public Button Button => _chestButton;
    public void OnCoinsButtonClicked(int coinsAmount)
    {
        OnChestButtonClicked();
        _coinsAmount.text = coinsAmount.ToString();
        ProjectPrefs.CoinsAmount.AddValue(coinsAmount);
       
    }

    public void OnSpesialButtonClicked(Sprite sprite)
    {
        OnChestButtonClicked();
        _spesialIamge.gameObject.SetActive(true);
        _spesialIamge.sprite = sprite;
    }

    public void Reset()
    {
        _chestButton.onClick.RemoveAllListeners();
        _chestButton.gameObject.SetActive(true);
        _spesialIamge.gameObject.SetActive(false);
    }

    private void OnChestButtonClicked()
    {
        _chestButton.gameObject.SetActive(false);
        ProjectPrefs.KeysAmount.AddValue(-1);
    }
}
