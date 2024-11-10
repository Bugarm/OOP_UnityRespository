using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
<<<<<<< HEAD
using UnityEngine.InputSystem.HID;
=======
>>>>>>> 5f7b9cd514eed3761cfaf7b1f0bcacc54383d574
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class ChessBoard : MonoBehaviour
{
    float xPos;
    float yPos;

    public static List<GameObject> allWhiteBlocks = new List<GameObject>();

    [SerializeField] GameObject chessBoardObj;
    //[SerializeField] GameObject chessBoardObj1;
    Color SquareColor;
    bool colorSwitch= true;
    GameObject square;

    // Start is called before the first frame update
    void Start()
    {
        for (float i = -4.48f; i < 5; i++)
        {
            for (float j = -4.48f; j < 5; j++)
            {
                /* if (i % 2 == 0 && j % 2 == 1)
                 {
                     allWhiteBlocks.Add( Instantiate(chessBoardObj, new Vector2(xPos, yPos), Quaternion.identity));
                 }
                 else if (i % 2 == 1 && j % 2 == 0)
                 {
                     allWhiteBlocks.Add(Instantiate(chessBoardObj, new Vector2(xPos, yPos), Quaternion.identity));
                 }
                 else
                 {
                     Instantiate(chessBoardObj1, new Vector2(xPos, yPos), Quaternion.identity);
                 }*/
                //xPos += 1f;

                SquareColor = (colorSwitch) ? (Color.white) : (Color.black);
                chessBoardObj.GetComponent<SpriteRenderer>().color = SquareColor;

                square = Instantiate(chessBoardObj, new Vector2(j, i), Quaternion.identity);
<<<<<<< HEAD
                square.transform.SetParent(this.gameObject.transform);
=======
>>>>>>> 5f7b9cd514eed3761cfaf7b1f0bcacc54383d574

                if (colorSwitch == true)
                {
                    allWhiteBlocks.Add(square);
                }

                colorSwitch = !colorSwitch;
              

            }
            colorSwitch = !colorSwitch;

            //yPos -= 1f;
            //xPos = -4.48f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            foreach (GameObject square in allWhiteBlocks)
            {
                square.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            foreach (GameObject square in allWhiteBlocks)
            {
                square.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        
    }
}
