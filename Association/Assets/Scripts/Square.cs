using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    Rigidbody2D squareRB;
    [SerializeField] float xSpeed;

    ShapeMovement mySquareMovement = new ShapeMovement(-3f, 0f, 2f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        squareRB = GetComponent<Rigidbody2D>();
        mySquareMovement.ResetPosition(squareRB);
    }

    // Update is called once per frame
    void Update()
    {
        mySquareMovement.MoveLeft(squareRB);
        mySquareMovement.MoveRight(squareRB);

    }
}
