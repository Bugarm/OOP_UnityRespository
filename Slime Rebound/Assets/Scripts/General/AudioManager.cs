using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource[] audioSourcesSFX;
    public AudioSource[] audioSourcesMusic;

    public Dictionary<string, AudioSource> audioSourceDicSFX;
    public Dictionary<string, AudioSource> audioSourceDicMusic;


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

        OptionData.SfxVolume = 0.2f;

        AdjustSFX();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustMusic()
    {
        foreach (var source in audioSourceDicMusic)
        {
            source.Value.volume = OptionData.MusicVolume;
            //Debug.Log(source.Key + " " + source.Value);
        }
    }

    public void AdjustSFX()
    {
        foreach (var source in audioSourceDicSFX)
        {
            source.Value.volume = OptionData.SfxVolume;
            //Debug.Log(source.Key + " " + source.Value);
        }
    }

    public IEnumerator PlayAudio(string audio, Vector2 pos, bool isSFX = true)
    {
        Dictionary<string, AudioSource> audioDic = isSFX == true ? audioSourceDicSFX : audioSourceDicMusic;

        foreach (var source in audioDic)
        {
            if (source.Key == audio)
            {
                AudioSource audioObj = Instantiate(source.Value);
                audioObj.Play();
                audioObj.transform.position = pos;
                 
                yield return new WaitForSeconds(audioObj.clip.length);
                Destroy(audioObj.gameObject);
            }
        }
    }

    public void StopAllAudio(bool isSFX = true)
    {
        Dictionary<string, AudioSource> audioDic = isSFX == true ? audioSourceDicSFX : audioSourceDicMusic;

        foreach (var source in audioDic)
        {
            source.Value.Stop();
            Destroy(source.Value.gameObject);
        }
    }
}
