using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    public int id;
    public int speed;
    public float yOffset;

    private GameObject doorObj;

    private bool setupOnce = true;
    
    private float startPosY;

    // Start is called before the first frame update
    void Start()
    {
        setupOnce = false;
        doorObj = this.gameObject;
        startPosY = doorObj.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SlideDoorOpen()
    {
        Vector3 doorPos;
        doorPos = new Vector2(doorObj.transform.position.x, startPosY + yOffset);

        while(doorPos != doorObj.transform.position)
        { 
            doorObj.transform.position = Vector3.MoveTowards(doorObj.transform.position, doorPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 doorPos;
        doorObj = this.gameObject;

        if (setupOnce == true) // This will follow the current pos
        {
            doorPos = new Vector2(doorObj.transform.position.x, doorObj.transform.position.y + yOffset);
        }
        else
        {
            doorPos = new Vector2(doorObj.transform.position.x, startPosY + yOffset);
        }

        Gizmos.DrawWireSphere(doorPos, 0.5f);

    }
}
