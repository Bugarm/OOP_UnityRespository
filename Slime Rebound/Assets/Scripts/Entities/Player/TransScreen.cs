using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransScreen : Singleton<TransScreen>
{
    public GameObject screenObj;
    public Image blackTrans;
    protected override void Awake()
    {
        base.Awake();

        screenObj = this.gameObject;
        blackTrans = this.GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        screenObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
