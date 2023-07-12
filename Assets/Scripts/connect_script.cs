using UnityEngine.SceneManagement;
using Photon.Pun;

public class connect_script : MonoBehaviourPunCallbacks
{
    PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        view = GetComponent<PhotonView>();
        SceneManager.LoadScene("MainMenu");
    }

}
