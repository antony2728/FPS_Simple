using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

/*public class PlayerInfo 
{
    public ProfileData profile;
    public int actor;
    public short kills;
    public short deaths;

    public PlayerInfo(ProfileData p, int a, short k, short d) 
    {
        this.profile = p;
        this.actor = a;
        this.kills = k;
        this.deaths = d;   
    }

}*/

public class Manager : MonoBehaviour //IOnEventCallback
{
    public string playerPrefabString;
    public string playerManagerString;
    public GameObject playerPrefab;
    public GameObject prefabManager;
    public Transform[] spawnsPos;
    public PhotonView pv;

    //public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
    public int myID;

    public int typeGame; // free for all = 0, time death match = 1

    Text uiKills;
    Text uiDeaths;


    [SerializeField] GameObject cnvMan;

    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeState
    }


    private void Start()
    {
            Spawn();
            ValideteConnection();
    }


    public void OnEneble() 
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void Spawn() 
    {
        Transform spawnSlect = spawnsPos[Random.Range(0, spawnsPos.Length)];

        if (PhotonNetwork.IsConnected) 
        {
            if (typeGame == 0)
            {
                PhotonNetwork.Instantiate(playerPrefabString, spawnSlect.position, spawnSlect.rotation);
            }
            else if (typeGame == 1) 
            {
                PhotonNetwork.Instantiate(playerManagerString, transform.position, Quaternion.identity);
            }
        }
        else {
            GameObject newplayer = Instantiate(playerPrefab, spawnSlect.position, spawnSlect.rotation) as GameObject;
        }
    }




    void ValideteConnection() 
    {
        if (PhotonNetwork.IsConnected) return;
    }

 
}
