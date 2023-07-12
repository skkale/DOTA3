using UnityEngine.SceneManagement;
using Photon.Pun;

public class connect_script : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
