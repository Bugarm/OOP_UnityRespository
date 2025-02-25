using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreUpdate : Singleton<HighScoreUpdate>
{
    private List<TMP_Text> highScoreGroup;

    private TMP_Text highScoreTut;
    private TMP_Text highScoreForest;

    private GameObject highScoreTutObj;
    private GameObject highScoreForestObj;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScoreFunc();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HighScoreFunc();
    }

    void HighScoreFunc()
    {
        highScoreGroup = new List<TMP_Text>();

        highScoreTutObj = GameObject.Find("TutScore");
        highScoreForestObj = GameObject.Find("FortScore");

        if (highScoreTutObj != null)
        {
            highScoreTut = highScoreTutObj.GetComponent<TMP_Text>();
            highScoreGroup.Add(highScoreTut);
        }

        if (highScoreForestObj != null)
        {
            highScoreForest = highScoreForestObj.GetComponent<TMP_Text>();
            highScoreGroup.Add(highScoreForest);
        }

        UpdateHighScore();
    }

    void UpdateHighScore()
    {
        
        foreach (var highScore in highScoreGroup)
        {
            if (highScore != null)
            {
                string highName = highScore.name;
                    
                switch (highName)
                {
                    case "TutScore":
                        highScoreTut.text = "Total Score: " + GameData.Tutorial_HighScore.ToString();
                        break;
                    case "FortScore":
                        highScoreForest.text = "Total Score: " + GameData.Level1_HighScore.ToString();
                        break;
                }
            }
        }
        
        
    }
}
