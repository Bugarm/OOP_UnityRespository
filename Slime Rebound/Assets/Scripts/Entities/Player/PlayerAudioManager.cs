using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : Singleton<PlayerAudioManager>
{
    AudioManager audioInstance;

    Vector3 pos;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        pos = this.gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = this.gameObject.transform.position;

        try 
        { 
            AudioController();
        }
        catch (Exception ex)
        {
            Debug.Log("MISSING AUDIO MANAGER: " + ex.Message);
        }
    }

    void AudioController()
    {
        audioInstance = AudioManager.Instance;

        if (PlayerState.IsAttack == true || PlayerState.IsDash == true)
        {
            if (audioInstance.audioPlayRoutine == null)
            {
                audioInstance.audioPlayRoutine = StartCoroutine(audioInstance.PlayAudio("SwooshSFX", pos));
            }
        }

        if (PlayerState.IsPound == true && (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true || PlayerState.IsDamaged == true))
        {
            if (audioInstance.audioPlayRoutine == null)
            {
                audioInstance.audioPlayRoutine = StartCoroutine(audioInstance.PlayAudio("SlimeLandSFX", pos));
            }
        }

        if (PlayerState.IsJump == true && PlayerState.IsBounceMode == true && PlayerState.IsTouchingWall == true)
        {
            if (audioInstance.audioPlayRoutine == null)
            {
                audioInstance.audioPlayRoutine = StartCoroutine(AudioManager.Instance.PlayAudio("JumpSFX", pos));

                audioInstance.audioPlayRoutine = StartCoroutine(AudioManager.Instance.PlayAudio("SlimeSFX", pos));
            }
        }

        if (PlayerState.IsAttackJump == true && (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true || PlayerState.IsDamaged == true))
        {
            if (audioInstance.audioPlayRoutine == null)
            {
                audioInstance.audioPlayRoutine = StartCoroutine(AudioManager.Instance.PlayAudio("JumpSFX", pos));
            }
        }

        if (PlayerState.IsJump == true && PlayerState.IsBounceMode == false && PlayerState.IsAttackJump == false)
        {
            if (audioInstance.audioPlayRoutine == null)
            {
                audioInstance.audioPlayRoutine = StartCoroutine(AudioManager.Instance.PlayAudio("JumpSFX", pos));
            }
        }

    }
}
