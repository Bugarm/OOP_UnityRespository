using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeleChain : MonoBehaviour
{
    protected GameObject player;
    private Rigidbody2D playerRB;

    private int powerX;
    private int powerXval;

    private Coroutine deathRoutine;

    public AudioSource breakSFX;

    // Start is called before the first frame update
    void Start()
    {
        powerXval = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if(deathRoutine == null)
            {
                deathRoutine = StartCoroutine(DestroyingObject());
            }
        }
    }

    private IEnumerator DestroyingObject()
    {
        yield return new WaitForSeconds(0.1f);
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(player);
        playerRB = player.GetComponentInParent<Rigidbody2D>();

        StartCoroutine(AudioManager.Instance.PlaySFXManual(breakSFX, this.gameObject.transform.position));

        GameData.ChainsInLevel--;
        
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;

        powerX = Mathf.Sign(playerRB.velocity.x) < 0 ? -powerXval : powerXval;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(powerX, 3, 1);

        yield return new WaitForSeconds(0.5f);

        LevelExitDoor.Instance.DestroyDoorCheck(DontDestroyGroup.Instance.exitDoor, DontDestroyGroup.Instance.exitTrigger);
        yield return new WaitForSeconds(0.4f);

        Destroy(this.gameObject);
    }
}
