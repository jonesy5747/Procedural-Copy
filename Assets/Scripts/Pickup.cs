using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    private bool ePressed;

    public Audio manager;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddKeyToInventory()
    {
        manager.Play(manager.purchaseSound, manager.sfxSource);
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == false) 
            { 
                inventory.isFull[i] = true;
                Instantiate(itemButton, inventory.slots[i].transform, false);
                Destroy(gameObject);
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().totalMoney -= 100;
                break;
            }
        }
    }
}
