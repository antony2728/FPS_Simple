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


    [SerializeField] Text killstx;
    [SerializeField] Text deathstx;

    Player player;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine) 
        {
            killstx = GameObject.Find("HUD/Leadboard/Kills").GetComponent<Text>();
            deathstx = GameObject.Find("HUD/Leadboard/Deaths").GetComponent<Text>();
            CreateController();
        }
    }

    void CreateController() 
    {
        
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity, 0, new object[] { pv.ViewID });
    }

    public void Die() 
    {
        PhotonNetwork.Destroy(controller);
        CreateController();

        deaths++;
        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void GetKill()
    {
        pv.RPC(nameof(RPC_GetKill), pv.Owner);
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
