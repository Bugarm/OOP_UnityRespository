using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : Singleton<ExitTrigger>
{

    protected override void Awake()
    {
        base.Awake();   
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneSwitchManager.Instance.SwitchToLevel("HUB");
        }
    }
}
