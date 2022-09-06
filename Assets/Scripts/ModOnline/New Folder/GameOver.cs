using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static bool paused = false;

    bool disconnecting = false;

    MatchController matchController;

    private void Start()
    {
        matchController = GameObject.FindGameObjectWithTag("Controller").GetComponent<MatchController>();
    }

    private void Update()
    {
        if (matchController.overTime == true) 
        {
            OverTime();
        }
    }

    public void OverTime()
    {
        if (disconnecting) return;


        transform.GetChild(0).gameObject.SetActive(paused);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitMatch()
    {
        disconnecting = true;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("NewMenu");
    }
}
