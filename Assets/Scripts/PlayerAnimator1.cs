using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator1 : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            anim.SetTrigger("Run");

        if (Input.GetKeyDown(KeyCode.D))
            anim.SetTrigger("Run");

        if (Input.GetKeyDown(KeyCode.S))
            anim.SetTrigger("Run");

        if (Input.GetKeyDown(KeyCode.A))
            anim.SetTrigger("Run");
    }
}
