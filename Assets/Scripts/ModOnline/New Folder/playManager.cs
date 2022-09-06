using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Realtime;

public class playManager : MonoBehaviourPunCallbacks
{

    Transform pointBlue;
    Transform pointRed;

    //[SerializeField] PhotonView view;


    public int id;

    Transform cnv;

    public string playerString;

    private void Start()
    {
        cnv = GameObject.FindGameObjectWithTag("CanvasPlayer").GetComponent<Transform>();
        ValideteConnection();
        pointRed = GameObject.FindGameObjectWithTag("Red").GetComponent<Transform>();
        pointBlue = GameObject.FindGameObjectWithTag("Blue").GetComponent<Transform>();
    }


    public void SpawnBlue()
    {
        if (PhotonNetwork.IsConnected) 
        {
          
                PhotonNetwork.Instantiate(playerString, pointBlue.position, pointBlue.rotation);
                cnv.GetChild(0).gameObject.SetActive(true);

                Destroy(gameObject);
            

        }
    }

    public void SpawnRed()
    {
        if (PhotonNetwork.IsConnected)
        {
          
                PhotonNetwork.Instantiate(playerString, pointRed.position, pointRed.rotation);
                cnv.GetChild(0).gameObject.SetActive(true);

                Destroy(gameObject);
            

        }
    }


    void ValideteConnection()
    {
        if (PhotonNetwork.IsConnected) return;
        //SceneManager.LoadScene("MenuOnline");
    }
}
