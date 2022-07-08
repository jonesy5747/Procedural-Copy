using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private GameObject KeyShop;
    private GameObject[] mainUI;

    public Audio manager;
    private bool hasEntered = false;
    // Start is called before the first frame update
    void Awake()
    {
        mainUI = GameObject.FindGameObjectsWithTag("mainUI");
        KeyShop = GameObject.FindWithTag("keyShop");
        KeyShop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (!hasEntered) {
                    GetComponent<AudioSource>().Play();
                    hasEntered = true;
                }
                KeyShop.SetActive(true);
                foreach (GameObject ui in mainUI) 
                {
                    ui.SetActive(false);
                }
            }
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        hasEntered = false;
        if (other.gameObject.tag == "Player")
        {
            KeyShop.SetActive(false);
            foreach (GameObject ui in mainUI)
            {
                ui.SetActive(true);
            }
        }
    }
}
