using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaPresentacion : MonoBehaviour
{
    public bool start;

    private void Update()
    {
        if (start) 
        {
            SceneManager.LoadScene("NewMenu");
        }   
    }
}
