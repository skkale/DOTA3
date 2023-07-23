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
        if (Input.GetKey(KeyCode.W))
            anim.SetTrigger("Run");

        if (Input.GetKey(KeyCode.D))
            anim.SetTrigger("Run");

        if (Input.GetKey(KeyCode.S))
            anim.SetTrigger("Run");

        if (Input.GetKey(KeyCode.A))
            anim.SetTrigger("Run");
    }
}
