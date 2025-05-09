using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : Singleton<GameManager>
{

    public List<Image> hpBarsUI;

    public TMP_Text scoreUI;
    public TMP_Text bounceUI;
    public Image blackTrans;

    public GameObject player;
    private Rigidbody2D playerRB;

    private float powerX;

    private int maxHP;

    public bool sceneSwitch;

    public List<SaveableObjects> saveableObjects;
    SerializedLevelData myLevelData = new SerializedLevelData();


    public Coroutine damageRoutine, flashRoutine, dataSwitchRoutine, InbounceUIRoutine, OutbounceUIRoutine, highBounceRoutine;


    protected override void Awake()
    {
        base.Awake();

        GameData.ChainsInLevel = 0;

    }

    // Start is called before the first frame update
    void Start()
    {
        // To remove the uneccery Error when loading
        try
        {
            maxHP = hpBarsUI.Count;
            GameData.Hp = hpBarsUI.Count;

            bounceUI.alpha = 0;
            bounceUI.gameObject.SetActive(false);

            playerRB = player.GetComponentInParent<Rigidbody2D>();

            GameData.Score = 0;
            GameData.TotalBounces = 0;
            GameData.HasEnteredDoor = false;

            DisplayScore();
            DisplayHp();
        }
        catch
        {
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void CheckHP()
    {
        if (GameData.Hp <= 0)
        {
            SaveLoadManager.Instance.LoadCheckpointData();
            StartCoroutine(DontDestroyManager.Instance.ScreenTrans(false, GameData.LevelState));
            SaveLoadManager.Instance.LoadLevelData(SceneManager.GetActiveScene().name);
            StartCoroutine(DontDestroyGroup.Instance.CheckpointLoadData());

        }
    }

    // Routines
    public IEnumerator DamagePlayer()
    {
        PlayerState.IsBounceMode = false;
        PlayerState.IsStickActive = false;

        PlayerState.IsDamaged = true;
        PlayerState.DisableAllMove = true;

        // KnockBack
        playerRB.gravityScale = 0.5f;

        powerX = Mathf.Sign(playerRB.velocity.x) < 0 ? 6 : -6;
        playerRB.velocity = new Vector3(powerX, 4, 1);

        if(flashRoutine == null)
        { 
             flashRoutine = StartCoroutine(ColorFlash(player));
        }

        StartCoroutine(AudioManager.Instance.PlayAudio("DamageSFX", player.transform.position));

        yield return new WaitForSeconds(0.7f);

        GameData.Hp--;
        DisplayHp();

        PlayerState.IsDamaged = false;
        PlayerState.DisableAllMove = false;

        StopCoroutine(flashRoutine);
        flashRoutine = null;

        playerRB.gravityScale = 10f;
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        PlayerAnimationManager.Instance.PlayAnimation("idle");

        CheckHP();

        // Delay before getting damaged again
        yield return new WaitForSeconds(1.5f);
        damageRoutine = null;
    }

    public IEnumerator ColorFlash(GameObject entity)
    {
        bool mode;

        //Debug.Log(entity.name);

        mode = entity.name == "Player" ? true : false;

        while (true)
        {
            yield return new WaitForSeconds(0.20f);

            if (mode == true)
            {
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                entity.GetComponent<SpriteRenderer>().color = Color.red;
            }
            yield return new WaitForSeconds(0.25f);

            if (mode == true)
            {
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                entity.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    // Displays
    public void DisplayHp()
    {
        if(GameData.Hp > maxHP)
        {
            GameData.Hp = maxHP;
        }

        for (int i = 0; i < maxHP; i++)
        {
            if(i < GameData.Hp)
            { 
                hpBarsUI[i].enabled = true;
            }
            else
            {
                hpBarsUI[i].enabled = false;
            }
        }

        //hpUI.text = GameData.Hp.ToString() + "/10";
    }

    public void DisplayScore()
    {
        string scoreDefualt = "0000000";
        int scoreData = GameData.Score;
        string score;

        if (GameData.Score.ToString().Length > scoreDefualt.Length)
        {
            // Maxes out of 999999 if it over loads
            score = "9999999";
        }
        else
        { 
                                                        // Range
           score = (scoreDefualt + scoreData.ToString())[scoreData.ToString().Length..];
        }

        scoreUI.text = score;
        // save data here?
    }


    // Bounce UI Functions
    private IEnumerator HighestBounceEffect(bool failed)
    {
        int repeat = 0;

        Color colorPick = failed == false ? Color.yellow : Color.red;

        while (repeat < 2)
        {
            yield return new WaitForSeconds(0.3f);
            bounceUI.color = colorPick;
            yield return new WaitForSeconds(0.3f);
            bounceUI.color = Color.white;
            yield return new WaitForSeconds(0.3f);
            bounceUI.color = colorPick;
            repeat++;
        }

        bounceUI.color = Color.white;
        highBounceRoutine = null;
    }

    public IEnumerator FadeInBounceUI()
    {
        bounceUI.gameObject.SetActive(true);
        //bounceUI.alpha = 0;

        while (bounceUI.alpha <= 1)
        {
            // Fade In
            bounceUI.alpha += Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

        }

        bounceUI.alpha = 1;
        InbounceUIRoutine = null;

    }

    public IEnumerator FadeOutBounceUI()
    {
        bounceUI.gameObject.SetActive(true);
        //bounceUI.alpha = 1;

        while (bounceUI.alpha >= 0)
        {
            // Fade Out
            bounceUI.alpha -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }

        bounceUI.alpha = 0;
        bounceUI.gameObject.SetActive(false);
        OutbounceUIRoutine = null;

    }

    public void UpdateBounces(int bounces, bool failed)
    {

        if(GameData.TotalBounces < bounces || failed == false)
        {
            bounceUI.text = "Bounces: " + bounces.ToString();
        }
        else if (failed == true)
        {
            bounceUI.text = "Owie..Tired...";
        }
        
    }

    public void BouncesTotalCounted(int bounces, bool failed)
    {
        if (GameData.TotalBounces < bounces && failed == false)
        {
            GameData.TotalBounces = bounces;

            // High Bounce Count Effect
            if(highBounceRoutine == null)
            {
                highBounceRoutine = StartCoroutine(HighestBounceEffect(failed));
            }
        }
        else if(failed == true)
        {
            if (highBounceRoutine == null)
            {
                highBounceRoutine = StartCoroutine(HighestBounceEffect(failed));
            }
        }
    }


}
