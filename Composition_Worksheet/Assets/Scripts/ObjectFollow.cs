using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectFollow : MonoBehaviour
{

    //public Transform Follow;

    [SerializeField] Transform characterObj;
    RectTransform sliderTrans;

    float offsetY = 0.9f;

    private Camera MainCamera;


    // Start is called before the first frame update
    void Start()
    {
        sliderTrans = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        TransformWorldScreenSpace(characterObj.position);
    }

    void TransformWorldScreenSpace(Vector2 worldPos)
    {
        MainCamera = Camera.main;

        sliderTrans.position = new Vector2(worldPos.x, worldPos.y + offsetY);
    }
}

