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

    public TMP_Text hpUI;
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


    public Coroutine screenTransRoutine, damageRoutine, flashRoutine, dataSwitchRoutine, InbounceUIRoutine, OutbounceUIRoutine, highBounceRoutine;


    protected override void Awake()
    {
        base.Awake();

        maxHP = 10;
        bounceUI.alpha = 0;
        bounceUI.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponentInParent<Rigidbody2D>();

        GameData.Hp = 3;
        GameData.Score = 0;
        GameData.TotalBounces = 0;
        GameData.HasSceneTransAnim = false;

        DisplayScore();
        DisplayHp();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameData.Hp <= 0)
        {
            if(SaveLoadManager.Instance.hasSavedOnce == false)
            {
                SceneManager.LoadScene(GameData.LevelState);
                SaveLoadManager.Instance.LoadCheckpointData();
                SaveLoadManager.Instance.LoadLevelData(SceneManager.GetActiveScene().name);
                DontDestroyGroup.Instance.CheckpointLoadData();
            }
            else
            {
                SaveLoadManager.Instance.LoadCheckpointData();
                SaveLoadManager.Instance.LoadLevelData(SceneManager.GetActiveScene().name);
                DontDestroyGroup.Instance.CheckpointLoadData();

            }
        }

    }
 
    // Routines

    public IEnumerator DamagePlayer()
    {
        PlayerState.IsBounceMode = false;
        PlayerState.IsStickActive = false;

        PlayerState.IsDamaged = true;

        // KnockBack
        playerRB.gravityScale = 0.5f;

        powerX = Mathf.Sign(playerRB.velocity.x) < 0 ? 6 : -6;
        playerRB.velocity = new Vector3(powerX, 4, 1);

        if(flashRoutine == null)
        { 
             flashRoutine = StartCoroutine(ColorFlash(player));
        }

        yield return new WaitForSeconds(0.7f);

        GameData.Hp--;
        DisplayHp();

        PlayerState.IsDamaged = false;

        StopCoroutine(flashRoutine);
        flashRoutine = null;

        playerRB.gravityScale = 10f;
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;

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

        hpUI.text = GameData.Hp.ToString() + "/10";
        // save data here?
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

    public IEnumerator ScreenTrans(int? nextRoomNum)
    {

        blackTrans.gameObject.SetActive(true);
        blackTrans.color = new Color(0,0,0,0);

        while (blackTrans.color.a <= 1)
        {
            // Fade In
            blackTrans.color = new Color(0, 0, 0, blackTrans.color.a + Time.deltaTime + 0.01f); 
            yield return new WaitForSeconds(0.001f);

        }
        
        SceneTransFunct(nextRoomNum);
        yield return new WaitForSeconds(0.35f);
        

        while (blackTrans.color.a >= 0)
        {
            // Fade Out
            blackTrans.color = new Color(0, 0, 0, blackTrans.color.a - Time.deltaTime - 0.01f);
            yield return new WaitForSeconds(0.001f);
        }

        GameData.HasSceneTransAnim = false;
        blackTrans.color = new Color(0, 0, 0, 0);
        blackTrans.gameObject.SetActive(false);
        screenTransRoutine = null;
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

    private void SceneTransFunct(int? nextRoomNum)
    {
        if (nextRoomNum <= 0)
        {
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(GameData.LevelState);
        }
        else
        {
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(GameData.LevelState + nextRoomNum);
        }
        this.gameObject.GetComponentInParent<DontDestroyGroup>().enabled = true;
    }
}
