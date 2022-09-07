using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject myPnl;
    NewLauncher launcher;

    Animator anim;
    ControllerPlayer controllerPlayer;

    public int type;

    private void Start()
    {
        controllerPlayer = GameObject.FindGameObjectWithTag("Launcher").GetComponent<ControllerPlayer>();
        anim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        launcher = GameObject.FindGameObjectWithTag("Launcher").GetComponent<NewLauncher>();
    }

    public void Click() 
    {
        if (myPnl.activeInHierarchy == false) 
        {
            if (launcher.pnlActive != null)
            {
                launcher.pnlActive.SetActive(false);
                myPnl.SetActive(true);
                launcher.pnlActive = myPnl;
            }
            else if(launcher.pnlActive == null)
            {
                myPnl.SetActive(true);
                launcher.pnlActive = myPnl;
            }

        }
        else if (myPnl.activeInHierarchy == true)
        {
            myPnl.SetActive(false);
            launcher.pnlActive = null;
        }


        if(type == 1)
        {
            if(controllerPlayer.mov == true)
            {
                anim.Play("MovStart", 0, 0);
                controllerPlayer.mov = false;
            }
        }
    }
}
