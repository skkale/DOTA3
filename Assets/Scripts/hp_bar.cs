using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_bar : MonoBehaviour
{
    public float hp = 100f;
    public Image bar;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Death")
    //    {
    //        hp -= 5;
    //        bar.fillAmount = hp / 100;
    //        if (hp < 0)
    //        {
    //            Death();
    //        }
    //    }
    //}
    //private void Death()
    //{
    //    Destroy(gameObject);
    //}
}
