using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class FuelFire : MonoBehaviour
{
    public float fireTime;
    public GameObject cookedTuna;
    private bool isBurning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTime -= Time.deltaTime;

        if (fireTime <= 0)
        {
            fireTime = 0;
            gameObject.transform.GetChild(0).GetComponent<Light2D>().enabled = false;
            isBurning = false;
        }
        if (fireTime > 0)
        {
            gameObject.transform.GetChild(0).GetComponent<Light2D>().enabled = true;
            isBurning = true;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "woodCollectable")
        {
            fireTime += 5;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "coalCollectable")
        {
            fireTime += 15;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "tunaCollectable")
        {
            if (isBurning) 
            {
                Destroy(other.gameObject);
                Instantiate(cookedTuna, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);
            }
        }
    }
}
