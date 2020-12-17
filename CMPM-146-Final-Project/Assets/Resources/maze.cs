using JetBrains.Annotations;
using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public class maze : MonoBehaviour
{
    public GameObject enemy;
    public GameObject wall;
    public GameObject floor;
    public GameObject key;
    public GameObject computer;
	public GameObject exit;
    public char[,] mazeArray;
    public char[,] reducedMazeArray;
    // Start is called before the first frame update
    void Start()
    {
        // int size = Random.Range(8, 15);
        // int mazeNumber = Random.Range(0, 100);
        int origSize = 14;
        int mazeNumber = 0;
        int size = origSize * 2 + 2;

        // TextAsset t1 = (TextAsset)Resources.Load("Size" + size.ToString() + "/Maze" + levelNumber.ToString(), typeof(TextAsset));
        // TextAsset t1 = (TextAsset)Resources.Load("level", typeof(TextAsset));
        TextAsset t1 = (TextAsset)Resources.Load("Size" + origSize.ToString() + "/Maze" + mazeNumber.ToString(), typeof(TextAsset));

        string s = t1.text;

        int i, j, k = 0;
        // Debug.Log(s.Length);
        // s = s.Replace("\n", "");
        mazeArray = new char[size, size];
        for (i = 0; i < size; i++)
        {
            for(j = 0; j < size; j++)
            {
                /*if (s[k] == '\n')
                {
                    k++;
                }*/

                while(k < s.Length && s[k] != '0' && s[k] != '2' && s[k] != '3' && s[k] != '4' && s[k] != '5' && s[k] != '6' && s[k] != '7' && s[k] != '8')
                {
                    k++;
                }

                mazeArray[i, j] = s[k];
                k++;
            }
        }
        string debugArrayString = "";
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                debugArrayString += mazeArray[i, j];
            }
            debugArrayString += "\n";
        }

        char[,] reducedMaze = new char[origSize, origSize];
        for (i = 0; i < origSize; i ++)
        {
            int iScaled = (i * 2) + 2;
            for (j = 0; j < origSize; j ++)
            {
                int jScaled = (j * 2) + 2;
                reducedMaze[i, j] = mazeArray[iScaled, jScaled];
            }
        }

        string debugReducedArrayString = "";
        for (i = 0; i < origSize; i++)
        {
            for (j = 0; j < origSize; j++)
            {
                debugReducedArrayString += reducedMaze[i, j];
            }
            debugReducedArrayString += "\n";
        }

        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                float row = -i * 1.6f;
                float column = j * 1.6f;
                GameObject t;
                switch (mazeArray[i, j])
                {
                    case '0':
                        t = (GameObject)(Instantiate(floor, new Vector2(column, row), Quaternion.identity));
                        break;
                    case '2':
                        t = (GameObject)(Instantiate(wall, new Vector2(column, row), Quaternion.identity));
                        break;
                    case '3':
                        t = (GameObject)(Instantiate(exit, new Vector2(column, row), Quaternion.identity));
                        break;
                    case '4':
                        //Debug.Log("USB KEY");
                        //t = (GameObject)(Instantiate(floor, new Vector2(column, row), Quaternion.identity));
                        t = (GameObject)(Instantiate(key, new Vector2(column, row), Quaternion.identity));
                        break;
                    case '5':
                        t = (GameObject)(Instantiate(computer, new Vector2(column, row), Quaternion.identity));
                        break;
                    case '6':
                    case '7':
                    case '8':
                        t = (GameObject)(Instantiate(floor, new Vector2(column, row), Quaternion.identity));
                        t = (GameObject)(Instantiate(enemy, new Vector2(column, row), Quaternion.identity));
                        break;
                }
            }
        }
    }
}
