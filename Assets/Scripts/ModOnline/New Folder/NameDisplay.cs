using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class NameDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameSup;
    [SerializeField] PhotonView pv; 

    private void Start()
    {

        nameSup.text = pv.Owner.NickName;
    }
}
