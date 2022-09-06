using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Looking : MonoBehaviourPunCallbacks
{
    public static bool cursorLocked = true;
    public Transform weapon;

    [SerializeField] Transform player;
    [SerializeField] Transform cam;
    Quaternion camCenter;

    public float xSens;
    public float ySens;
    public float maxAngle;

    // Start is called before the first frame update
    void Start()
    {
        camCenter = cam.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (Pause.paused) return;

        SetY();   
        SetX();
        UpdateCursorLock();
    }

    void SetY() 
    {
        float input = Input.GetAxisRaw("Mouse Y") * ySens * Time.deltaTime;
        Quaternion adj = Quaternion.AngleAxis(input, -Vector3.right);
        Quaternion delta = cam.localRotation * adj;

        if (Quaternion.Angle(camCenter, delta) < maxAngle) 
        {
            cam.localRotation = delta;
        }

        weapon.rotation = cam.rotation;
    }

    void SetX()
    {
        float input = Input.GetAxisRaw("Mouse X") * xSens * Time.deltaTime;
        Quaternion adj = Quaternion.AngleAxis(input, Vector3.up);
        Quaternion delta = player.localRotation * adj;
        player.localRotation = delta;
    }

    void UpdateCursorLock() 
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false;
            }
        }
        else 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true;
            }
        }
    }
}
