using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNight : MonoBehaviour
{
    public float weight;
    public float time;
    public bool day = true;
    public bool night = false;
    public Volume vol;
    public float timeStep;

    public Audio manager;
    private float nextSoundTimeDay;
    private float nextSoundTimeNight;
    private bool fade = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        weight = time;

        if (day)
        {
            /*manager.Stop(manager.nightMusicSource);
            if (Time.time >= nextSoundTimeDay)
            {
                manager.Play(manager.daySoundtrack, manager.dayMusicSource);
                nextSoundTimeDay += manager.daySoundtrack.length;
            }*/
            if (time < 2) {
                time += timeStep * Time.deltaTime;
                vol.weight = weight;
            }
            if (time >= 2)
            {
                day = false;
                night = true;
            }
        }

        if (night)
        {
            /*manager.Stop(manager.dayMusicSource);
            if (Time.time >= nextSoundTimeNight)
            {
                manager.Play(manager.nightSoundtrack, manager.nightMusicSource);
                nextSoundTimeNight += manager.nightSoundtrack.length;
            }*/
            if (time > -1)
            {
                time -= timeStep * Time.deltaTime;
                vol.weight = weight;
            }
            if (time <= -1)
            {
                day = true;
                night = false;
            }
        }

        if (time < 0.5)
        {
            manager.Stop(manager.nightMusicSource);
            if (Time.time >= nextSoundTimeDay)
            {
                manager.Play(manager.daySoundtrack, manager.dayMusicSource);
                nextSoundTimeDay += manager.daySoundtrack.length;
            }
        }

        if (time > 0.5)
        {
            manager.Stop(manager.dayMusicSource);
            if (Time.time >= nextSoundTimeNight)
            {
                manager.Play(manager.nightSoundtrack, manager.nightMusicSource);
                nextSoundTimeNight += manager.nightSoundtrack.length;
            }
        }
    }
}
