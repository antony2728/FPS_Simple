using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MatchController : MonoBehaviourPunCallbacks
{
    bool startTimer = false;
    public double timerIncrementValue;
    double startTime;

    public Text seconds;
    public Text minutes;

    [SerializeField] double timer;
    ExitGames.Client.Photon.Hashtable CustomValue;

    public double timeMatch;
    public bool overTime = false;

    private void Awake()
    {

    }

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CustomValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            CustomValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomValue);
        }
        else
        {
            startTime = PhotonNetwork.Time;
            startTimer = true;
        }
    }

    void Update()
    {

        if (!startTimer) return;

        
        timerIncrementValue = PhotonNetwork.Time - startTime;
        seconds.text = timerIncrementValue.ToString("f0");

        if (timerIncrementValue >= timeMatch) 
        {
            overTime = true;
        }

    }

        /*public float timer;

        public Text tx;


        private void Update()
        {
            tx.text = timer.ToString("f0");


            if (PhotonNetwork.IsMasterClient == true) 
            {
                photonView.RPC("RPC_ControlTimer", RpcTarget.All);
            }
        }

        [PunRPC]
        public void RPC_ControlTimer() 
        {
            timer += Time.deltaTime;
        }*/
    }
