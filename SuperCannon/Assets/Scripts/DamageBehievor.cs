using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehievor : MonoBehaviour, ITakeDamage
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ApplyDamage(int hitpoints)
    {
        EnemyScript _enemy = GetComponent<EnemyScript>();
        _enemy.strength--;
        Debug.Log("Enemy strength: " + _enemy.strength.ToString());
        if (_enemy.strength <= 0)
        {
            GameManager.Instance.OnEnemyDie(hitpoints);
            Destroy(this.gameObject);
        }
        StartCoroutine(ApplyDamageEffect());
    }

    IEnumerator ApplyDamageEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color enemyColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        animator.SetBool("isSpinning", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isSpinning", false);
        spriteRenderer.color = enemyColor;
    }

}