using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbitEvent : MonoBehaviour
{
    public AudioSource ribbitSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RibbitSFX()
    {
        StartCoroutine(AudioManager.Instance.PlaySFXManual(ribbitSFX, this.gameObject.transform.position));
    }
}
