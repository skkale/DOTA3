using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.SlotRacer;

public class first_person_movement : MonoBehaviour
{
    [SerializeField] public float mvSpeed = 5f;
    private Vector2 velocity;

    PhotonView view;
    public GameObject Camera;
    public first_person_movement scriptPlayerController;
   
    private void Awake()
    {
        view = GetComponent<PhotonView>();

        if(!view.IsMine) 
        {
            Camera.SetActive(false);
            scriptPlayerController.enabled = false;
        }
    }

    private void Update()
    {
        velocity.x = Input.GetAxis("Horizontal") * mvSpeed * Time.deltaTime;
        velocity.y = Input.GetAxis("Vertical") * mvSpeed * Time.deltaTime; 

        transform.Translate(velocity.x, 0f,velocity.y);
    }
}
