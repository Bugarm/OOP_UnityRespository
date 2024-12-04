using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HP_UI : MonoBehaviour
{
    Slider sliderUI;
    // Start is called before the first frame update
    void Start()
    {
        sliderUI = this.gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sliderUI.value == 0)
        {
           
        }
    }
}
