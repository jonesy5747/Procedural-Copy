using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatItem : MonoBehaviour
{
    private GameObject player;
    private string foodItem;

    public Audio manager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        foodItem = gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void eatItem(int healValue)
    {
        GetComponent<AudioSource>().Play();
        if (player.GetComponent<PlayerControl>().health < 100)
        {
            player.GetComponent<PlayerControl>().health += healValue;
        }
        else
        {
            player.GetComponent<Hunger>().hungerValue += healValue;
        }

        if (gameObject.transform.GetChild(0).GetComponent<Text>().text == "1") 
        {
            Destroy(gameObject);
        }
        if (foodItem == "appleButton")
        {
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple -= 1;
        }
        if (foodItem == "cookedTunaButton")
        {
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna -= 1;
        }
    }
}
