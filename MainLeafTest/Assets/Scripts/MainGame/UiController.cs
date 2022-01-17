using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public Text Tip;
    public Text CoinCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
