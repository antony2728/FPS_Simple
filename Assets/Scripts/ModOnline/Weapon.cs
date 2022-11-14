
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Weapon : MonoBehaviourPunCallbacks
{
    public List<gun> loadaut;
    public gun currentGunData;
    public Transform gunParent;
    public GameObject bulletHolePrefab;
    public LayerMask canBeShot;
    public bool isAim = false;
    public AudioSource sfx;
    public AudioClip hitMarkerSound;

    bool weap1 = false;
    bool weap2 = false;

    AmmoController ammoController;

    [SerializeField] Transform weapModelTrans;
    GameObject currentModel;

    Image imgGun;
    float currentCooldown;
    int currentIndex;
    public GameObject currentWeapon;
    [SerializeField]bool isReloading = false;

    int ammoEquip;
    int ammoTotal;
    int ammoGas;

    Movement mov;

    Image hitMarkerImg;
    float hitMarkerWait;
    Color CLEARWHITE = new Color(1, 1, 1, 0);

    [SerializeField] Animator anim;

    int p;
    int s;

    public gun[] weapons;
    public string team;
    void Start()
    {
        mov = GetComponent<Movement>();
        imgGun = GameObject.Find("HUD/Ammo/ImageGun").GetComponent<Image>();
        hitMarkerImg = GameObject.Find("HUD/HitMarker/Image").GetComponent<Image>();
        EquipWeapons();
        ammoController = GetComponent<AmmoController>();
        hitMarkerImg.color = new Color(1, 1, 1, 0);
        hitMarkerImg.color = CLEARWHITE;
        //foreach (gun a in loadaut) a.Initialize();
        Equip(0);
        weap1 = true;
        weap2 = false;
    }

    public void EquipWeapons() 
    {
        p = PlayerPrefs.GetInt("Pri");
        s = PlayerPrefs.GetInt("Sec");

        loadaut.Add(weapons[p]);
        loadaut.Add(weapons[s]);
        
    }

    void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.U))
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.All, loadaut[currentIndex].damage);
        }*/

        if (Pause.paused && photonView.IsMine) return;

        if (mov.die == false) 
        {
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("Equip", RpcTarget.All, 0);
                weap1 = true;
                weap2 = false;
            }
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("Equip", RpcTarget.All, 1);
                weap1 = false;
                weap2 = true;
            }

            if (currentWeapon != null)
            {
                if (photonView.IsMine)
                {
                    Aim(Input.GetMouseButton(1));

                    if (loadaut[currentIndex].burst != 1)
                    {
                        if (Input.GetMouseButtonDown(0) && currentCooldown <= 0)
                        {
                            if (ammoEquip > 0)
                            {
                                photonView.RPC("Shoot", RpcTarget.All);
                            }
                            else
                            {
                                if (isReloading == false && ammoGas > 0)
                                {
                                    StartCoroutine(Reload(loadaut[currentIndex].reload));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButton(0) && currentCooldown <= 0)
                        {
                            if (ammoEquip > 0)
                            {
                                photonView.RPC("Shoot", RpcTarget.All);
                            }
                            else
                            {
                                if (isReloading == false && ammoGas > 0)
                                {
                                    StartCoroutine(Reload(loadaut[currentIndex].reload));
                                }
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (isReloading == false && ammoGas > 0)
                        {
                            photonView.RPC("ReloadRPC", RpcTarget.All);
                        }
                    }

                    if (currentCooldown > 0)
                        currentCooldown -= Time.deltaTime;
                }
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            }


            if (photonView.IsMine)
            {
                if (hitMarkerWait > 0)
                {
                    hitMarkerWait -= Time.deltaTime;
                }
                else if (hitMarkerImg.color.a > 0)
                {
                    hitMarkerImg.color = Color.Lerp(hitMarkerImg.color, CLEARWHITE, Time.deltaTime * 2f);
                }
            }
        }
    }

    private void LateUpdate()
    {
        anim.SetBool("Aim", isAim);
    }

    [PunRPC]
    void Equip(int p_id)
    {
        if (currentWeapon != null)
        {
            if (isReloading) 
            {
                StartCoroutine("Reload");
            }
            Destroy(currentWeapon);
        }

        if (currentModel != null) 
        {
            Destroy(currentModel);
        }

        currentIndex = p_id;

        GameObject newWeapon = Instantiate(loadaut[p_id].prefab, gunParent.position, gunParent.rotation, gunParent) as GameObject;
        GameObject model = Instantiate(loadaut[p_id].modelWeap, weapModelTrans.position, weapModelTrans.rotation, weapModelTrans) as GameObject;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = Vector3.zero;
        newWeapon.GetComponent<Sway>().isMine = photonView.IsMine;
        newWeapon.GetComponent<Animator>().Play("Equip", 0, 0);
        imgGun.sprite = loadaut[p_id].sprGun;


        if(ammoController.equip1 == false)
        {
            ammoController.ammoCant1 = loadaut[p_id].clipSize;
            ammoController.ammoTotal1 = loadaut[p_id].ammo;
            ammoController.equip1 = true;
        }
        else
        {
            if(ammoController.equip2 == false)
            {
                ammoController.ammoCant2 = loadaut[p_id].clipSize;
                ammoController.ammoTotal2 = loadaut[p_id].ammo;
                ammoController.equip2 = false;
            }
        }

        currentWeapon = newWeapon;
        currentModel = model;
        currentGunData = loadaut[p_id];
    }


    [PunRPC]
    void ReloadRPC() 
    {
        StartCoroutine(Reload(loadaut[currentIndex].reload));
    }

    IEnumerator Reload(float timer) 
    {
        isReloading = true;

        if(currentWeapon.GetComponent<Animator>())
            currentWeapon.GetComponent<Animator>().Play("Reload", 0, 0);
        else
            currentWeapon.SetActive(true);

        yield return new WaitForSeconds(timer);
        newAmmo();
        currentWeapon.SetActive(true);
        isReloading = false;

    }

    void newAmmo()
    {
        if (weap1)
        {
            if (ammoController.ammoTotal1 >= ammoController.ammoGas1)
            {
                ammoController.ammoCant1 += ammoController.ammoGas1;
                ammoController.ammoTotal1 -= ammoController.ammoGas1;
                ammoController.ammoGas1 = 0;
            }
            else if (ammoController.ammoTotal1 < ammoController.ammoGas1)
            {
                ammoController.ammoCant1 += ammoController.ammoTotal1;
                ammoController.ammoGas1 -= ammoController.ammoTotal1;
                ammoController.ammoTotal1 = 0;
            }
        }
        else if (weap2) 
        {
            if (ammoController.ammoTotal2 >= ammoController.ammoGas2)
            {
                ammoController.ammoCant2 += ammoController.ammoGas2;
                ammoController.ammoTotal2 -= ammoController.ammoGas2;
                ammoController.ammoGas2 = 0;
            }
            else if (ammoController.ammoTotal2 < ammoController.ammoGas2) 
            {
                ammoController.ammoCant2 += ammoController.ammoTotal2;
                ammoController.ammoGas2 -= ammoController.ammoTotal2;
                ammoController.ammoTotal2 = 0;
            }
        }
    }

    void Aim(bool isAiming) 
    {
        isAim = isAiming;
        Transform anchor = currentWeapon.transform.Find("Anchor");
        Transform state_ADS = currentWeapon.transform.Find("States/ADS");
        Transform state_HIP = currentWeapon.transform.Find("States/Hip");

        if (isAiming)
        {
            anchor.position = Vector3.Lerp(anchor.position, state_ADS.position, Time.deltaTime * loadaut[currentIndex].aimSpeed);
        }
        else 
        {
            anchor.position = Vector3.Lerp(anchor.position, state_HIP.position, Time.deltaTime * loadaut[currentIndex].aimSpeed);
        }
    }

    [PunRPC]
    void Shoot()
    {
        if (isReloading == false) 
        {
            Transform spawn = transform.Find("Cameras/Camera");
            //CoolDown
            currentCooldown = loadaut[currentIndex].firate;
            //Rest
            if (weap1 && !weap2)
            {
                ammoController.ammoCant1 -= 1;
                ammoController.ammoGas1 += 1;
                ammoEquip = ammoController.ammoCant1;
            }
            else if(!weap1 && weap2)
            {
                ammoController.ammoCant2 -= 1;
                ammoController.ammoGas2 += 1;
                ammoEquip = ammoController.ammoCant2;
            }

            for (int i = 0; i < Mathf.Max(1, currentGunData.pellets); i++) 
            {
                //Bloom
                Vector3 bloom = spawn.position + spawn.forward * 1000f;
                bloom += Random.Range(-loadaut[currentIndex].bloom, loadaut[currentIndex].bloom) * spawn.up;
                bloom += Random.Range(-loadaut[currentIndex].bloom, loadaut[currentIndex].bloom) * spawn.right;
                bloom -= spawn.position;
                bloom.Normalize();





                //Raycast
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(spawn.position, bloom, out hit, 1000f, canBeShot))
                {
                    GameObject newHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                    newHole.transform.LookAt(hit.point + hit.normal);
                    Destroy(newHole, 5f);

                    if (photonView.IsMine)
                    {
                        if (hit.collider.gameObject.tag == "TeamBlue")
                        {
                            if (gameObject.tag == "TeamRed") 
                            {
                                hit.collider.transform.root.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, loadaut[currentIndex].damage);

                                //hit.collider.transform.root.gameObject.GetComponent<Movement>().TakeDamage(loadaut[currentIndex].damage);
                                hitMarkerImg.color = Color.white;
                                sfx.PlayOneShot(hitMarkerSound);
                                hitMarkerWait = 1f;
                            }
                        }
                        if (hit.collider.gameObject.tag == "TeamRed")
                        {
                            if (gameObject.tag == "TeamBlue") 
                            {
                                hit.collider.transform.root.gameObject.GetPhotonView().RPC("RPC_TakeDamage", RpcTarget.All, loadaut[currentIndex].damage);

                                //hit.collider.transform.root.gameObject.GetComponent<Movement>().TakeDamage(loadaut[currentIndex].damage);
                                hitMarkerImg.color = Color.white;
                                sfx.PlayOneShot(hitMarkerSound);
                                hitMarkerWait = 1f;
                            }
                        }
                        if (hit.collider.gameObject.layer == 8)
                        {
                            hit.collider.transform.root.gameObject.GetComponent<Movement>().TakeDamage(loadaut[currentIndex].damage);
                            hitMarkerImg.color = Color.white;
                            sfx.PlayOneShot(hitMarkerSound);
                            hitMarkerWait = 1f;
                        }
                    }
                }
            }


            //sfx
            sfx.Stop();
            sfx.clip = currentGunData.soundShoot;
            sfx.volume = currentGunData.volume;
            sfx.Play();

            //Gun fx
            currentWeapon.transform.Rotate(-loadaut[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadaut[currentIndex].kickback;
            if(currentGunData.recovery) currentWeapon.GetComponent<Animator>().Play("Recovery", 0, 0);

        }
    }

    [PunRPC]
    void TakeDamage(int damage) 
    {
        GetComponent<Movement>().TakeDamage(damage);
    }


    public void RefreshAmmo(Text txt) 
    {
        if (weap1)
        {
            ammoEquip = ammoController.ammoCant1;
            ammoTotal = ammoController.ammoTotal1;
            ammoGas = ammoController.ammoGas1;
        }
        else if(weap2) 
        {
            ammoEquip = ammoController.ammoCant2;
            ammoTotal = ammoController.ammoTotal2;
            ammoGas = ammoController.ammoGas2;
        }

        txt.text = ammoEquip.ToString("D2") + " / " + ammoTotal.ToString("D2");
    }
}
