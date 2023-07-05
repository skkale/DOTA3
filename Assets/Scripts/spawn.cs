using UnityEngine;
using Photon.Pun;

public class spawn : MonoBehaviour
{
    public GameObject[] spawns;
    public GameObject Player;

    private void Awake()
    {
        Vector3 randomPosition = spawns[Random.Range(0, spawns.Length)].transform.localPosition;
        PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
    }
}
