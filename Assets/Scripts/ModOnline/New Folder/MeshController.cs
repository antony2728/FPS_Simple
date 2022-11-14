using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeshController : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject[] mesh;
    private void Start()
    {
        if (photonView.IsMine) 
        {
            foreach (GameObject meshChange in mesh)
            {
                meshChange.GetComponent<Transform>().gameObject.layer = 11;
            }
        }
    }
}
