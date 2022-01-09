using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    [SerializeField] 
    private string music;
    [SerializeField]
    private string[] loopSongs;


    // Use this for initialization
    void Start () {
        SoundController.playMusic(music);
        SoundController.stopLoopSonds();
        foreach(string name in loopSongs)
            SoundController.playSongLowLoop(name);

    }

    public void changeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void OpenURL(string URL)
    {
        Application.OpenURL(URL);
    }

    public void resetPrefabs()
    {
        PlayerPrefs.DeleteAll();
    }
}
