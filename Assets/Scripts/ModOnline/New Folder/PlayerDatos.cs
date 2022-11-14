using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerDatos : MonoBehaviour
{
    bool partFinal;
    int levelinicial = 0;

    [SerializeField] string level;
    float exp;
    float expTotal;

    [SerializeField] Text uiLevel;
    [SerializeField] Image uiExp;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            uiLevel.text = PlayerPrefs.GetString("Level");
            PhotonNetwork.Level = PlayerPrefs.GetString("Level");
        }
        else
        {
            uiLevel.text = levelinicial.ToString();
            SaveDatos();
        }
    }

    private void Update()
    {
        RefreshUI();
    }

    void SaveDatos()
    {
        PhotonNetwork.Level = uiLevel.text;
        PlayerPrefs.SetString("Level", uiLevel.text);
    }

    void RefreshUI()
    {
        exp = Mathf.Clamp(exp, 0, expTotal);
        uiExp.fillAmount = exp / expTotal;
    }
}
