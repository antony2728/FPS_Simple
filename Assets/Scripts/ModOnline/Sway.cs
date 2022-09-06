using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sway : MonoBehaviour
{
    public float instns;
    public float smooth;
    public bool isMine;

    Quaternion tarRot;

    void Start()
    {
        tarRot = transform.localRotation;
    }

    void Update()
    {
        if (Pause.paused) return;
        UpdateSway();   
    }

    void UpdateSway()
    {
        float XMOUSE = Input.GetAxis("Mouse X");
        float YMOUSE = Input.GetAxis("Mouse Y");

        if (!isMine) 
        {
            XMOUSE = 0;
            YMOUSE = 0;
        }

        Quaternion x_adj = Quaternion.AngleAxis(-instns * XMOUSE, Vector3.up);
        Quaternion y_adj = Quaternion.AngleAxis(instns * YMOUSE, Vector3.right);

        Quaternion targetR = tarRot * x_adj * y_adj;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetR, Time.deltaTime * smooth);
    }
}
