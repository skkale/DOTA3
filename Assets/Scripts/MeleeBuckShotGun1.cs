using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class MeleeBuckShotGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView view;
    public GameObject muzzleFlash;
    private float time;
    private bool canShoot = false;
    public float distance = 4;

    void Awake(){
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if ((time += Time.deltaTime) > 0.5f)
        {
            time = 0.0f;
            canShoot = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //muzzleFlash.SetActive(false);
        }
    }

    public override void Use()
    {
        if (canShoot)
        {
            Shoot();
            canShoot = false;
        }
    }
    [PunRPC]
    void Shoot()
    {
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = cam.transform.position;
                if (Physics.Raycast(ray, out RaycastHit hit, distance))
                {
                    view.RPC("HitSound", RpcTarget.All);
                    hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                     view.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
        else
        {
            view.RPC("MissSound", RpcTarget.All);
        }
    }
    [PunRPC]
    void HitSound()
    {
     GetComponent<AudioSource>().PlayOneShot(((GunInfo)itemInfo).fire);
     //muzzleFlash.SetActive(true);
    }
    [PunRPC]
    void MissSound()
    {
        GetComponent<AudioSource>().PlayOneShot(((GunInfo)itemInfo).miss);
    }

        [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal){
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0){
           GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 30f);
           bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
        
    }
}
