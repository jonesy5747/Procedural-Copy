using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class LightIntensity : MonoBehaviour
{
    public float intensity;
    // Start is called before the first frame update
    void Awake()
    {
        intensity = GetComponent<Light2D>().intensity;
        GetComponent<Light2D>().intensity = intensity * GameObject.FindWithTag("volume").GetComponent<Volume>().weight;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("volume").GetComponent<Volume>().weight >= 0 && GameObject.FindWithTag("volume").GetComponent<Volume>().weight <= 1)
        {
            GetComponent<Light2D>().intensity = intensity * GameObject.FindWithTag("volume").GetComponent<Volume>().weight;
        }

        if (GameObject.FindWithTag("volume").GetComponent<Volume>().weight <= 0)
        {
            GetComponent<Light2D>().intensity = 0;
        }

        if (GameObject.FindWithTag("volume").GetComponent<Volume>().weight >= 1)
        {
            GetComponent<Light2D>().intensity = intensity;
        }
    }
}
