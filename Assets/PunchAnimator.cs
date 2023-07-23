using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAnimator : MonoBehaviour
{
    PhotonView view;
    Animator anim;
    private float time;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
         if (!view.IsMine)
             return;

        if ((time += Time.deltaTime) > 1.0f && Input.GetMouseButton(0))
        {
            time = 0.0f;
            anim.SetTrigger("Punch");
        }
            

    }
}
