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
    }
}
