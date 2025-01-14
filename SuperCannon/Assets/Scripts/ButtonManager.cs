using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] List<Button> buttons;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in buttons)
        {
            if(button.name == "StartBut")
            {
                button.onClick.AddListener(StartGame);
            }
            if (button.name == "ExitBut")
            {
                button.onClick.AddListener(ExitGame);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void StartGame()
    {
        Debug.Log("You have pressed the Start Button");
        SceneManager.LoadScene("SampleScene");
    }

    void ExitGame()
    {
        Debug.Log("You have pressed the Exit Button");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();

        #endif
    }

}
