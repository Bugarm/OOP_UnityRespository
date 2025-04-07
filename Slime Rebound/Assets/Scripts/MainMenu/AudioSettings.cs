using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettings : Singleton<AudioSettings>
{ 
    [SerializeField] Slider musicSlider;

    [SerializeField] TMP_Text musicNum;

    [SerializeField] Slider sfxSlider;

    [SerializeField] TMP_Text sfxNum;

    protected override void Awake()
    {
        base.Awake();

        musicSlider.value = GameData.MusicVolume * 100;

        sfxSlider.value = GameData.SfxVolume * 100;
        SaveLoadManager.Instance.LoadAudioData();
    }

    public void UpdateMusic()
    {
        float val = musicSlider.value / 100;

        musicNum.text = musicSlider.value.ToString();

        GameData.MusicVolume = val;
        AudioManager.Instance.AdjustMusic();
        SaveLoadManager.Instance.SaveAudioData();
    }

    public void UpdateSFX()
    {
        float val = sfxSlider.value / 100;

        sfxNum.text = sfxSlider.value.ToString();

        GameData.SfxVolume = val;
        AudioManager.Instance.AdjustSFX();
        SaveLoadManager.Instance.SaveAudioData();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
