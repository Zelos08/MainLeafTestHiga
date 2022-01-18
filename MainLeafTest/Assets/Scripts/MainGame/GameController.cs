using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static bool b_isPaused;
    public UiController _uiController;
    public static GameController _instance { get; private set; }

    private void Awake()
    {
        b_isPaused = false;

        Cursor.visible = b_isPaused;

        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }  
        
    }
    
    public static bool GetPaused()
    {
        return b_isPaused;
    }

    public static void ChangePouse()
    {
        b_isPaused = !b_isPaused;
        Cursor.visible = b_isPaused;
    }

    public static void LoadScene(string s_sceneName)
    {
        SceneManager.LoadScene(s_sceneName, LoadSceneMode.Single);
    }

    public static void ResetGame()
    {
        SceneManager.LoadScene("Map1", LoadSceneMode.Single);
        PlayerPrefs.DeleteAll();
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void ChangeTip(string s_tipText)
    {
        _instance._uiController.ChangeAndShowTip(s_tipText);
    }

    public static void HideTip()
    {
        _instance._uiController.HideTip();
    }

    public static void ChangeCoins(int i_valor)
    {
        _instance._uiController.ChangeCoins(i_valor);
    }
}
