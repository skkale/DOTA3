using UnityEngine;
using Photon.Pun;

public class spawn : MonoBehaviour
{
    private int i = 1;
    public GameObject[] AllCharacters;
    public GameObject[] spawns;

    private void Awake()
    {
        
        Vector3 randomPosition = spawns[Random.Range(0, spawns.Length)].transform.localPosition;
        PhotonNetwork.Instantiate(AllCharacters[i].name, randomPosition, Quaternion.identity);
    }
}
