using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ReadyButton : MonoBehaviourPunCallbacks
{
    private int playersReady = 0;
    public static bool gameStarted = false;

    public Button readyButton;

    private void Start()
    {
        readyButton.onClick.AddListener(OnReadyButtonClick);
        Debug.Log(playersReady);
    }

    public void OnReadyButtonClick()
    {
        // Встановлюємо кнопку "Готово" в неактивний стан для даного гравця
        readyButton.interactable = false;

        // Відправляємо повідомлення про готовність гравця
        photonView.RPC("PlayerReady", target: RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PlayerReady()
    {
        playersReady++;
        Debug.Log(playersReady);
        // Перевіряємо, чи готові всі гравці
        if (playersReady == PhotonNetwork.PlayerList.Length)
        {
            // Запускаємо гру або виконуємо необхідні дії для початку гри
            StartGame();
        }
    }

    public void StartGame()
    {
        // Встановлюємо прапорець для позначення початку гри
        Debug.Log(playersReady);
        gameStarted = true;
        if (gameStarted)
        {
            SceneManager.LoadScene("Game");
        }
    }
}