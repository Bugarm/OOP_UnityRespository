using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = this.gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameData.Score.ToString();
    }
}
