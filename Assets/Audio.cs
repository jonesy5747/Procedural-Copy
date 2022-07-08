using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource dayMusicSource;
    public AudioSource nightMusicSource;
    public AudioSource enemySource;

    public AudioClip hittingSound;
    public AudioClip pickupSound;
    public AudioClip pickupGemSound;
    public AudioClip pickupWoodSound;
    public AudioClip pickupAppleSound;
    public AudioClip eatingSound;

    public AudioClip enemyHurt;
    public AudioClip enemyPassive;
    public AudioClip enemyDeath1;
    public AudioClip enemyDeath2;
    public AudioClip playerDeath1;
    public AudioClip playerDeath2;

    public AudioClip health1;
    public AudioClip health2;
    public AudioClip health3;
    public AudioClip purchaseSound;
    public AudioClip daySoundtrack;
    public AudioClip nightSoundtrack;

    public AudioClip hungerSound1;
    public AudioClip hungerSound2;
    public AudioClip hungerSound3;
    public AudioClip hungerSound4;

    public AudioClip shopOpenSound;
    public AudioClip achievedSound;

    public AudioClip walkRockSound;
    public AudioClip walkGrassSound;

    public float minPitchRange;
    public float maxPitchRange;
    public float fadeTime;

    private float nextSoundTime;

    public void Play(AudioClip sound, AudioSource source)
    {
        source.PlayOneShot(sound);
    }

    public void PlayRandom(AudioClip sound, AudioSource source)
    {
        float randomPitch = Random.Range(minPitchRange, maxPitchRange);
        source.pitch = randomPitch;
        source.PlayOneShot(sound);
    }

    public void PlayLooped(AudioClip sound, AudioSource source)
    {
        if (Time.time >= nextSoundTime)
        {
            source.PlayOneShot(sound);
            nextSoundTime += sound.length;
        }
    }

    public void Stop(AudioSource source)
    {
        source.Stop();
    }

    public void Fade(AudioSource source)
    {
        for (float i = 1000; i > 0; i --)
        {
            source.volume = i/1000;
        }
    }
}
