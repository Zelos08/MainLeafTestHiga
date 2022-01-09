using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SoundController : MonoBehaviour{

    public static SoundController Instance{ get; private set; }
    private static AudioSource music;
    private static List<AudioSource> loops;
    private static float audioVolume,musicVolume;
    private static bool muteAudio, muteMusic;
    private static string atualMusic;

    private void Awake()
    {       
        if (Instance == null)
        {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadSondPrefs();
            loops = new List<AudioSource>();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void stopLoopSonds()
    {
        foreach(AudioSource loop in loops)
            Destroy(loop);

        loops.Clear();
    }

    public static void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }

    public static bool GetBool(string key)
    {
        int value = PlayerPrefs.GetInt(key,0);

        if (value == 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public static void loadSondPrefs()
    {
        muteAudio = SoundController.GetBool("muteAudio");
        muteMusic = SoundController.GetBool("muteMusic");
        audioVolume = PlayerPrefs.GetFloat("audioVolume", 0.7f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.7f);

    }

    public static void saveSondPrefs()
    {
        SoundController.SetBool("muteMusic", muteMusic);
        SoundController.SetBool("muteAudio", muteAudio);
        PlayerPrefs.SetFloat("audioVolume", audioVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
	// Update is called once per frame
	void Update () {
       
    }

    public static void playSongLoop(string name)
    {
        if (!muteAudio)
        {
            AudioSource audioSource = Instance.gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load("Sounds/SFX/" + name, typeof(AudioClip)) as AudioClip;
            audioSource.volume = audioVolume;
            audioSource.loop = true;
            audioSource.Play();
            loops.Add(audioSource); 
        }
    }

    public static void playSongLowLoop(string name)
    {
        if (!muteAudio)
        {
            AudioSource audioSource = Instance.gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load("Sounds/SFX/" + name, typeof(AudioClip)) as AudioClip;
            audioSource.volume = audioVolume;
            audioSource.loop = true;
            audioSource.Play();
            loops.Add(audioSource);
        }
    }

    public static void playSong(string name)
    {
        if (!muteAudio)
        {
            AudioSource audioSource = Instance.gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load("Sounds/SFX/" + name,typeof(AudioClip)) as AudioClip;
            audioSource.volume = audioVolume;
            Destroy(audioSource, audioSource.clip.length);
            audioSource.Play();
        }
    }

    public static void playSong(string name, float time)
    {
        if (!muteAudio)
        {
            AudioSource audioSource = Instance.gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load("Sounds/SFX/" + name, typeof(AudioClip)) as AudioClip;
            audioSource.volume = audioVolume;
            time = (time / 1000) * 60;
            Destroy(audioSource, time);
            audioSource.Play();
        }
    }

    public static void playMusic (string name)
    {

        if (music == null)
        {
            music = Instance.gameObject.AddComponent<AudioSource>();
            music.clip = Resources.Load("Sounds/BGM/" + name, typeof(AudioClip)) as AudioClip;
            music.loop = true;
            music.volume = musicVolume;
            music.mute = muteMusic;
            music.Play();
            atualMusic = name;
        }
        else
        {
            if (atualMusic != name)
            {
                music.clip = Resources.Load("Sounds/BGM/" + name, typeof(AudioClip)) as AudioClip;
                music.Play();
                atualMusic = name;
            }
        }
    }

    public static void changeMusicVol(float vol)
    {
        if (music)
        { 
            if (vol < 0)
                vol = 0;

            if (vol > 1)
            {
                vol = 1;
            }
            musicVolume = vol;
            music.volume = musicVolume;
        }

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
    
    public static void changeAudioVol(float vol)
    {
        if (vol < 0)
            vol = 0;

        if (vol > 1)
        {
            vol = 1;
        }
        audioVolume = vol;

        PlayerPrefs.SetFloat("audioVolume", audioVolume);
    }

    public static float getMusicVol()
    {
        return musicVolume;
    }

    public static float getAudioVol()
    {
        return audioVolume;
    }

    public static void MuteMusic()
    {
        if (music)
        {
            muteMusic = !muteMusic;
            music.mute = muteMusic;
        }
        SoundController.SetBool("muteMusic", muteMusic);

    }


    public static void MuteAudio()
    {
        muteAudio = !muteAudio;
        foreach (AudioSource loop in loops)
            loop.mute = muteAudio;

        SoundController.SetBool("muteAudio", muteAudio);

    }

    public static bool GetMuteAudio()
    {
        return muteAudio;
    }

    public static bool GetMuteMusic()
    {
        return muteMusic;
    }
}
