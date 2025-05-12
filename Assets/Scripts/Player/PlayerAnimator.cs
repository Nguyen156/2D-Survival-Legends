using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < .001)
            anim.Play("Idle");
        else
            anim.Play("Move");
    }
}
