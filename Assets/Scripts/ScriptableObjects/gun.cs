using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New gun", menuName = "Gun")]
public class gun : ScriptableObject
{
    public string names;
    public float firate;
    public float bloom;
    public float recoil;
    public float kickback;
    public float aimSpeed;
    public GameObject prefab;
    public float reload;
    public int damage;
    public int ammo;
    public int clipSize;
    public int burst; //0 semi //1 auto
    public int pellets;
    public Sprite sprGun;
    public AudioClip soundShoot;
    public float pitchRandom;
    public float volume;
    public bool recovery;
    public GameObject modelWeap;

    int stash;
    int clip;
    int normal;

    /*public void Initialize() 
    {
        stash = ammo;
        clip = clipSize;
    }

    public bool fireBullet() 
    {
        if(clip > 0)
        {
            clip -= 1;
            return true;
        }
        else return false;
    }

    public void Reload() 
    {

            stash += clip;
            clip = Mathf.Min(clipSize, stash);
            stash -= clip;
    }*/

    public int GetStash() { return stash; }
    public int GetClip() { return clip; }

}
