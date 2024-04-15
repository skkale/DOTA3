using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject changeGamemodeButton;
    [SerializeField] GameObject mapGameButton;
    public static int o = 0;
    public static int i = 0;
    public static int j = 1; // всього сцен(по рахунку як масив)

    private void Start()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    private void Awake()
    {
        Instance = this;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("roomDM");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    changeGamemodeButton.SetActive(PhotonNetwork.IsMasterClient);
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    mapGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        changeGamemodeButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game"+i);
    }
    public void ChangeMenu()
    {
        MenuManager.Instance.OpenMenu("roomHNS");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SelectCharacter1()
    {
        o = 0;
        PlayerPrefs.SetInt("SelectedCharacter", o);
    }
    public void SelectCharacter2()
    {
        o = 1;
        PlayerPrefs.SetInt("SelectedCharacter", o);
    }
    public void SelectCharacter3()
    {
        o = 2;
        PlayerPrefs.SetInt("SelectedCharacter", o);
    }
    public void SelectCharacter4()
    {
        o = 3;
        PlayerPrefs.SetInt("SelectedCharacter", o);
    }
    public void SelectCharacterHider()
    {
        o = 4;
        PlayerPrefs.SetInt("SelectedCharacter", o);
    }
    public void SelectCharacterSeeker()
    {
        o = 5;
        PlayerPrefs.SetInt("SelectedCharacter", o);
    }



    public void SelectScene0()
    {
        i = 0;
    }

    public void SelectScene1()
    {
        i = 1;
    }

    public void SelectScene2()
    {
        i = 2;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
