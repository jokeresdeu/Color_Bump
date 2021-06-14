using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextElement : MonoBehaviour
{
    [SerializeField] private UiElement _element;
    private TMP_Text _text;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        try
        {
            _text.text = GetComponentInParent<UiContainer>().Localization[_element];
        }
        catch
        {
            _text.text = "No localization";
        }
    }
}
