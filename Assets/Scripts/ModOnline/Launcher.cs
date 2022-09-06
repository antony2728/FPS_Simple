using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txCarga;
    [SerializeField] GameObject panel;

    public InputField usernameFile;

    public GameObject tabMain;
    public GameObject tabRooms;
    public GameObject roomPrefab;

    List<RoomInfo> roomList;

    public InputField roomName;
    public Slider maxPlayersSlider;
    public Text maxNum;
    public GameObject tabCreate;


    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        txCarga.text = "¡Conectado!" ;
        PhotonNetwork.JoinLobby();
        panel.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        StartGame();

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();
    }

    public void Connect() 
    {
        txCarga.text = "Conectando a los servidores..." ;
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join() 
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create() 
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayersSlider.value;

        ExitGames.Client.Photon.Hashtable propeties = new ExitGames.Client.Photon.Hashtable();
        propeties.Add("Map1", 0);
        options.CustomRoomProperties = propeties;

        PhotonNetwork.CreateRoom(roomName.text, options);
    }

    public void ChangeMap()
    {

    }

    public void ChangeMaxPlayers(float value)
    {
        maxNum.text = Mathf.RoundToInt(value).ToString();
    }

    public void StartGame() 
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) 
        {

            PhotonNetwork.LoadLevel("Map1");
        }
    }

    public void AbrirListaSalas() 
    {
        tabMain.SetActive(false);
        tabRooms.SetActive(true);
    }

    public void CerrarListaSalas() 
    {
        tabMain.SetActive(true);
        tabRooms.SetActive(false);
    }

    public void AbrirCreacion() 
    {
        tabMain.SetActive(false);
        tabCreate.SetActive(true);
    }


    public void CerrarCreacion() 
    {
        tabMain.SetActive(true);
        tabCreate.SetActive(false);
    }

    void ClearRoomList() 
    {
        Transform content = tabRooms.transform.Find("Image/Content");
        foreach (Transform a in content) Destroy(a.gameObject);
    }


    public override void OnRoomListUpdate(List<RoomInfo> room_List)
    {
        roomList = room_List;
        ClearRoomList();

        Transform content = tabRooms.transform.Find("Image/Content");

        foreach (RoomInfo a in roomList) 
        {
            GameObject newRoomButton = Instantiate(roomPrefab, content) as GameObject;
            newRoomButton.transform.Find("Name").GetComponent<Text>().text = a.Name;
            newRoomButton.transform.Find("Players").GetComponent<Text>().text = a.PlayerCount + " / " + a.MaxPlayers;
            newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
        }
        
        base.OnRoomListUpdate(roomList);
    }

    public void JoinRoom(Transform button) 
    {
        string roomName = button.transform.Find("Name").GetComponent<Text>().text;
        PhotonNetwork.JoinRoom(roomName);
    }
}
