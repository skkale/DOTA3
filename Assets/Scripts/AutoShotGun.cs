using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;

public class AutoShotGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject tracerPrefab;
    public Transform muzzlePoint;
    PhotonView view;
    public GameObject muzzleFlash;
    public float spreadAngle = 2f;
    private int shotCount = 0;

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
            if (Random.value < 0.3f)
                view.RPC("CreateTracer", RpcTarget.All, muzzlePoint.position, hit.point);
        }
    }
    [PunRPC]
    void CreateTracer(Vector3 from, Vector3 to)
    {
        GameObject tracer = Instantiate(tracerPrefab, from, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, from);
            lr.SetPosition(1, to);
        }
        Destroy(tracer, 0.2f); // ∆иве 0.2 сек, пот≥м зникаЇ
    }

    [PunRPC]
    void MuzzleFlashOn()
    {
        GetComponent<AudioSource>().PlayOneShot(((GunInfo)itemInfo).fire);
        muzzleFlash.SetActive(true);
    }
    [PunRPC]
    void MuzzleFlashOff()
    {
        muzzleFlash.SetActive(false);
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