using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.SlotRacer;

public class first_person_movement : MonoBehaviour
{
    [SerializeField] public float mvSpeed = 5f;
    private Vector2 velocity;


    [SerializeField] public float jumpStrength = 2f;
    [SerializeField] private float maxGroundDistance = 0.3f;

    private Rigidbody _rigidbody;
    private Transform _groundCheckObject;

    private bool isground;


    PhotonView view;
    public GameObject Camera;
    public first_person_movement scriptPlayerController;
   
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheckObject = GameObject.FindGameObjectWithTag("GroundCheck").transform;

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

        isground = Physics.Raycast(_groundCheckObject.transform.position, Vector3.down, maxGroundDistance);

        if (Input.GetButtonDown("Jump") && isground)
            _rigidbody.AddForce(Vector3.up * 100 * jumpStrength);

    }
}
