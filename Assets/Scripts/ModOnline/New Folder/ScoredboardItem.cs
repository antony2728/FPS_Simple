using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoredboardItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text usernametx;
    [SerializeField] Text killstx;
    [SerializeField] Text deathstx;

    Player player;

    public void Initialize(Player player) 
    {
        usernametx.text = player.NickName;
        this.player = player;
        UpdateStates();
    }


    void UpdateStates() 
    {
        if(player.CustomProperties.TryGetValue("kills", out object kills))
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

