using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectScript : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        SceneManager.LoadScene("Menu");
    }
}
