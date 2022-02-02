using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public Text t_tip;
    public Text t_blueTip;
    public Text t_coinCounter;
    public GameObject _pauseMenu;
    public GameObject _capturedMenu;

    public void ChangeCoins(int i_valor)
    {
        t_coinCounter.text = "x " + i_valor;
    }

    public void ChangeAndShowTip(string s_tipText)
    {
        t_tip.enabled = true;
        t_tip.text = s_tipText;
    }

    public void HideTip()
    {
        t_tip.enabled = false;
    }

    public void ChangeAndShowBlueTip(string s_tipText)
    {
        t_blueTip.enabled = true;
        t_blueTip.text = s_tipText;
    }

    public void HideBlueTip()
    {
        t_blueTip.enabled = false;
    }

    public void CallPauseMenu(bool b_valor)
    {
        _pauseMenu.active = b_valor;
;
    }

    public void CallCapturedMenu(bool b_valor)
    {
        _capturedMenu.active = b_valor;       
    }
}
