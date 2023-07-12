using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class first_person_contorller : MonoBehaviourPunCallbacks, IDamagable
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
    public GameObject CameraHolder;
    public first_person_contorller scriptPlayerController;

    [SerializeField] GameObject ui;

    [SerializeField] Item[] items;

    int itemIndex;
    int previousItemIndex = -1;

    const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    PlayerManager playerManager;
    public Image bar;
   
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheckObject = GameObject.FindGameObjectWithTag("GroundCheck").transform;

        view = GetComponent<PhotonView>(); 
        if (!view.IsMine)
        {
            Destroy(ui);
            Camera.SetActive(false);
            scriptPlayerController.enabled = false;
        }

        playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            EquipItem(0);
        }
    }
    private void Update()
    {
        velocity.x = Input.GetAxis("Horizontal") * mvSpeed * Time.deltaTime;   // пересування, керування
        velocity.y = Input.GetAxis("Vertical") * mvSpeed * Time.deltaTime;

        transform.Translate(velocity.x, 0f, velocity.y);

        isground = Physics.Raycast(_groundCheckObject.transform.position, Vector3.down, maxGroundDistance);

        if (Input.GetButtonDown("Jump") && isground)
            _rigidbody.AddForce(Vector3.up * 100 * jumpStrength);

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
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

    private void OnTriggerEnter(Collider other)
    {
        view = GetComponent<PhotonView>();
        if (other.gameObject.tag == "Death" && view.IsMine)
        {
            currentHealth -= 5;
        }
    }                                                   // закінчується тута

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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) // цей скрипт не працює, а має синхронізувати зброю в руках
    {
        if (!view.IsMine && targetPlayer == view.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage)     // механіка стрільби
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
    private void Death()
        {
        playerManager.Die();
        }

}
