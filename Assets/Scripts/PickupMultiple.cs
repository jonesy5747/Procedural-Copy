using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMultiple : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    private bool ePressed;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            ePressed = true;
        }
        else
        {
            ePressed = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ePressed)
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == true)
                {
                    if (inventory.slots[i].gameObject.transform.GetChild(0).GetComponent<Image>().sprite == this.gameObject.transform.GetComponent<SpriteRenderer>().sprite) 
                    {
                        Destroy(gameObject);
                        inventory.slots[i].GetComponent<Slot>().DestroyItem();
                        Instantiate(itemButton, inventory.slots[i].transform, false);
                        break;
                    }
                }

                if (inventory.isFull[i] == false)
                {
                    for (int j = 0; j < inventory.slots.Length; j++)
                    {
                        if (inventory.slots[j].gameObject.transform.childCount > 0)
                        {
                            if (inventory.slots[j].gameObject.transform.GetChild(0).GetComponent<Image>().sprite == this.gameObject.transform.GetComponent<SpriteRenderer>().sprite)
                            {
                                return;
                            }
                        }
                    }

                    inventory.isFull[i] = true;
                    inventory.items[i] = this.gameObject.tag.ToString();
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
