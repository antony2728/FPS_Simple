using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGun : MonoBehaviour
{
    [SerializeField] ControllerPlayer player;

    public int type;
    public int weapon;

    ControllerPlayer controllerPlayer;

    private void Start()
    {
        controllerPlayer = GameObject.FindGameObjectWithTag("Launcher").GetComponent<ControllerPlayer>();
    }

    public void Change() 
    {
        if (type == 1)
        {
            player.primaria = weapon;
            PlayerPrefs.SetInt("Pri", player.primaria);
            controllerPlayer.ChangePrimary();
        }
        else if(type == 2)
        {
            player.secundaria = weapon;
            PlayerPrefs.SetInt("Sec", player.secundaria);
            controllerPlayer.ChangeSecundary();
        }
    }
}
