using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<GameObject> terrains;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> terrains = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //terrains[(GameObject.FindWithTag("nextTile").GetComponent<NextTile>().playerCurrentTileNumber) - 1].SetActive(true);
        for (int i = 0; i < terrains.Count; i++)
        {
            if (terrains[i] != null)
            {
                if ((i != GameObject.FindWithTag("nextTile").GetComponent<NextTile>().playerCurrentTileNumber - 1))
                {
                    terrains[i].SetActive(false);
                }
                if ((i == GameObject.FindWithTag("nextTile").GetComponent<NextTile>().playerCurrentTileNumber - 1))
                {
                    terrains[i].SetActive(true);
                }
            }
        }
    }
}
