using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView view;
    static int i;
    GameObject controller;
    int kills;
    int deaths;

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
        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void Quit()
    {
        PhotonNetwork.Destroy(controller);
    }

    public void GetKill()
    {
        view.RPC(nameof(RPC_GetKill), view.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.view.Owner == player);
    }
}
