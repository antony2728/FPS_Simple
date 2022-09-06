using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public gun[] loadaut;
    public Transform gunParent;
    public GameObject bulletHolePrefab;
    public LayerMask canBeShot;
    public bool isAim = false;

    float currentCooldown;
    int currentIndex;
    GameObject currentWeapon;
    [SerializeField] bool isReloading = false;
    Image imgGun;

    //new 
    int ammoCant;
    int ammoTotal;
    int ammoGas;


    void Start()
    {
        imgGun = GameObject.Find("HUD/Ammo/ImageGun").GetComponent<Image>();
        //foreach (gun a in loadaut) a.Initialize();
        Equip(0);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (loadaut.Length > 1)
            {
                Equip(1);
            }
            else
            {

            }
        }

        if (currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            if (loadaut[currentIndex].burst != 1)
            {
                if (Input.GetMouseButtonDown(0) && currentCooldown <= 0)
                {

                    if (ammoCant > 0)
                    {
                        Shoot();
                    }
                    else
                    {
                        if (isReloading == false) 
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

                    if (ammoCant > 0)
                    {
                        Shoot();
                    }
                    else
                    {
                        if (isReloading == false)
                        {
                            StartCoroutine(Reload(loadaut[currentIndex].reload));
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (isReloading == false) 
                {
                    StartCoroutine(Reload(loadaut[currentIndex].reload));
                }
            }

            if (currentCooldown > 0)
                currentCooldown -= Time.deltaTime;

        }
        currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
    }

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

        currentIndex = p_id;

        GameObject newWeapon = Instantiate(loadaut[p_id].prefab, gunParent.position, gunParent.rotation, gunParent) as GameObject;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localEulerAngles = Vector3.zero;

        ///new
        ammoCant = loadaut[p_id].clipSize;
        ammoTotal = loadaut[p_id].ammo;

        newWeapon.GetComponent<Animator>().Play("Equip", 0, 0);
        imgGun.sprite = loadaut[p_id].sprGun;
        currentWeapon = newWeapon;

    }

    IEnumerator Reload(float timer)
    {
        isReloading = true;
        if (currentWeapon.GetComponent<Animator>())
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
        if (ammoTotal >= ammoGas)
        {
            ammoCant += ammoGas;
            ammoTotal -= ammoGas;
            ammoGas = 0;
        }
        else if (ammoTotal < ammoGas)
        {
            ammoCant += ammoTotal;
            ammoGas -= ammoTotal;
            ammoTotal = 0;
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

    void Shoot()
    {
        if (isReloading == false) 
        {
            Transform spawn = transform.Find("Cameras/Camera");
            

            //Bloom
            Vector3 bloom = spawn.position + spawn.forward * 1000f;
            bloom += Random.Range(-loadaut[currentIndex].bloom, loadaut[currentIndex].bloom) * spawn.up;
            bloom += Random.Range(-loadaut[currentIndex].bloom, loadaut[currentIndex].bloom) * spawn.right;
            bloom -= spawn.position;
            bloom.Normalize();

            //CoolDown
            currentCooldown = loadaut[currentIndex].firate;

            //Rest
            ammoCant -= 1;
            ammoGas += 1;

            //Raycast
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(spawn.position, bloom, out hit, 1000f, canBeShot))
            {
                GameObject newHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                newHole.transform.LookAt(hit.point + hit.normal);
                Destroy(newHole, 5f);
                
            }

            //Gun fx
            currentWeapon.transform.Rotate(-loadaut[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadaut[currentIndex].kickback;
        }
    }


    public void RefreshAmmo(Text txt)
    {
        int ammoEquip = ammoCant;
        int ammoInTotal = ammoTotal;

        txt.text = ammoEquip.ToString() + " / " + ammoTotal.ToString();
    }
}
