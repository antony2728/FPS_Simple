using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPlayer : MonoBehaviour
{
    public int primaria;
    public int secundaria;

    public Transform transPri;
    public Transform transSec;

    public Animator anim;

    public bool mov = false;

    [SerializeField] GameObject[] weapons;

    [SerializeField] Sprite[] sprWeap;
    [SerializeField] Image primary;
    [SerializeField] Image secondary;

    private void Start()
    {
        primaria = PlayerPrefs.GetInt("Pri");
        secundaria = PlayerPrefs.GetInt("Sec");

        ChangePrimary();
        ChangeSecundary();  
    }


    public void ChangePrimary()
    {
        /*if (transPri.GetChild(0) != null) 
        {
            Destroy(transPri.GetChild(0).gameObject);
        }
        GameObject newWeap = Instantiate(weapons[primaria], transPri.position, transPri.rotation);
        newWeap.transform.SetParent(transPri);
        newWeap.transform.localScale = new Vector3(1, 1, 1);*/

        primary.sprite = sprWeap[primaria];
    }

    public void ChangeSecundary()
    {
        /*if (transSec.GetChild(0) != null)
        {
            Destroy(transSec.GetChild(0).gameObject);
        }
        GameObject newWeap = Instantiate(weapons[secundaria], transSec.position, transSec.rotation);
        newWeap.transform.SetParent(transSec);
        newWeap.transform.localScale = new Vector3(1, 1, 1);*/
        secondary.sprite = sprWeap[secundaria];
    }

    public void CamMov()
    {
        if (!mov)
        {
            anim.Play("MovWeapons", 0, 0);
            mov = true;
        }
        else
        {
            anim.Play("MovStart", 0, 0);
            mov = false;
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
