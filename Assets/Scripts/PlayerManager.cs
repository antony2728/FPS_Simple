using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    GameObject controller;

    int kills;
    int deaths;

    public GameObject myCam;
    Transform pointSpawn;

    Transform pointRed;
    Transform pointBlue;

    [SerializeField] GameObject pnlTeam;
    [SerializeField] Text killstx;
    [SerializeField] Text deathstx;

    string team;
    bool die;

    Player player;

    //Datos
    float timerDestruct;
    [SerializeField] float timeD;

    private void Awake()
    {
        pointRed = GameObject.FindGameObjectWithTag("Red").GetComponent<Transform>();
        pointBlue = GameObject.FindGameObjectWithTag("Blue").GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        //pointSpawn = GameObject.FindGameObjectWithTag("Blue").GetComponent<Transform>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            killstx = GameObject.Find("HUD/Leadboard/Kills").GetComponent<Text>();
            deathstx = GameObject.Find("HUD/Leadboard/Deaths").GetComponent<Text>();
            //CreateController();
        }
        else if (!pv.IsMine)
        {
            Destroy(myCam);
            Destroy(pnlTeam);
        }
    }

    private void Update()
    {
        /*if (photonView.IsMine)
        {
            if (die)
            {
                pv.RPC("RPC_Timer", RpcTarget.All);
            }
        }*/
        /*else if (!photonView.IsMine) 
        {
            PhotonNetwork.Destroy(controller);
        }*/

    }

    public void InstanceRed()
    {
        //CreateController();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerRed"), pointRed.position, Quaternion.identity, 0, new object[] { pv.ViewID });
        controller.GetComponent<Weapon>().team = "Red";
        pointSpawn = pointRed;
        team = "Red";
        myCam.SetActive(false);
        pnlTeam.SetActive(false);
    }

    public void InstanceBlue()
    {
        //CreateController();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerBlue"), pointBlue.position, Quaternion.identity, 0, new object[] { pv.ViewID });
        controller.GetComponent<Weapon>().team = "Blue";
        pointSpawn = pointBlue;
        team = "Blue";
        myCam.SetActive(false);
        pnlTeam.SetActive(false);
    }

    void CreateController()
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), pointRed.position, Quaternion.identity, 0, new object[] { pv.ViewID });
    }

    public void Die()
    {
        if (!die)
        {
            deaths++;
            Hashtable hash = new Hashtable();
            hash.Add("deaths", deaths);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            RespawnPlayer();
            /*if (team == "Red")
            {
                InstanceRed();
            }
            else if (team == "Blue")
            {
                InstanceBlue();
            }

            if (team == null)
            {
                CreateController();
            }*/
            //die = true;
        }
    }

    void RespawnPlayer() 
    {
        StartCoroutine(Respawn(4));
    }

    IEnumerator Respawn(float timer) 
    {
        Destroy(controller.GetComponent<Weapon>().currentWeapon);
        controller.GetComponent<Movement>().camPar.SetActive(false);
        PosCam();
        myCam.SetActive(true);
        yield return new WaitForSeconds(timer);
        myCam.SetActive(false);
        PhotonNetwork.Destroy(controller);
        if (team == "Red")
        {
            InstanceRed();
        }
        else if (team == "Blue")
        {
            InstanceBlue();
        }

        if (team == null)
        {
            CreateController();
        }
    }

    void PosCam() 
    {
        Transform newPos = controller.transform.GetChild(0).GetComponent<Transform>();
        myCam.transform.position = newPos.position;
        myCam.transform.rotation = newPos.rotation;
    }

    public void GetKill()
    {
        pv.RPC(nameof(RPC_GetKill), pv.Owner);
    }

    public void ActiveTimer() 
    {
        pv.RPC(nameof(RPC_Timer), pv.Owner);
    }

    [PunRPC]
    void RPC_Timer() 
    {
        //if (photonView.IsMine) 
        //{
            timerDestruct += Time.deltaTime;
            if (timerDestruct >= timeD)
            {
                PhotonNetwork.Destroy(controller);
                if (team == "Red")
                {
                    InstanceRed();
                }
                else if (team == "Blue")
                {
                    InstanceBlue();
                }

                /*if (team == null)
                {
                    CreateController();
                }*/
                die = false;
            }
        //}

    }

    [PunRPC]
    void RPC_GetKill() 
    {
        kills++;
        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player) 
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pv.Owner == player);
    }


    public void Initialize(Player player)
    {
        this.player = player;
        UpdateStates();
    }


    void UpdateStates()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killstx.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deahts))
        {
            deathstx.text = deahts.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                UpdateStates();
            }
        }
    }
}
