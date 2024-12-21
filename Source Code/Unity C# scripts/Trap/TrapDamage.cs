using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damage;

    private Collider2D coll;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

}
