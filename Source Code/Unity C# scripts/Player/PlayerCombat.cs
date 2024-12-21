using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private MQTTAttack mqttattack;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    [SerializeField] private int attackDamage = 20;
    public float attackRange = 0.5f;
    public float attackRate = 1f;
    float nextAttackTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        mqttattack = GetComponent<MQTTAttack>();
    }
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (mqttattack.receivedAttackPayload == "True")
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // play attack animation
        anim.SetTrigger("attack");

        // detect enemies in range of an attack
        Collider2D[] hitEnemies =   Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<MonsterLife>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
