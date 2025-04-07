using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinManager : Singleton<WinManager>
{
    public TMP_Text scoreText;

    public TMP_Text bouncetext;

    public Button backButton;

    private float counterDelay = 0.02f;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextCounter(scoreText));

        bouncetext.text = "Total Bounces: " + GameData.TotalBounces.ToString();

        backButton.onClick.AddListener(GoBackHUB);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TextCounter(TMP_Text textObj)
    {
        bool finished = false;
        int totalCount = GameData.Score;
        int count = 0;

        while (finished == false)
        {
            if(count < totalCount)
            {
                count++;
                textObj.text = "Score: " + count.ToString();

                // Speeds up
                if(count > totalCount /2)
                {
                    counterDelay = 0.005f;
                }
            }
            else
            {
                if (count > totalCount)
                {
                    count = totalCount;
                }
                textObj.text = "Score: " + count.ToString();

                finished = true;
            }
            yield return new WaitForSeconds(counterDelay);
        }
    }

    private void GoBackHUB()
    {
        SceneSwitchManager.Instance.SwitchToLevel("HUB");
    }

}
