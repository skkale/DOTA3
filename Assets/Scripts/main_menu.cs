using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class main_menu : MonoBehaviourPunCallbacks
{
    public InputField InputFieldNAMEROOM;
    public GameObject ErrorPanel;
    private bool firstStart = true;
    
    public void CreateRoom()
    {
    ErrorPanel.SetActive(false);
        if(InputFieldNAMEROOM.text.Length > 3 || firstStart == true)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(InputFieldNAMEROOM.text, roomOptions);
            firstStart= false;
        }
        else ErrorPanel.SetActive(true);

    }

    public void JoinRoom()
    {
        if (InputFieldNAMEROOM.text.Length > 3 || firstStart == true) 
        {
            PhotonNetwork.JoinRoom(InputFieldNAMEROOM.text);
            firstStart = false;
        } 
        else ErrorPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

}
