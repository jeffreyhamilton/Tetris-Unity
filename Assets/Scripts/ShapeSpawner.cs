using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{

    //an array of of game objects to store the tetris shapes
    public GameObject[] shapes;
    //an array of of game objects to store the tetris shapes to be shown in the next shape area
    public GameObject[] nextShapes;

    GameObject upNextObject = null;

    int shapeIndex = 0;
    int nextShapeIndex = 0;

    //function to randomly choose from the 7 shapes
    public void SpawnShape()
    {
        //pulls from the next shape index to display the current shape you move
        shapeIndex = nextShapeIndex;

        //takes number and applies it to the index of shapes. What number each shape is assigned is handled over in unity.
        Instantiate(shapes[shapeIndex],
            transform.position,//position of shape spawner on the screen
            Quaternion.identity);//this handles rotation

        //generates random number between 0 and 6
        nextShapeIndex = Random.Range(0, 6);

        Vector3 nextShapePos = new Vector3(-6, 15, 0);

        if (upNextObject != null)
            Destroy(upNextObject);

        upNextObject = Instantiate(nextShapes[nextShapeIndex], nextShapePos, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {

        nextShapeIndex = Random.Range(0, 6);

        // Spawn first shape
        SpawnShape();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
