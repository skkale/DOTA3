using UnityEngine;
using Photon.Pun;

public class spawn : MonoBehaviour
{
    public GameObject[] AllCharacters;
    public GameObject[] spawns;
    public int o = select_character.currentCharacter;

    private void Awake()
    {
        
        Vector3 randomPosition = spawns[Random.Range(0, spawns.Length)].transform.localPosition;
        PhotonNetwork.Instantiate(AllCharacters[o].name, randomPosition, Quaternion.identity);
    }
}
