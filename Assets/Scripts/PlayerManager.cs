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
        if (view.IsMine)
        {
            CreateController();
        }
    }

    //public GameObject[] AllCharacters;
    //public GameObject[] spawns;
    static int i = Launcher.o;
    GameObject controller;
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
