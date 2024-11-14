using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBall : Ball
{
    // Start is called before the first frame update
    protected override void Start()
    {
        impulseVector = new Vector2(-2f, 0f);

        base.Start();

        Debug.Log("Rball");
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

}
