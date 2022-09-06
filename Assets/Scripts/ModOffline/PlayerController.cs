using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public int maxHtl;
    public float vel;
    public float sprintVel;
    public Camera myCam;
    public Transform waeponPar;
    public float jumpFor;
    public LayerMask maskGround;
    public Transform pointFloor;
    public GameObject camPar;
    public float lenghtOfSlide;
    public float slideModifier;

    Vector3 targetWeponBobPos;
    Rigidbody rb;
    float baseOF;
    Vector3 weapOr;
    float sprintOF = 1.25f;
    float movementCounter;
    float idleCounter;
    int currentHtl;
    Transform uiHtl;
    WeaponController weap;
    Text uiAmmo;
    bool sliding;
    float slideTime;
    Vector3 slideDir;
    Vector3 origin;
    Vector3 weaponParentCurrentPos;



    private void Start()
    {
        weap = GetComponent<WeaponController>();
        currentHtl = maxHtl;


        baseOF = myCam.fieldOfView;
        origin = myCam.transform.localPosition;

        if (Camera.main)
        {
            Camera.main.enabled = false;
        }
        rb = GetComponent<Rigidbody>();
        weapOr = waeponPar.localPosition;
        weaponParentCurrentPos = weapOr;


        uiHtl = GameObject.Find("HUD/Health/Bar").transform;
        uiAmmo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();

        RefreshBar();
    }

    private void Update()
    {



        float HMOVE = Input.GetAxisRaw("Horizontal");
        float VMOVE = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        bool isGRounded = Physics.Raycast(pointFloor.position, Vector3.down, 0.1f, maskGround);
        bool isJumping = jump && isGRounded && !sliding;
        bool isSprinting = sprint && VMOVE > 0 && !isJumping && isGRounded;

        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpFor);
        }



        if (sliding)
        {
            HeadBo(movementCounter, 0.15f, 0.075f);
            waeponPar.localPosition = Vector3.Lerp(waeponPar.localPosition, targetWeponBobPos, Time.deltaTime * 6f);
        }
        else if (HMOVE == 0 && VMOVE == 0)
        {
            HeadBo(idleCounter, .025f, .025f);
            idleCounter += Time.deltaTime;
            waeponPar.localPosition = Vector3.Lerp(waeponPar.localPosition, targetWeponBobPos, Time.deltaTime * 2f);
        }
        else if (!isSprinting)
        {
            HeadBo(movementCounter, .035f, .035f);
            movementCounter += Time.deltaTime * 3;
            waeponPar.localPosition = Vector3.Lerp(waeponPar.localPosition, targetWeponBobPos, Time.deltaTime * 6f);
        }
        else
        {
            HeadBo(movementCounter, .15f, .075f);
            movementCounter += Time.deltaTime * 5;
            waeponPar.localPosition = Vector3.Lerp(waeponPar.localPosition, targetWeponBobPos, Time.deltaTime * 10f);
        }

        RefreshBar();
        weap.RefreshAmmo(uiAmmo);
    }

    private void FixedUpdate()
    {
        float HMOVE = Input.GetAxisRaw("Horizontal");
        float VMOVE = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool slide = Input.GetKey(KeyCode.C);

        bool isGRounded = Physics.Raycast(pointFloor.position, Vector3.down, 0.1f, maskGround);
        bool isJumping = jump && isGRounded && !sliding;
        bool isSprinting = sprint && VMOVE > 0 && !isJumping && isGRounded;
        bool isSliding = isSprinting && slide && !sliding;

        Vector3 MOVE_DIR = Vector3.zero;
        float adjustedSpeed = vel;

        if (!sliding)
        {
            MOVE_DIR = new Vector3(HMOVE, 0, VMOVE);
            MOVE_DIR.Normalize();
            MOVE_DIR = transform.TransformDirection(MOVE_DIR);

            if (isSprinting)
                adjustedSpeed *= sprintVel;
        }
        else
        {
            MOVE_DIR = slideDir;
            adjustedSpeed *= slideModifier;
            slideTime -= Time.deltaTime;
            if (slideTime <= 0)
            {
                sliding = false;
                weaponParentCurrentPos += Vector3.up * 0.5f;
            }
        }

        Vector3 targetVel = MOVE_DIR * adjustedSpeed * Time.deltaTime;
        targetVel.y = rb.velocity.y;
        rb.velocity = targetVel;

        if (isSliding)
        {
            sliding = true;
            slideDir = MOVE_DIR;
            slideTime = lenghtOfSlide;
            weaponParentCurrentPos += Vector3.down * 0.5f;
        }

        if (sliding)
        {
            myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, baseOF * sprintOF * 1.25f, Time.deltaTime * 8);
            myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, origin + Vector3.down * 0.5f, Time.deltaTime * 6f);
        }
        else
        {
            if (isSprinting)
            {
                myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, baseOF * sprintOF, Time.deltaTime * 8);
            }
            else
            {
                myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, baseOF, Time.deltaTime * 8);
            }
            myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, origin, Time.deltaTime * 6f);
        }
    }

    void RefreshBar()
    {
        float healthBarRatio = (float)currentHtl / (float)maxHtl;
        uiHtl.localScale = Vector3.Lerp(uiHtl.localScale, new Vector3(healthBarRatio, 1, 1), Time.deltaTime * 8f);
    }

    void HeadBo(float pz, float px, float py)
    {
        float aimAdjust = 1f;
        if (weap.isAim)
        {
            aimAdjust = 0.1f;
        }
        targetWeponBobPos = weaponParentCurrentPos + new Vector3(Mathf.Cos(pz) * px * aimAdjust, Mathf.Sin(pz * 2) * py * aimAdjust, 0);
    }

}
