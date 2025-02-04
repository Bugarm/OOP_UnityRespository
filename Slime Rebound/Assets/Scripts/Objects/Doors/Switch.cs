using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Switch : MonoBehaviour
{
    public int id;

    public GameObject particlePlay;

    private GameObject thisGameObj;

    private bool switchActive;

    private Coroutine doorRoutine;
    // Start is called before the first frame update
    void Start()
    {
        thisGameObj = this.gameObject;
        switchActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(switchActive == true)
        { 
            if (collision.CompareTag("PlayerAttack"))
            {
                GameObject[] doors = GameObject.FindGameObjectsWithTag("SwitchDoor");

                foreach (GameObject door in doors)
                {
                    SwitchDoor doorSwitch = door.GetComponent<SwitchDoor>();

                    if(doorSwitch.id == id)
                    {
                        Instantiate(particlePlay,this.gameObject.transform.position, Quaternion.LookRotation(Vector3.up));
                        StartCoroutine(doorSwitch.SlideDoorOpen());     
                        
                    }
                }
                switchActive = false;
            }
        }
    }
}
