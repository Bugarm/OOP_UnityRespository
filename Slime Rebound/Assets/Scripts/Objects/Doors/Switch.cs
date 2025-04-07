using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Switch : MonoBehaviour
{
    public string Objectid = "switch";

    public int id;

    public AudioSource winSFX;

    public GameObject particlePlay;

    private GameObject thisGameObj;

    public bool switchActive;

    private ParticleSpawnerManager particleInstance;

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
                if(collision.name == "Head-RB")
                { 
                    GameObject[] doors = GameObject.FindGameObjectsWithTag("SwitchDoor");

                    foreach (GameObject door in doors)
                    {
                        SwitchDoor doorSwitch = door.GetComponent<SwitchDoor>();
                    
                        if(doorSwitch.id == id)
                        {
                            StartCoroutine(AudioManager.Instance.PlaySFXManual(winSFX, this.gameObject.transform.position));

                            StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleBasketWin, thisGameObj.transform.position, Quaternion.LookRotation(Vector3.up)));
                            StartCoroutine(doorSwitch.SlideDoorOpen());     
                        }
                    }
                    switchActive = false;
                }
            }
        }
    }
}
