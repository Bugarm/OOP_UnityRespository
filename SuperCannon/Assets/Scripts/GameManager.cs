using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text playerHP;
    [SerializeField] TMP_Text enemyLeftText;
    [SerializeField] TMP_Text levelText;

    public int fadeSpeed = 1;
    public int enemyCount = 5;
    int startHp = 100;

    Coroutine levelTrans;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject); // won't destroy when scene loads again

        GameData.EnemyCount = enemyCount;
        //DisplayECount();
        GameData.LevelCount = 0;
        levelText.alpha = 0;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnEnemyDie(int hitpoints)
    {
        GameData.Score += hitpoints;
        GameData.EnemyCount--;
        DisplayScore();

        if(GameData.EnemyCount <= 0)
        {
            // Goes to the next Level aka makes the level harder
            if (GameData.LevelCount >= 2)
            {
                SceneManager.LoadScene("Win Screen");
            }
            else
            {
                GameData.LevelCount++;
                DisplayLevel();
                if(levelTrans == null)
                {
                    levelTrans = StartCoroutine(LevelTransition());
                }
                GameData.EnemyCount = 20;
                Debug.Log("LEVEL COUNT: " + GameData.LevelCount);
                
            }
            
        }
        //else
        //{
        //    //DisplayECount();
        //}
        
    }

    

    private void OnEnemyHits()
    {
        DisplayHP();
        if (GameData.Hp <= 0)
        {
            SceneManager.LoadScene("Lose Screen");
        }
        else
        {
            GameData.Hp -= 1;
            //Debug.Log("Player health: " + GameData.Hp);
            GameManager.Instance.DisplayHP();
            Destroy(this.gameObject);
        }
       

    }

    public void DisplayScore()
    {
        playerScoreText.text = "Score: " + GameData.Score.ToString();
        SaveLoadManager.Instance.SaveData();
    }

    public void DisplayHP()
    {
        playerHP.text = "HP: " + GameData.Hp.ToString();
    }

    //public void DisplayECount()
    //{
    //    enemyLeftText.text = "Enemies Left: " + GameData.EnemyCount.ToString();
    //}

    public void DisplayLevel()
    {
        levelText.text = "Level " + GameData.LevelCount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.Score = 0;
        GameData.Hp = startHp;
        SaveLoadManager.Instance.LoadData();
        DisplayScore();
        DisplayHP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LevelTransition()
    {
        while(true)
        {
            bool finishFade = FadeObjIn(levelText.color);

            if (finishFade == true)
            {
                StopCoroutine(levelTrans);
                
            }
            yield return new WaitForSeconds(1);
        }
        
    }

    public bool FadeObjIn(Color objColor)
    {
        float fadeAmount = objColor.a + (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r,objColor.g,objColor.b,fadeAmount);
        
        if(objColor.a >= 100)
        {
            return true;
        }
        return false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Destory Enemy Spawner
        if(scene.name == "Lose Screen" || scene.name == "Win Screen")
        {
            EnemySpawner myEnemySpawner = GetComponent<EnemySpawner>();
            Destroy(myEnemySpawner);
        }

        //Reset at lose
        if (scene.name == "Lose Screen")
        {
            GameData.Hp = startHp;
            //GameData.Score = 0;
            SaveLoadManager.Instance.SaveData();
        }

        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
}
