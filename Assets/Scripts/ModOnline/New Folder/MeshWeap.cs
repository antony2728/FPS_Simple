using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeshWeap : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject[] meshs;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            foreach (GameObject meshChange in meshs) 
            {
                meshChange.GetComponent<Transform>().gameObject.layer = 11;
            }
        }
    }
}
