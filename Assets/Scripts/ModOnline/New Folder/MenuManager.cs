using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject myPnl;
    NewLauncher launcher;

    private void Start()
    {
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
    }
}
