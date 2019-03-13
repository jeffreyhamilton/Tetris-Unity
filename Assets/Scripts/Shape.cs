using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{

    public static float speed = 1.0f;

    float lastMoveDown = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsInGrid())
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.gameOver);

            Invoke("OpenGameOverScene", .5f);
        }

        InvokeRepeating("IncreaseSpeed", 2.0f, 2.0f);
    }

    void OpenGameOverScene()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    void IncreaseSpeed()
    {
        Shape.speed -= .001f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("a"))
        {
            transform.position += new Vector3(-1, 0, 0);

            //all debug logs below used to find border cordinates back in the console window in unity
            //Debug.Log(transform.position);

            //checks if in border an adjusts for when you move the object left
            if (!IsInGrid())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }
        }

        if (Input.GetKeyDown("d"))
        {
            transform.position += new Vector3(1, 0, 0);

            //Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }
        }

        if (Input.GetKeyDown("s") || Time.time - lastMoveDown >= Shape.speed)
        {
            transform.position += new Vector3(0, -1, 0);

            //Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(0, 1, 0);
                //calls function in GameBoard to see if row should be deleted if true 
                //the the next function to increase score and delete row
                bool rowDeleted = GameBoard.DeleteAllFullRows();

                if (rowDeleted)
                {
                    
                    //GameBoard.DeleteAllFullRows(); found to be not needed early on kept just incase

                    IncreaseTextUIScore();
                }

                //turns off shape once it reaches the bottom
                enabled = false;

                //Calls on shapespawner function to spawn next shape
                FindObjectOfType<ShapeSpawner>().SpawnShape();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeStop);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }

            //TODO: find way to speed up if key is held down
            lastMoveDown = Time.time;
        }

        //both w and e are used to rotate the shape clockwise and counter clockwise
        if (Input.GetKeyDown("w"))
        {
            transform.Rotate(0, 0, 90);

            //Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateSound);
            }
        }

        if (Input.GetKeyDown("e"))
        {
            transform.Rotate(0, 0, -90);

            //Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, 90);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateSound);
            }
        }

    }

    //checks to make sure the border of the object dont go outside the border of the game when moving and rotating
    public bool IsInGrid()
    {
        
        foreach(Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);

            if (!IsInBorder(vect))
            {
                return false;
            }

            if(GameBoard.gameBoard[(int)vect.x, (int)vect.y] != null &&
                GameBoard.gameBoard[(int)vect.x, (int)vect.y].parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    //rounds vector to int. Fractions were causing blocks to occasionaly hang up on corners instead of sliding by.
    public Vector2 RoundVector(Vector2 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }

    //after finding cordinates through debugging I was able to create this boolean to check if object is within border
    public static bool IsInBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
            (int)pos.x <= 9 &&
            (int)pos.y >= 1);
    }

    //cycles through all different blocks in gameboard array checks for transforms and null values in blocks. 
    //After that as the blocks move null will placed where the blocks used to be.
    public void UpdateGameBoard()
    {
        for(int y= 0; y <20; ++y)
        {
            for (int x = 0; x < 10; ++x)
            {
                if (GameBoard.gameBoard[x, y] != null &&
                    GameBoard.gameBoard[x, y].parent == transform)
                {
                    GameBoard.gameBoard[x, y] = null;
                }
            }
        }

        foreach (Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);


            GameBoard.gameBoard[(int)vect.x, (int)vect.y] = childBlock;

            Debug.Log("Cube At : " + (int)vect.x + " " + (int)vect.y);
            
        }

        //Early on this was used to print the grid array to check for errors
        //GameBoard.PrintArray();

    }

    //Increase the Score
    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score++;

        textUIComp.text = score.ToString();

        //TODO find out how to do multipliers for when multiple rows are deleted at once
    }

}
