using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestOpen : MonoBehaviour
{
    private Collider2D coll;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("open");
            GetComponent<Collider2D>().enabled = false;
            Invoke("endingGame",2f);
        }
    }

    public void endingGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
