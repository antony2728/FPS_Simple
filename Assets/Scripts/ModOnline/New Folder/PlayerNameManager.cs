using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] InputField usrnameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            usrnameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else 
        {
            usrnameInput.text = "Player " + Random.Range(0, 9999).ToString("0000");
            OnUsernameInputChanged();
        }
    }

    public void OnUsernameInputChanged() 
    {
        PhotonNetwork.NickName = usrnameInput.text;
    }
}
