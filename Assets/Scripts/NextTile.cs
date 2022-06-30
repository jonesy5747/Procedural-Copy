using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTile : MonoBehaviour
{
    public GameObject objectsToMove;
    public int playerCurrentTileNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(GameObject.FindWithTag("Player").transform.position);

        if (viewPosition.x > 1.01f)
        {
            MoveObjectsRight();
        }
        else if (viewPosition.x < -0.01f)
        {
            MoveObjectsLeft();
        }
        if (viewPosition.y > 1.01f)
        {
            MoveObjectsUp();
        }
        else if (viewPosition.y < -0.01f)
        {
            MoveObjectsDown();
        }
    }

    void MoveObjectsRight()
    {
        objectsToMove.transform.position = new Vector3(objectsToMove.transform.position.x + 89, objectsToMove.transform.position.y, objectsToMove.transform.position.z);
    }
    void MoveObjectsLeft()
    {
        objectsToMove.transform.position = new Vector3(objectsToMove.transform.position.x - 89, objectsToMove.transform.position.y, objectsToMove.transform.position.z);
    }
    void MoveObjectsUp()
    {
        objectsToMove.transform.position = new Vector3(objectsToMove.transform.position.x, objectsToMove.transform.position.y + 50, objectsToMove.transform.position.z);
    }
    void MoveObjectsDown()
    {
        objectsToMove.transform.position = new Vector3(objectsToMove.transform.position.x, objectsToMove.transform.position.y - 50, objectsToMove.transform.position.z);
    }
}
