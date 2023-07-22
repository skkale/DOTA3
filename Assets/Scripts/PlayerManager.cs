using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView view;
    static int i;
    GameObject controller;
    public void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        i = PlayerPrefs.GetInt("SelectedCharacter");
        if (view.IsMine)
        {
            CreateController();
        }
    }

    //public GameObject[] AllCharacters;
    //public GameObject[] spawns;
    
    void CreateController()
    {
        Debug.Log(i);
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"+i), spawnpoint.position, spawnpoint.rotation, 0, new object[] { view.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
