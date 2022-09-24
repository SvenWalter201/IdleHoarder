using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] SpriteRenderer uiImage;

    public void SetIcon(Sprite sprite)
    {
        uiImage.sprite = sprite;
    }

    public void SetAmount(int amount)
    {
        text.text = amount.ToString();
    }
}
