using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    PhotonView view;
    public void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
       // if (view.IsMine)
        //{
            CreateController();
        //}
            
    }
    
    public GameObject[] AllCharacters;
    public GameObject[] spawns;
    static int o = select_character.currentCharacter;
    GameObject controller;
    void CreateController()
    {
        Vector3 randomPosition = spawns[Random.Range(0, spawns.Length)].transform.localPosition;
        controller = PhotonNetwork.Instantiate(AllCharacters[o].name, randomPosition, Quaternion.identity, 0, new object[] {view.ViewID});
    }
    
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
            CreateController();
    }
}
