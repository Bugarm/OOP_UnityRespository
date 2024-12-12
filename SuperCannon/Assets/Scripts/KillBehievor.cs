using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBehievor : MonoBehaviour, ITakeDamage
{
    public void ApplyDamage(int hitpoints)
    {

            GameManager.Instance.OnEnemyDie(hitpoints);
            Destroy(this.gameObject);
        
    }


}
