using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource[] audioSourcesSFX;
    public AudioSource[] audioSourcesMusic;

    public Dictionary<string, AudioSource> audioSourceDicSFX;
    public Dictionary<string, AudioSource> audioSourceDicMusic;

    private AudioSource musicObj;

    private GameObject backMusicObj;

    public Coroutine audioPlayRoutine;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
       

        // Adds music to dictionary

        // SFX
        audioSourceDicSFX = new Dictionary<string, AudioSource>();

        for(int i = 0; i < audioSourcesSFX.Length; i++)
        {
            audioSourceDicSFX.Add(audioSourcesSFX[i].name, audioSourcesSFX[i]);
        }

        // Music
        audioSourceDicMusic = new Dictionary<string, AudioSource>();

        for (int i = 0; i < audioSourcesMusic.Length; i++)
        {
            audioSourceDicMusic.Add(audioSourcesMusic[i].name, audioSourcesMusic[i]);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustMusic()
    {
        if(audioSourceDicMusic.Count > 0)
        { 
            foreach (var source in audioSourceDicMusic)
            {
                source.Value.volume = GameData.MusicVolume;
                //Debug.Log(source.Key + " " + source.Value);
            }
        }
    }

    public void AdjustSFX()
    {
        foreach (var source in audioSourceDicSFX)
        {
            source.Value.volume = GameData.SfxVolume;
            //Debug.Log(source.Key + " " + source.Value);
        }
    }

    public IEnumerator PlaySFXManual(AudioSource audio,Vector2 pos)
    {
        AudioSource audioObj = Instantiate(audio);
        audioObj.Play();
        audioObj.transform.position = pos;

        yield return new WaitForSeconds(audioObj.clip.length);
        Destroy(audioObj.gameObject);
    }

    public IEnumerator PlayAudio(string audio, Vector2 pos)
    {
        foreach (var source in audioSourceDicSFX)
        {
            if (source.Key == audio)
            {
                AudioSource audioObj = Instantiate(source.Value);
                audioObj.Play();
                audioObj.transform.position = pos;
                 
                yield return new WaitForSeconds(audioObj.clip.length);
                Destroy(audioObj.gameObject);
                audioPlayRoutine = null;
            }
        }
    }

    public void StopAllSFX()
    {
        foreach (var source in audioSourceDicSFX)
        {
            source.Value.Stop();
            Destroy(source.Value.gameObject);
        }
    }

    public void PlayBackgroundMusic(AudioSource music)
    {
        backMusicObj = GameObject.FindGameObjectWithTag("BackgroundMusic");

        if (musicObj == null)
        { 
            musicObj = Instantiate(music);
            musicObj.Play();
            musicObj.volume = GameData.MusicVolume;
            musicObj.gameObject.transform.SetParent(backMusicObj.transform);
        }
    }

    public void StopBackgroundMusic()
    {
        if(musicObj != null)
        {
            musicObj.Stop();
        }
    }

    public void BackgroundMusicSetup(string sceneName)
    {
        if (audioSourceDicMusic.Count > 0)
        {
            foreach (AudioSource music in audioSourcesMusic)
            {
                if (sceneName.StartsWith("HUB") && music.name.StartsWith("HUB"))
                {
                    PlayBackgroundMusic(music);
                }
                else if (sceneName.StartsWith("TutorialRoom") && music.name.StartsWith("TutorialRoom"))
                {
                    PlayBackgroundMusic(music);
                }
                else if (sceneName.StartsWith("ForestLevel") && music.name.StartsWith("ForestLevel"))
                {
                    PlayBackgroundMusic(music);
                }
                else if (sceneName.StartsWith("MainMenu") && sceneName != "MainMenuWinRoom" && music.name.StartsWith("Title"))
                {
                    PlayBackgroundMusic(music);
                }
            }
        }
        else
        {
            Debug.Log("Music List is empty");
        }
    }

}
