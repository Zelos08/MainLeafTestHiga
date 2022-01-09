using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMenuController : MonoBehaviour {

    public Button audioButton;
    public Button musicButton;
    public Scrollbar musicBar;
    public Scrollbar audioBar;
    [SerializeField]
    private string clickSong;

    private void Start()
    {
        audioButton.GetComponent<ButtonOnOff>().change(SoundController.GetMuteAudio());    
        musicButton.GetComponent<ButtonOnOff>().change(SoundController.GetMuteMusic());
        musicBar.value = SoundController.getMusicVol();
        audioBar.value = SoundController.getAudioVol();
    }

    public void changeAudioVol(float vol)
    {
        SoundController.changeAudioVol(vol);
    }

    public void changeMusicVol(float vol)
    {
        SoundController.changeMusicVol(vol);
    }

    public void muteMusic()
    {
        SoundController.playSong(clickSong);
        SoundController.MuteMusic();
    }

    public void muteAudio()
    {
        SoundController.playSong(clickSong);
        SoundController.MuteAudio();
    }
}
