using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    private Inventory inventory;
    private bool canOpen;
    private GameObject endScreen;
    private GameObject[] mainUI;

    void Awake()
    {
        mainUI = GameObject.FindGameObjectsWithTag("mainUI");
        endScreen = GameObject.FindWithTag("endShop");
        endScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i].gameObject.transform.childCount > 0) {
                if (inventory.slots[i].gameObject.transform.GetChild(0).tag == "keyButton")
                {
                    canOpen = true;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (canOpen && other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                endScreen.SetActive(true);
                Time.timeScale = 0;
                foreach (GameObject ui in mainUI)
                {
                    ui.SetActive(false);
                }
            }
        }
    }
}
