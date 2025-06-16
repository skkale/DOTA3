using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using static Unity.VisualScripting.Member;

public class NyanShotGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView view;
    //[SerializeField] GameObject tracerPrefab;
    public Transform muzzlePoint;
    public GameObject muzzleFlash;
    public float spreadAngle = 0.2f;
    private int shotCount = 0;
    public GameObject nyanCatProjectilePrefab;
    public float projectileSpeed = 40f;


    void Awake()
    {
        view = GetComponent<PhotonView>();

    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            shotCount = 0;
            view.RPC("MuzzleFlashOff", RpcTarget.All);
        }
    }
    public override void Use()
    {
        Shoot();
    }
    void Shoot()
    {
        Vector3 direction;
        if (shotCount == 0)
        {
            // ѕерший вистр≥л Ч ≥деально по центру
            direction = cam.transform.forward;
        }
        else
        {
            // ≤нш≥ Ч з розбросом
            direction = cam.transform.forward;

            // ƒодаЇмо невеликий розброс
            direction = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle), // X Ч вертикальний розброс
                Random.Range(-spreadAngle, spreadAngle), // Y Ч горизонтальний розброс
                0) * direction;
        }
        shotCount++;


        Ray ray = new Ray(cam.transform.position, direction);

        view.RPC("MuzzleFlashOn", RpcTarget.All);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            view.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);

            // “расер з ймов≥рн≥стю 30%
            // if (Random.value < 0.3f)
            //    CreateTracer(muzzlePoint.position, hit.point);
        }
        Vector3 start = muzzlePoint.position;
        Vector3 dir = cam.transform.forward;
        view.RPC("ThrowTracer", RpcTarget.All, start, dir);
    }
    [PunRPC]
    void ThrowTracer(Vector3 start, Vector3 dir)
    {
        GameObject proj = Instantiate(nyanCatProjectilePrefab, start, Quaternion.LookRotation(dir));
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.velocity = dir * projectileSpeed;
        Destroy(proj, 3f);
    }
    //void CreateTracer(Vector3 from, Vector3 to)
    //{
    //    GameObject tracer = Instantiate(tracerPrefab, from, Quaternion.identity);
    //    LineRenderer lr = tracer.GetComponent<LineRenderer>();
    //    if (lr != null)
    //    {
    //        lr.SetPosition(0, from);
    //        lr.SetPosition(1, to);
    //    }
    //    Destroy(tracer, 0.2f); // ∆иве 0.2 сек, пот≥м зникаЇ
    //}
    public override void StartFireEffect()
    {
        view.RPC("PlayMusic", RpcTarget.All);
    }
    public override void StopFireEffect()
    {
        view.RPC("StopMusic", RpcTarget.All);
    }

    [PunRPC]
    void MuzzleFlashOn()
    {
        float randomZ = Random.Range(0f, 360f);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 90, randomZ);
        muzzleFlash.SetActive(true);
    }
    [PunRPC]
    void MuzzleFlashOff()
    {
        muzzleFlash.SetActive(false);
    }
    [PunRPC]
    void PlayMusic()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = ((GunInfo)itemInfo).fire;
        if (!source.isPlaying)
        {
            source.loop = true;
            source.Play();
        }
    }

    [PunRPC]
    void StopMusic()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source.isPlaying)
        {
            source.Stop();
        }
    }


    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 15f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }

    }

}