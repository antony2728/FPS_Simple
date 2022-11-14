using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

[System.Serializable]
public class ProfileData
{
    public int level;
    public int xp;

    public ProfileData()
    {
        this.level = 0;
        this.xp = 0;
    }

    public ProfileData(int l, int x)
    {
        this.level = l;
        this.xp = x;
    }
}
public class NewLauncher : MonoBehaviourPunCallbacks
{

    public static ProfileData myPlayer = new ProfileData();


    public static NewLauncher instance;

    [SerializeField] InputField nameRoom;
    [SerializeField] Text txError;
    [SerializeField] Text txNameRoom;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;

    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;

    [SerializeField] GameObject startButton;

    [SerializeField] Text txCarg;

    public GameObject pnlActive;

    [SerializeField] GameObject topUI;

    [SerializeField] GameObject pnlCarga;
    [SerializeField] GameObject pnlMenu;
    [SerializeField] GameObject pnlRoom;
    [SerializeField] GameObject pnlCreacion;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        txCarg.text = "Entrando a la red";
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        txCarg.text = "Conectando a Servidores";
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        //usernameFile.text = myPlayer.username;

    }

    public override void OnJoinedLobby()
    {
        txCarg.text = "¡Todo Listo!";
        //PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
        pnlCarga.SetActive(false);
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(nameRoom.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(nameRoom.text);
        pnlCarga.SetActive(true);
        pnlMenu.SetActive(false);
        pnlCreacion.SetActive(false);
        topUI.SetActive(false);
        //MenuManager.instance.OpenMenu("Cargando");
    }

    public override void OnJoinedRoom()
    {
        //MenuManager.instance.OpenMenu("Room");
        pnlMenu.SetActive(false);
        pnlCarga.SetActive(false);
        pnlRoom.SetActive(true);

        txNameRoom.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent) 
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        txError.text = "No se pudo crear o entrar a sala: " + message;
        //MenuManager.instance.OpenMenu("Error");
    }

    public void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
        pnlRoom.SetActive(false);

        pnlCarga.SetActive(true);
        topUI.SetActive(true);
        //MenuManager.instance.OpenMenu("Cargando");
    }

    public void JoinRoom(RoomInfo info) 
    {
        PhotonNetwork.JoinRoom(info.Name);
        pnlCarga.SetActive(true);
        //VerifyUsername();

        //MenuManager.instance.OpenMenu("Cargando");
    }

    public void StartGame() 
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnLeftRoom() 
    {
        pnlMenu.SetActive(true);
        //MenuManager.instance.OpenMenu("Menu");

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent) 
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItemPrefab>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
