using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    public int currentHealth;

    [SerializeField] private float knockbackStrength = 16, knockbackDelay = 0.1f;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D coll;
    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            int enemyDamage = collision.gameObject.GetComponent<MonsterLife>().damage;
            TakeDamage(enemyDamage,collision.collider);
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Trap"))
        {
            int trapDamage = collider.GetComponent<TrapDamage>().damage;
            TakeDamage(trapDamage,collider);
        }
    }

    public void TakeDamage(int amount, Collider2D collider)
    {
        // take damage
        currentHealth -= amount;

        // update hp bar
        healthBar.SetHealth(currentHealth);

        // check if character is going to die first
        if (currentHealth <= 0)
        {
            Die(collider);
        }
        else
        {
            Hurt(collider);
        }
    }

    private void Hurt(Collider2D collider)
    {   
        // knockback mechanic
        Knockback(collider);

        // disable collision temporarily
        Physics2D.IgnoreCollision(coll, collider, true);

        // play hurt animation and blink effect
        anim.SetTrigger("hurt");
        StartCoroutine(Blink(1f));

        // re-enable collision after a delay
        StartCoroutine(ReactivateCollider(1f,collider));
    }

    private void Die(Collider2D collider)
    {
        // play death animation
        Physics2D.IgnoreCollision(coll, collider, true);
        anim.SetTrigger("hurt");
        anim.SetBool("death", true);

        // set body to be unable to do anything
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        this.enabled = false;

        // restart the scene
        Invoke("RestartLevel",2.5f);
    }

    public void Knockback(Collider2D collider)
    {
        GetComponent<PlayerMovement>().enabled = false;
        Vector2 direction = new Vector2(transform.position.x - collider.transform.position.x,0.2f);
        direction.Normalize();
        rb.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);
        StartCoroutine(KnockbackDelay(knockbackDelay));
    }

    private IEnumerator KnockbackDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator ReactivateCollider(float duration,Collider2D collider)
    {
        yield return new WaitForSeconds(duration); // Delay for assigned duration
        Physics2D.IgnoreCollision(coll,collider,false);
    }

    private IEnumerator Blink(float duration)
    {
        float blinkTime = 0f;

        while (blinkTime < duration)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.17f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.17f);
            blinkTime += 0.2f;
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}