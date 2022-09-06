using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItemPrefab : MonoBehaviour
{
    [SerializeField] Text tx;

    [SerializeField] GameObject top;

    public RoomInfo roomInfo;
    NewLauncher launcher;


    private void Start()
    {
        launcher = GameObject.FindGameObjectWithTag("Launcher").GetComponent<NewLauncher>();
        top = GameObject.FindGameObjectWithTag("Top");
    }

    public void SetUp(RoomInfo _info) 
    {
        roomInfo = _info;
        tx.text = _info.Name;
    }

    public void OnClick() 
    {
        NewLauncher.instance.JoinRoom(roomInfo);
        launcher.pnlActive.SetActive(false);
        launcher.pnlActive = null;
        top.SetActive(false);
    }
}
