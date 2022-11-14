using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonSonido : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioSource source;

    public bool sound;
    public bool played;

    public bool sound2;
    public bool played2;

    private void Update()
    {
        if (sound)
        {
            if (!played)
            {
                played = true;
                source.PlayOneShot(clip);
            }
        }

        if (sound2) 
        {
            if (!played2) 
            {
                played2 = true;
                source.PlayOneShot(clip2);
            }
        }
    }

    public void PlaySound() 
    {
        source.PlayOneShot(clip);
    }
}
