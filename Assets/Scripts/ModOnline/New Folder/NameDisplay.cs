using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class NameDisplay : MonoBehaviour
{
    Text myName;
    [SerializeField] TMP_Text nameSup;
    [SerializeField] PhotonView pv; 

    private void Start()
    {
        myName = GameObject.Find("HUD/Username/Text").GetComponent<Text>();

        nameSup.text = pv.Owner.NickName;
        myName.text = pv.Owner.NickName;
    }
}
