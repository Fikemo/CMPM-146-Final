using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;
using UnityEngine.SceneManagement;

public class maze : MonoBehaviour
{
    public GameObject enemy;
    public GameObject wall;
    public GameObject floor;
    public GameObject key;
    public GameObject computer;
    public GameObject exit;
    public int origSize;
    public int size;
    public char[,] mazeArray;
    public char[,] reducedMazeArray;
    public List<string> commands;
    public bool enemySpawn = true;

    private void Awake()
    {
        //origSize = UnityEngine.Random.Range(8, 15);
        int mazeNumber = UnityEngine.Random.Range(0, 100);
        //origSize = 14;
        //int mazeNumber = 0;
        origSize = mainMenu.optionInt;
        size = origSize * 2 + 2;

        // TextAsset t1 = (TextAsset)Resources.Load("Size" + size.ToString() + "/Maze" + levelNumber.ToString(), typeof(TextAsset));
        // TextAsset t1 = (TextAsset)Resources.Load("level", typeof(TextAsset));
        TextAsset t1 = (TextAsset)Resources.Load("Size" + origSize.ToString() + "/Maze" + mazeNumber.ToString(), typeof(TextAsset));

        string s = t1.text;

        int i, j, k = 0;
        mazeArray = new char[size, size];
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                while (k < s.Length && s[k] != '0' && s[k] != '2' && s[k] != '3' && s[k] != '4' && s[k] != '5' && s[k] != '6' && s[k] != '7' && s[k] != '8')
                {
                    k++;
                }

                mazeArray[i, j] = s[k];
                k++;
            }
        }


        int[,] reducedMaze = new int[origSize, origSize];
        for (i = 0; i < origSize; i++)
        {
            int iScaled = (i * 2) + 2;
            for (j = 0; j < origSize; j++)
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

        int[] start = new int[2] { 1, 1 };
        int[][] goals = new int[3][];

        int[] usb = new int[2];
        int[] cpu = new int[2];

        string debugArrayString = "";
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {

                if (mazeArray[i, j] == '4')
                {
                    usb[0] = i;
                    usb[1] = j;
                }

                if (mazeArray[i, j] == '5')
                {
                    cpu[0] = i;
                    cpu[1] = j;
                }

                debugArrayString += mazeArray[i, j];
            }
            debugArrayString += "\n";
        }
        Debug.Log(debugArrayString);



        int[] exit = new int[2] { size - 2, size - 2 };


        goals[0] = usb;
        goals[1] = cpu;
        goals[2] = exit;
        Debug.Log("Goal " + exit[0] + " " + exit[1]);
        List<int[]> pathToUSB = generatePathfinderCommands(start, usb);
        List<int[]> pathToCPU = generatePathfinderCommands(usb, cpu);
        List<int[]> pathToExit = generatePathfinderCommands(cpu, exit);

        List<int[]> totalPath = new List<int[]>();
        totalPath.AddRange(pathToExit);
        totalPath.AddRange(pathToCPU);
        totalPath.AddRange(pathToUSB);
        totalPath.Reverse();

        foreach (int[] c in totalPath)
        {
            Debug.Log("Path " + c[0] + " " + c[1]);
        }

        for (i = 0; i < totalPath.Count - 1; i++)
        {
            int[] normalDirection = new int[2];

            normalDirection[0] = totalPath[i + 1][0] - totalPath[i][0];
            normalDirection[1] = totalPath[i + 1][1] - totalPath[i][1];

            if (normalDirection[0] == 1)
            {
                commands.Add("down");
            }
            else if (normalDirection[0] == -1)
            {
                commands.Add("up");
            }
            else if (normalDirection[1] == 1)
            {
                commands.Add("right");
            }
            else if (normalDirection[1] == -1)
            {
                commands.Add("left");
            }
        }

        foreach (string c in commands)
        {
            Debug.Log("Go " + c);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "TestScene")
        {
            enemySpawn = false;
        }
        if (sceneName == "GameScene")
        {
            enemySpawn = true;
        }
        int i, j = 0;
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
                        if (enemySpawn)
                        {
                            t = Instantiate(enemy, new Vector2(column, row), Quaternion.identity);
                            t.gameObject.tag = mazeArray[i, j].ToString();
                        }
                        break;
                }
            }
        }
    }

    private List<int[]> getNeighbors(int[] currentCoor)
    {
        List<int[]> neighbors = new List<int[]>();

        if (mazeArray[currentCoor[0] + 1, currentCoor[1]] != '2')
        {
            int[] n = new int[2] { currentCoor[0] + 1, currentCoor[1] };
            neighbors.Add(n);
        }
        if (mazeArray[currentCoor[0] - 1, currentCoor[1]] != '2')
        {
            int[] n = new int[2] { currentCoor[0] - 1, currentCoor[1] };
            neighbors.Add(n);
        }
        if (mazeArray[currentCoor[0], currentCoor[1] + 1] != '2')
        {
            int[] n = new int[2] { currentCoor[0], currentCoor[1] + 1 };
            neighbors.Add(n);
        }
        if (mazeArray[currentCoor[0], currentCoor[1] - 1] != '2')
        {
            int[] n = new int[2] { currentCoor[0], currentCoor[1] - 1 };
            neighbors.Add(n);
        }

        return neighbors;
    }

    private float getDistanceSquared(int[] start, int[] end)
    {
        float d = ((end[0] - start[0]) * (end[0] - start[0])) + ((end[1] - start[1]) * (end[1] - start[1]));
        return d;
    }



    private List<int[]> generatePathfinderCommands(int[] start, int[] goal)
    {
        Debug.Log(goal[0] + ", " + goal[1]);

        commands = new List<string>();
        List<int[]> path = new List<int[]>();

        SimplePriorityQueue<int[]> frontier = new SimplePriorityQueue<int[]>();
        frontier.Enqueue(start, 0f);

        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>(new MyEqualityComparer());
        Dictionary<int[], float> costSoFar = new Dictionary<int[], float>(new MyEqualityComparer());

        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);

        int stepLimit = 0;

        while (frontier.Count > 0)
        {
            int[] current = frontier.Dequeue();

            if ((current[0] == goal[0] && current[1] == goal[1]))
            {
                while (current != null)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                break;
            }
            stepLimit++;

            foreach (int[] next in getNeighbors(current))
            {
                int[] nextCopy = new int[2] { next[0], next[1] };
                float newCost = costSoFar[current] + 1f;
                if (!costSoFar.ContainsKey(nextCopy) || newCost < costSoFar[nextCopy])
                {


                    if (!costSoFar.ContainsKey(nextCopy))
                    {
                        costSoFar.Add(nextCopy, newCost);
                    }
                    else
                    {
                        costSoFar[nextCopy] = newCost;
                    }

                    float priority = newCost + getDistanceSquared(nextCopy, goal);
                    frontier.Enqueue(nextCopy, priority);

                    if (!cameFrom.ContainsKey(nextCopy))
                    {
                        cameFrom.Add(nextCopy, current);
                    }
                    else
                    {
                        cameFrom[nextCopy] = current;
                    }
                }
            }
        }



        return path;

    }
}

public class MyEqualityComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[] x, int[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(int[] obj)
    {
        int result = 17;
        for (int i = 0; i < obj.Length; i++)
        {
            unchecked
            {
                result = result * 23 + obj[i];
            }
        }
        return result;
    }
}