using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class hp_bar : MonoBehaviour
{
    PhotonView view;
    public float hp = 100f;
    [SerializeField]
    public Image bar;
    private void OnTriggerEnter(Collider other)
    {
        view = GetComponent<PhotonView>();
        if (other.gameObject.tag == "Death" && view.IsMine)
        {
            hp -= 5;
            bar.fillAmount = hp / 100;
            if (hp < 1)
            {
                Death();
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < -5f)
        {
            Death();
        }
    }
    private void Death()
    {
        Destroy(gameObject);
    }

}
