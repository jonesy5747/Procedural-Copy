using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButton : MonoBehaviour
{
    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        key.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().totalMoney >= 1)
        {
            key.SetActive(true);
        }
    }
}
