using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGeneral : MonoBehaviour
{
    [SerializeField] string nameScene;


    public void CargarScena() 
    {
        SceneManager.LoadScene(nameScene);
    }
}
