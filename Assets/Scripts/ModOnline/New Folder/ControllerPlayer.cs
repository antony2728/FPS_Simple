using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour
{
    public int primaria;
    public int secundaria;

    public Transform transPri;
    public Transform transSec;


    [SerializeField] GameObject[] weapons;

    private void Start()
    {
        primaria = PlayerPrefs.GetInt("Pri");
        secundaria = PlayerPrefs.GetInt("Sec");

        ChangePrimary();
    }


    public void ChangePrimary()
    {
        if (transPri.GetChild(0) != null) 
        {
            Destroy(transPri.GetChild(0));
        }
        GameObject newWeap = Instantiate(weapons[primaria], transPri.position, transPri.rotation);
        newWeap.transform.SetParent(transPri);
    }

}
