using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpawnDroppedItem>().spawnDroppedItem();

            if (inventory.items[i] == "rockCollectable") {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().rock == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().rock -= 1;
            }

            if (inventory.items[i] == "coalCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().coal == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().coal -= 1;
            }

            if (inventory.items[i] == "rubyCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().ruby == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().ruby -= 1;
            }

            if (inventory.items[i] == "diamondCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().diamond == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().diamond -= 1;
            }

            if (inventory.items[i] == "woodCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().wood == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().wood -= 1;
            }

            if (inventory.items[i] == "tunaCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().tuna == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().tuna -= 1;
            }

            if (inventory.items[i] == "appleCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple -= 1;
            }
            if (inventory.items[i] == "cookedTunaCollectable")
            {
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna == 1)
                {
                    Destroy(child.gameObject);
                }
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna -= 1;
            }
        }
    }

    public void DestroyItem()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
