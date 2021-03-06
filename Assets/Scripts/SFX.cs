using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX Instance;
    public AudioSource audioSource;
    public AudioClip playerGunHit;
    private GameObject lastSource;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayHitSound(GameObject source)
    {
        if (lastSource != source)
        {
            audioSource.pitch = 1;
        }
        lastSource = source;
        audioSource.PlayOneShot(playerGunHit);
        audioSource.pitch += 0.05f;
    }
}