using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    Dictionary<Player, ScoredboardItem> scoreboardItems = new Dictionary<Player, ScoredboardItem> ();

    private void Start()
    {

        foreach (Player player in PhotonNetwork.PlayerList) 
        {
            AddItem(player);
        }
    }

    private void AddItem(Player player)
    {
        ScoredboardItem item = Instantiate(scoreboardPrefab, container).GetComponent<ScoredboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item; 
    }

    public override void OnPlayerEnteredRoom(Player newplayer)
    {
        AddItem(newplayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovedItem(otherPlayer);
    }

    public void RemovedItem(Player player) 
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvasGroup.alpha = 1;
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            canvasGroup.alpha = 0;
        }
    }
}
