using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemCount : MonoBehaviour
{
    private Text number;
    public string itemToCount;
    // Start is called before the first frame update
    void Start()
    {
        number = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemToCount == "rock")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().rock.ToString();
        }
        if (itemToCount == "coal")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().coal.ToString();
        }
        if (itemToCount == "ruby")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().ruby.ToString();
        }
        if (itemToCount == "diamond")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().diamond.ToString();
        }
        if (itemToCount == "wood")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().wood.ToString();
        }
        if (itemToCount == "tuna")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().tuna.ToString();
        }
        if (itemToCount == "apple")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple.ToString();
        }
        if (itemToCount == "cookedTuna")
        {
            number.text = GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna.ToString();
        }
    }
}
