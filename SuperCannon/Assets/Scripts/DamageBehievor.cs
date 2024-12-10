using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehievor : MonoBehaviour, ITakeDamage
{
    public void ApplyDamage(int hitpoints)
    {
       // GameManager.myGameManager.OnEnemyDie(hitpoints);
        EnemyScript _enemy = GetComponent<EnemyScript>();
        _enemy.strength--;
        StartCoroutine(ApplyDamageEffect());
        if (_enemy.strength <= 0)
        {
           // GameManager.myGameManager.OnEnemyDie(hitpoints);
            Destroy(this.gameObject);
        }
    }

    IEnumerator ApplyDamageEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color enemyColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = enemyColor;
    }

}