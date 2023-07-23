using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator1 : MonoBehaviour
{
    PhotonView view;
    Animator anim;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!view.IsMine)
            return;

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
