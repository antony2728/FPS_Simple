using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class Movement : MonoBehaviourPunCallbacks, IPunObservable
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
    [HideInInspector] public ProfileData profileData;
    public TextMeshPro playerUsername;

    Vector3 targetWeponBobPos;
    Rigidbody rb;
    float baseOF;
    Vector3 weapOr;
    float sprintOF = 1.25f;
    float movementCounter;
    float idleCounter;
    int currentHtl;
    Manager manager;
    Transform uiHtl;
    Weapon weap;
    Text uiAmmo;
    bool sliding;
    float slideTime;
    Vector3 slideDir;
    Vector3 origin;
    Vector3 weaponParentCurrentPos;
    Text uiUsername;

    [SerializeField] Animator anim;

    PlayerManager playerManager;

    Vector3 move;
    bool sprintAnim;

    public bool die;

    float aimAngle;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message) 
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)(waeponPar.transform.localEulerAngles.x * 100f));
        }
        else 
        {
            aimAngle = (int)stream.ReceiveNext() / 100f;
        }
    }

    private void Awake()
    {
        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {

        manager = GameObject.Find("Manager").GetComponent<Manager>();
        weap = GetComponent<Weapon>();
        currentHtl = maxHtl;
        camPar.SetActive(photonView.IsMine);

        if (!photonView.IsMine)
        {
            gameObject.layer = 8;
        }

        baseOF = myCam.fieldOfView;
        origin = myCam.transform.localPosition;

        if (Camera.main)
        {
            Camera.main.enabled = false;
        }
        rb = GetComponent<Rigidbody>();
        weapOr = waeponPar.localPosition;
        weaponParentCurrentPos = weapOr;

        if (photonView.IsMine)
        {
            uiHtl = GameObject.Find("HUD/Health/Bar").transform;
            uiAmmo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();

            RefreshBar();

        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            RefresMultiplayerState();
            return;
        }

        if (!die) 
        {
            float HMOVE = Input.GetAxisRaw("Horizontal");
            float VMOVE = Input.GetAxisRaw("Vertical");

            move = new Vector3(HMOVE, 0, VMOVE);

            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKeyDown(KeyCode.Space);
            bool pause = Input.GetKeyDown(KeyCode.Escape);

            bool isGRounded = Physics.Raycast(pointFloor.position, Vector3.down, 0.1f, maskGround);
            bool isJumping = jump && isGRounded && !sliding;
            bool isSprinting = sprint && VMOVE > 0 && !isJumping && isGRounded;

            sprintAnim = sprint;

            if (pause)
            {
                GameObject.Find("Pause").GetComponent<Pause>().TogglePaused();
            }

            if (Pause.paused)
            {
                HMOVE = 0;
                VMOVE = 0;
                sprint = false;
                jump = false;
                pause = false;
                isGRounded = false;
                isJumping = false;
                isSprinting = false;
            }


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
       
    }

    private void LateUpdate()
    {
        if (photonView.IsMine) 
        {
            anim.SetBool("Idle", move == Vector3.zero);
            anim.SetBool("Run", sprintAnim);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!die) 
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


            if (Pause.paused)
            {
                HMOVE = 0;
                VMOVE = 0;
                sprint = false;
                jump = false;
                isGRounded = false;
                isJumping = false;
                isSprinting = false;
                isSliding = false;
            }

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

    /*public void TakeDamage(int damage, int actor) 
    {
        if (photonView.IsMine) 
        {
            currentHtl -= damage;
            RefreshBar();

            if (currentHtl <= 0) 
            {
                manager.Spawn();

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }*/

    public void TakeDamage(int damage) 
    {
        photonView.RPC(nameof(RPC_TakeDamage), photonView.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage, PhotonMessageInfo info) 
    {
        if (photonView.IsMine) 
        {
            if (!die) 
            {
                currentHtl -= damage;
                RefreshBar();

                if (currentHtl <= 0)
                {
                    Die();
                    die = true;
                    anim.SetTrigger("Death");
                    PlayerManager.Find(info.Sender).GetKill();
                }
            }

        }
    }

    void RefresMultiplayerState() 
    {
        float cacheEuly = waeponPar.localEulerAngles.y;
        Quaternion targetRot = Quaternion.identity * Quaternion.AngleAxis(aimAngle, Vector3.right);
        waeponPar.rotation = Quaternion.Slerp(waeponPar.rotation, targetRot, Time.deltaTime * 8f);

        Vector3 finalRot = waeponPar.localEulerAngles;
        finalRot.y = cacheEuly;
        waeponPar.localEulerAngles = finalRot;
    }

    void Die() 
    {
        playerManager.Die();
    }
}
