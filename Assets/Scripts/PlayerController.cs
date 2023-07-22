using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject ui;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    float verticalLookRotation;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    
    PhotonView view;
    private Rigidbody _rigidbody;
    bool grounded;

    private bool isground;


    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;

    const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    public Image bar;
    
    PlayerManager playerManager;

    public AudioClip walk;

    private float time;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
    Cursor.lockState = CursorLockMode.Locked;
        if (view.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rigidbody);
            Destroy(ui);
        }
    }
    void Update()
    {
        if (!view.IsMine)   // пересування, керування
            return;

        Look();
        Jump();
        Move();

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
        
        if ((time += Time.deltaTime) > 0.1f)
        {
                if (Input.GetMouseButton(0))
                {
                time = 0.0f;
                items[itemIndex].Use();
                }
        }

        bar.fillAmount = currentHealth / 100;           // хапешка перенесена з іншого скрипта
        
        if (currentHealth < 1)
        {
            Death();
        }
        if (transform.position.y < -5f)
        {
            Death();
        }
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        //GetComponent<AudioSource>().PlayOneShot(walk);
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            _rigidbody.AddForce(transform.up * jumpForce * 100);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }
    void FixedUpdate()
    {
        if (!view.IsMine)
            return;

        _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
                                                  
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex) return;
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);
        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex = itemIndex;

        if (view.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!view.IsMine && targetPlayer == view.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage)     // механіка стрільби і дамагу
    {
        view.RPC("RPC_TakeDamage", RpcTarget.All, damage);  
    }
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if(!view.IsMine)
            return;
            
        Debug.Log("took damage " + damage);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        view = GetComponent<PhotonView>();
        if (other.gameObject.tag == "Death" && view.IsMine)
        {
            currentHealth -= 5;
        }
    }
    private void Death()
        {
        Destroy(gameObject);
        playerManager.Die();
        }

}
