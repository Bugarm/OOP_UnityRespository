using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text playerHP;

    public void OnEnemyDie(int hitpoints)
    {
        GameData.Score += hitpoints;
        playerScoreText.text = "Score: " + GameData.Score.ToString();
    }

    public void OnPlayerHP()
    {
        playerHP.text = "HP: " + GameData.Hp.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.Hp = 100;
        GameData.Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
