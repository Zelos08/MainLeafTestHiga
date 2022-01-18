using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public Text Tip;
    public Text CoinCounter;

    public void ChangeCoins(int valor)
    {
        CoinCounter.text = "x " + valor;
    }

    public void ChangeAndShowTip(string tipText)
    {
        Tip.enabled = true;
        Tip.text = tipText;
    }

    public void HideTip()
    {
        Tip.enabled = false;
    }
}
