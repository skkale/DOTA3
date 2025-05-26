using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject deathscreen;
    [SerializeField] GameObject crosshair;
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

    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public bool isDead = false;
    public Image bar;
    
    PlayerManager playerManager;

    public AudioClip walk;
    public AudioClip damagesound;
    public AudioClip deathsound;

    private float time;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        deathscreen.gameObject.SetActive(false);
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
        if (isDead)
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
        if (Input.GetMouseButtonDown(0))
            items[itemIndex].StartFireEffect();
        if (Input.GetMouseButtonUp(0))
            items[itemIndex].StopFireEffect();

        bar.fillAmount = currentHealth / maxHealth;           // хапешка перенесена з іншого скрипта
        
        if (currentHealth < 1)
        {
            deathscreen.gameObject.SetActive(true);
            Death();
        }
        if (transform.position.y < -5f)
        {
            deathscreen.gameObject.SetActive(true);
            Death();
        }

        // exit(beta)
        if (view.IsMine && ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) ))
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Menu");
            SceneManager.LoadScene("LoadingScene");
            Cursor.lockState = CursorLockMode.None;
            //Application.Quit();
        }
    }
    public override void OnLeftRoom()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingMenu");
        SceneManager.LoadScene("Menu");
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
        if (changedProps.ContainsKey("itemIndex") && !view.IsMine && targetPlayer == view.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
            // тут міг би бути мазл флеш
        }
    }

    public void TakeDamage(float damage)     // механіка стрільби і дамагу
    {
        view.RPC(nameof(RPC_TakeDamage), view.Owner, damage);  
    }
    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        Debug.Log("took damage " + damage);
        GetComponent<AudioSource>().PlayOneShot(damagesound);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            deathscreen.gameObject.SetActive(true);
            Death();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        view = GetComponent<PhotonView>();
        if (other.gameObject.tag == "Death" && view.IsMine)
        {
            GetComponent<AudioSource>().PlayOneShot(damagesound);
            currentHealth -= 5;
        }
    }
    [PunRPC]
    private void DeathEffect()
    {
        StartCoroutine(ShrinkAndDie());
    }

    private IEnumerator ShrinkAndDie()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
    }
    private void Death()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        isDead = true; // блокування всіх дій!
        crosshair.gameObject.SetActive(false);
        deathscreen.gameObject.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(deathsound);
        photonView.RPC("DeathEffect", RpcTarget.All);
        yield return new WaitForSeconds(1f); // 2 секунди для показу екрану смерті
        playerManager.Die();
        Destroy(gameObject);

        playerManager.Die(); // тут створиться новий PlayerController
        Destroy(gameObject); // старий видаляється
    }

}
