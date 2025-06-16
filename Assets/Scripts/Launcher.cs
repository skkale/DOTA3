using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

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
    [SerializeField] GameObject HNSroleButton;
    [SerializeField] GameObject DMcharacterButton;
    //public string menuname;
    public static int o = 0;
    public int mapIndex = 0;
    public int selectedMapIndex;

    float roomListUpdateTimer = 0f;
    float roomListUpdateInterval = 2f; // раз на 2 секунди
   // private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Start()
    {
        HNSroleButton.SetActive(false);
        MenuManager.Instance.OpenMenu("title");
    }

    private void Awake()
    {
       // menuname = "roomDM";
        Instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.InLobby)
        {
            roomListUpdateTimer += Time.deltaTime;
            if (roomListUpdateTimer >= roomListUpdateInterval)
            {
                roomListUpdateTimer = 0f;
                UpdateRoomListUI(); // просто перебудовуємо інтерфейс!
            }
        }
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
        UpdateRoomListUI();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
        //menuname = "roomDM";
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
    //changeGamemodeButton.SetActive(PhotonNetwork.IsMasterClient);
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    mapGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        //changeGamemodeButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed " + message;
        MenuManager.Instance.OpenMenu("error");
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // зчитати mapIndex із Room Properties для надійності
            object mapObj;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("SelectedMap", out mapObj))
            {
                int selected = (int)mapObj;
                PhotonNetwork.LoadLevel("Game" + selected);
            }
            else
            {
                // запасний варіант — брати зі своєї змінної
                PhotonNetwork.LoadLevel("Game" + mapIndex);
            }
        }
    }
    [PunRPC]
 //   public void RPC_ChangeMenuName()
 //   {
 //       menuname = "roomHNS";
 //       MenuManager.Instance.OpenMenu("roomHNS");
 //   }

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
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
                cachedRoomList.Remove(info.Name);
            else
                cachedRoomList[info.Name] = info;
        }
        UpdateRoomListUI();
    }

    void UpdateRoomListUI()
    {
        foreach (Transform trans in roomListContent)
            Destroy(trans.gameObject);

        foreach (var pair in cachedRoomList)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(pair.Value);
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
    //public void SelectCharacterHider()
    //{
    //    o = 4;
    //    PlayerPrefs.SetInt("SelectedCharacter", o);
    //}
    //public void SelectCharacterSeeker()
    //{
    //    o = 5;
    //    PlayerPrefs.SetInt("SelectedCharacter", o);
    //}



    public void SelectScene0()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mapIndex = 0;
            Hashtable props = new Hashtable { { "SelectedMap", mapIndex } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }
    public void SelectScene1()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mapIndex = 1;
            Hashtable props = new Hashtable { { "SelectedMap", mapIndex } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public void SelectScene2()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mapIndex = 2;
            Hashtable props = new Hashtable { { "SelectedMap", mapIndex } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public void SelectScene3()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mapIndex = 3;
            Hashtable props = new Hashtable { { "SelectedMap", mapIndex } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public void DMgamemode() 
    { 
    HNSroleButton.gameObject.SetActive(false);
    DMcharacterButton.gameObject.SetActive(true);
    }
    //public void HNSgamemode()
    //{
    //HNSroleButton.gameObject.SetActive(true);
    //DMcharacterButton.gameObject.SetActive(false);
    //}

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
