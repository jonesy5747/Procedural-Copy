using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public GameObject player;
    public GameObject fire;

    public RaycastHit2D hit;

    public float coal, ruby, diamond, rock, wood, tuna, apple, cookedTuna;

    public float coalValue, rubyValue, diamondVlaue, rockValue, woodValue, tunaValue, appleValue;

    public float totalMoney;

    public Text money;

    private bool tunaSelected;
    public GameObject tunaCooked;

    public Audio manager;
    private bool achieved;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalMoney = coal * coalValue + ruby * rubyValue + diamond * diamondVlaue + rock * rockValue + wood * woodValue + tuna * tunaValue + apple * appleValue;
        money.text = "$" + totalMoney.ToString(); 

        if (totalMoney >= 100)
        {
            if (!achieved) {
                manager.Play(manager.achievedSound, manager.sfxSource);
                achieved = true;
            }
        }

        if (totalMoney < 100)
        {
            achieved = false;
        }

        /*
        if (hit.transform.gameObject.tag == "fire")
        {
            if (tunaSelected && Input.GetMouseButtonDown(0) && tuna > 0)
            {
                Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                Instantiate(tunaCooked, pos, Quaternion.identity);
                tuna -= 1;
            }
        }
        */
    }
}
