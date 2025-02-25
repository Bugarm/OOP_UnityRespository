using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OptionData
{
    private static float _sfxVolume;
    private static float _musicVolume;


    // 

    public static float SfxVolume
    {
        get { return _sfxVolume; }
        set { _sfxVolume = value; }
    }

    public static float MusicVolume
    {
        get { return _musicVolume; }
        set { _musicVolume = value; }
    }
}
