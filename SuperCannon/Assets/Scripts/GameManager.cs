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

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject); // won't destroy when scene loads again
        GameData.Score = 0;
        DisplayScore();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnEnemyDie(int hitpoints)
    {
        GameData.Score += hitpoints;
        DisplayScore();
    }

    private void OnEnemyHits()
    {
        DisplayHP();
        if (GameData.Hp <= 0)
        {
            
        }
        GameData.Hp -= 1;
        //Debug.Log("Player health: " + GameData.Hp);
        GameManager.Instance.DisplayHP();
        Destroy(this.gameObject);


    }

    public void DisplayScore()
    {
        playerScoreText.text = "Score: " + GameData.Score.ToString();
    }

    public void DisplayHP()
    {
        playerHP.text = "HP: " + GameData.Hp.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.Hp = 100;
        DisplayHP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Lose Scene")
        {
            EnemySpawner myEnemySpawner = GetComponent<EnemySpawner>();
            Destroy(myEnemySpawner);
        }
        
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
}
