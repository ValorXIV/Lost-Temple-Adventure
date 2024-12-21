using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    public int currentHealth;
    public int damage = 20;

    private Animator anim;
    private MonsterMovement monsterMove;
    private Collider2D monsterColl;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        monsterMove = GetComponent<MonsterMovement>();
        monsterColl = GetComponent<Collider2D>();

        // Get the sprite renderer component and save the original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Die()
    {
        // play die animation
        anim.SetBool("death",true);

        // disable this enemy
        monsterMove.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public void TakeDamage(int amount)
    {
        // take damage
        currentHealth -= amount;

        // play hurt animation
        anim.SetTrigger("hurt");

        // Change the sprite renderer color to orange
        spriteRenderer.color = new Color(1f,0.3654775f,0.08176094f,1f);

        // Coroutine to change the sprite renderer color back to the original color after a delay
        StartCoroutine(ChangeColorBack());

        // check death status
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ChangeColorBack()
    {
        //
        yield return new WaitForSeconds(.5f);

        // Change the sprite renderer color back to the original color
        spriteRenderer.color = originalColor;
    }
}
