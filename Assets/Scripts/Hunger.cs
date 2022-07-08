using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour
{
    public float tickTime;
    public GameObject player;
    public Slider hungerBar;
    public float hungerValue;

    public Audio manager;
    private float nextSoundTime;
    // Start is called before the first frame update
    void Start()
    {
        hungerValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //Making hungerValue a number between 0 and 1 for bar output.
        hungerBar.value = hungerValue / 100;

        //Make hunger bar appear when health full.
        if (player.GetComponent<PlayerControl>().health == 100)
        {
            hungerBar.gameObject.SetActive(true);
        }

        if (hungerValue > 0)
        {
            hungerValue -= Time.deltaTime * tickTime;
            hungerBar.gameObject.SetActive(true);
        }
        else if (hungerValue <= 0)
        {
            player.GetComponent<PlayerControl>().health -= Time.deltaTime * tickTime;
            hungerValue = 0; 
            hungerBar.gameObject.SetActive(false);
        }

        if (hungerValue >= 100)
        {
            hungerValue = 100;
        }

        if (hungerValue <= 75 && hungerValue > 50)
        {
            if (Time.time >= nextSoundTime)
            {
                manager.Play(manager.hungerSound1, manager.sfxSource);
                nextSoundTime += 20;
            }
        }

        if (hungerValue <= 50 && hungerValue > 25)
        {
            if (Time.time >= nextSoundTime)
            {
                manager.Play(manager.hungerSound2, manager.sfxSource);
                nextSoundTime += 20;
            }
        }

        if (hungerValue <= 25 && hungerValue > 0)
        {
            if (Time.time >= nextSoundTime)
            {
                manager.Play(manager.hungerSound3, manager.sfxSource);
                nextSoundTime += 20;
            }
        }

        if (hungerValue <= 25 && hungerValue > 0)
        {
            if (Time.time >= nextSoundTime)
            {
                manager.Play(manager.hungerSound4, manager.sfxSource);
                nextSoundTime += 20;
            }
        }
    }
}
