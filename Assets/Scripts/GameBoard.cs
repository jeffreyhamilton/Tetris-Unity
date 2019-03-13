using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{

    public static Transform[,] gameBoard = new Transform[10, 20];

    /* //Used early on to print an array for bug testing
    public static void PrintArray()
    {

        string arrayOutput = "";

        int iMax = gameBoard.GetLength(0) - 1;
        int jMax = gameBoard.GetLength(1) - 1;

        for (int j = jMax; j >= 0; j--)
        {
            for (int i = 0; i <= iMax; i++)
            {

                if (gameBoard[i, j] == null)
                {
                    arrayOutput += "N ";
                }
                else
                {
                    arrayOutput += "X";
                }

            }

            arrayOutput += "\n \n";

        }

        var myArrayComp = GameObject.Find("MyArray").GetComponent<Text>();
        myArrayComp.text = arrayOutput;

    }*/

    //Checkes if a row needs to be deleted
        public static bool DeleteAllFullRows()
        {
            for (int row = 0; row < 20; row++)
            {
                if (IsRowFull(row))
                {
                    //if true the function to delete the row in the game is called
                    DeleteGBRow(row);

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rowDelete);

                    return true;
                }
            }

            return false;

        }

    //Checks if row is full by checking if any col returns null
    public static bool IsRowFull(int row)
    {
        for (int col = 0; col < 10; col++)
        {
            if (gameBoard[col, row] == null)
            {
                return false;
            }
        }

        return true;
    }


    //The function that deletes the row that is full
    public static void DeleteGBRow(int row)
    {
        for (int col = 0; col < 10; col++)
        {
            //Destroys the cube in the gameboard
            Destroy(gameBoard[col, row].gameObject);
            //Destroys the cube in the array so the array and gameboard continue to match
            gameBoard[col, row] = null;
        }
        //increments up a row so that we can now cycle through and pull all rows down
        row++;

        for (int j = row; j < 20; j++)
        {
            for (int col = 0; col < 10; col++)
            {
                if(gameBoard[col, j] != null)
                {
                    gameBoard[col, j - 1] = gameBoard[col, j];

                    gameBoard[col, j] = null;

                    gameBoard[col, j - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

}
