using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource music;
    public AudioSource spin;
    public AudioSource hum;
    public AudioSource sfx;
    public AudioClip pickup;
    public float pickupVol;
    public AudioClip shootout;
    public float shootoutVol;
    public AudioClip explosion;
    public float explosionVol;

    private float spinPitch;
    private float sfxPitch;
    void Start()
    {
        Instance = this;
        spinPitch = spin.pitch;
        sfxPitch = sfx.pitch;
    }

    void Update()
    {
        spin.pitch = spinPitch * Time.timeScale;
        sfx.pitch = sfxPitch * Time.timeScale;

    }

    public void PlayPickup()
    {
        sfx.volume = pickupVol;
        sfx.PlayOneShot(pickup);
    }

    public void PlayShootout()
    {
        sfx.volume = shootoutVol;
        sfx.PlayOneShot(shootout);
    }

    public void PlayExplosion()
    {
        sfx.volume = explosionVol;
        sfx.PlayOneShot(explosion);
    }
}
