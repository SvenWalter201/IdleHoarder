using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;
      
    public void SetAmount(int amount)
    {
        text.text = amount.ToString();
    }
}
