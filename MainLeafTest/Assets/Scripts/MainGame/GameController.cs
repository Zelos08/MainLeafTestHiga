using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static int i_coinCounter;
    public UiController _uiController;


    private void Awake()
    {
        i_coinCounter = PlayerPrefs.GetInt("Coins");
    }
    
    public void changeCoins(int valor)
    {
        i_coinCounter += valor;

    }
}
