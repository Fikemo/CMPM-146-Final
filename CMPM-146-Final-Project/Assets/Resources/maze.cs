using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;

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
        // Debug.Log(s.Length);
        // s = s.Replace("\n", "");
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
        string debugArrayString = "";
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                debugArrayString += mazeArray[i, j];
            }
            debugArrayString += "\n";
        }
        Debug.Log(debugArrayString);

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
        int[] goal = new int[2] { size - 2, size - 2 };
        Debug.Log("Goal " + goal[0] + " " + goal[1]);
        generatePathfinderCommands(start, goal);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySpawn = true;
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
                        t = (GameObject)(Instantiate(floor, new Vector2(column, row), Quaternion.identity));
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
        /*int xi,yi;

        int[] xOffset = new int[3] { -1, 0, 1 };
        int[] yOffset = new int[3] { -1, 0, 1 };

        for(xi = 0; xi < xOffset.Length; xi++)
        {
            for (yi = 0; yi < yOffset.Length; yi++)
            {
                if (Math.Abs(xi) != Math.Abs(yi))
                {
                    if (mazeArray[currentCoor[0] + xOffset[xi], currentCoor[1] + yOffset[yi]] != '2') // just avoid walls
                    {
                        int[] n = new int[2] { currentCoor[0] + xOffset[xi], currentCoor[1] + yOffset[yi] };
                        neighbors.Add(n);
                    }
                }
            }
        }*/

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



    private List<string> generatePathfinderCommands(int[] start, int[] goal)
    {
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

            if ((mazeArray[current[0], current[1]] == '3') || stepLimit > 1000000)
            {
                while (current != null)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
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
                    Debug.Log("Next " + nextCopy[0] + ", " + next[1] + ", priority " + priority);
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

        for (int i = 0; i < path.Count - 1; i++)
        {
            int[] normalDirection = new int[2];

            normalDirection[0] = path[i + 1][0] - path[i][0];
            normalDirection[1] = path[i + 1][1] - path[i][1];

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

        foreach (int[] s in path)
        {
            Debug.Log("Path " + s[0] + " " + s[1]);
        }

        return commands;


        /*commands = new List<string>();

        List<int[]> path = new List<int[]>();
        int stepLimit = 0;

        SimplePriorityQueue<int[]> frontier = new SimplePriorityQueue<int[]>();

        frontier.Enqueue(start, 0);

        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();
        cameFrom.Add(start, null);

        Dictionary<int[], float> costSoFar = new Dictionary<int[], float>();
        costSoFar.Add(start, 0f);

        while(frontier.Count > 0)
        {
            int[] current = frontier.Dequeue();

            if((current[0] == goal[0] && current[1] == goal[1]) || stepLimit > 1000000)
            {
                int[] ct = current;
                while (ct != null)
                {
                    path.Add(ct);
                    ct = cameFrom[ct];
                }
                path.Reverse();
                break;
            }
            stepLimit++;

            foreach(int[] next in getNeighbors(current))
            {
                float newCost = costSoFar[current] + 1;
                costSoFar[next] = newCost;
                float priority = newCost;
                frontier.Enqueue(next, priority);
                cameFrom[next] = current;
            }
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            int[] normalDirection = new int[2];

            normalDirection[0] = path[i + 1][0] - path[i][0];
            normalDirection[1] = path[i + 1][1] - path[i][1];

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

        return commands;*/

        /*Dictionary<float, List<int[]>> frontier = new Dictionary<float, List<int[]>>();

        void push(float priority, int[] value)
        {
            if (!frontier.ContainsKey(priority))
            {
                frontier.Add(priority, new List<int[]>());
            }

            frontier[priority].Insert(0, value);
        }

        int[] pop()
        {
            List<float> list = frontier.Keys.ToList(); // put the keys (priorities) in a list
            list.Sort(); // sort the list

            float firstKey = list[0]; // get the first priority

            int[] valueToReturn = frontier[firstKey][0]; // get the first int[] of the first priority list

            frontier[firstKey].RemoveAt(0); // remove that value from that list

            if (frontier[firstKey].Count <= 0)
            {
                frontier.Remove(firstKey); // if the list is now empty, remove it from the dictionary
            }

            return valueToReturn; // return the top priority value
        }

        push(0f, start);

        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();
        cameFrom.Add(start, null);

        Dictionary<int[], float> costSoFar = new Dictionary<int[], float>();
        costSoFar.Add(start, 0f);

        while( frontier.Count > 0)
        {
            int[] current = pop();

            if ((current[0] == goal[0] && current[1] == goal[1]) || stepLimit > 1000000)
            {
                int[] ct = current;
                while (ct != null)
                {
                    path.Add(ct);
                    ct = cameFrom[ct];
                }
                path.Reverse();
                break;
            }
            stepLimit++;

            foreach(int[] next in getNeighbors(current))
            {
                float newCost = costSoFar[current] + 1;
                if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    if (!costSoFar.ContainsKey(next))
                    {
                        costSoFar.Add(next, newCost);
                    }
                    else
                    {
                        costSoFar[next] = newCost;
                    }

                    float priority = newCost;// + getDistanceSquared(next, goal);
                    push(priority, next);

                    if (!cameFrom.ContainsKey(next))
                    {
                        cameFrom.Add(next, current);
                    }
                    else
                    {
                        cameFrom[next] = current;
                    }
                }
            }
        }

        foreach(int[] p in path)
        {
            Debug.Log("Path " + p[0] + ", " + p[1]);
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            int[] normalDirection = new int[2];

            normalDirection[0] = path[i + 1][0] - path[i][0];
            normalDirection[1] = path[i + 1][1] - path[i][1];

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

        return commands;*/


        /*int stepLimit = 0;

        commands = new List<string>();

        List<int[]> path = new List<int[]>();

        SortedList<float, int[]> frontier = new SortedList<float, int[]>();
        frontier.Add(0, start);

        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();
        cameFrom[start] = null;

        Dictionary<int[], float> costSoFar = new Dictionary<int[], float>();
        costSoFar[start] = 0f;

        void pushToFrontier(float priority, int[] value)
        {
            if (frontier.ContainsKey(priority))
            {
                int[] tempVal = frontier[priority];
                frontier[priority] = value;
                pushToFrontier(priority + 0.001f, tempVal);
            }
            else
            {
                frontier.Add(priority, value);
            }
        }

        while (frontier.Count > 0)
        {
            int[] current = frontier[0];

            if (stepLimit > 10000)
            {
                int[] ct = current;
                while (ct != null)
                {
                    path.Add(ct);
                    ct = cameFrom[ct];
                }

                break;
            }
            stepLimit++;

            if (current[0] == goal[0] && current[1] == current[1])
            {
                int[] ct = current;
                while(ct != null)
                {
                    path.Add(ct);
                    ct = cameFrom[ct];
                }
                
                break;
            }

            foreach(int[] next in getNeighbors(current))
            {
                float newCost = costSoFar[current] + 1;
                if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    float priority = newCost + getDistanceSquared(goal, next);
                    pushToFrontier(priority, next);
                    cameFrom[next] = current;
                }
            }
        }

        foreach(int[] p in path)
        {
            Debug.Log("Path " + p[0] + ", " + p[1]);
        }

        return commands;*/

        /*commands = new List<string>(); //list of commands ex. <right, left, right, up, down>

        List<int[]> path = new List<int[]>(); //list of path coordinates ex. <[1,1], [1,2], [1,3], [2,3]>

        //int[] start = new int[2] { 1, 1 }; //starting coordinate of player

        SortedList<float, int[]> heapQ = new SortedList<float, int[]>();
        heapQ.Add(0f, start);

        void push(float priority, int[] value)
        {
            if (heapQ.ContainsKey(priority))
            {
                int[] tempVal = heapQ[priority];
                heapQ[priority] = value;
                push(priority + 0.001f, tempVal);
            }
            else
            {
                heapQ.Add(priority, value);
            }
        }

        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();
        cameFrom[start] = null;

        Dictionary<int[], int> costSoFar = new Dictionary<int[], int>();
        costSoFar[start] = 0;

        int timer = 0;

        while (heapQ.Count > 0)
        {

            float firstKey = heapQ.Keys[0];
            Debug.Log(firstKey);
            int[] current = heapQ[firstKey];
            Debug.Log("current cell " + current[0] + " " + current[1]);
            heapQ.RemoveAt(0);
            Debug.Log("heapQ.Count: " + heapQ.Count);

            if (timer > 10000)
            {
                int[] ct = current;
                while (ct != null)
                {
                    path.Add(ct);
                    ct = cameFrom[ct];
                }
                path.Reverse();
                break;
            }
            timer++;

            //reconstruct path
            if (current[0] == goal[0] && current[1] == goal[1])
            {
                int[] ct = current;
                while(ct != null)
                {
                    path.Add(ct);
                    ct = cameFrom[ct];
                }
                path.Reverse();
                break;
            }

            foreach(int[] next in getNeighbors(current))
            {
                int newCost = costSoFar[current] + 1;
                if((!costSoFar.ContainsKey(next) || newCost < costSoFar[next]))
                {
                    costSoFar[next] = newCost;
                    float priority = newCost;
                    Debug.Log(("priority ", priority)); //lower proirity has more prority and appears sooner in the queue
                    push(priority, next);
                    cameFrom[next] = current;
                }
            }
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            int[] normalDirection = new int[2];

            normalDirection[0] = path[i + 1][0] - path[i][0];
            normalDirection[1] = path[i + 1][1] - path[i][1];

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

        foreach(int[] s in path)
        {
            Debug.Log("Path " + s[0] + " " + s[1]);
        }

        return commands;*/
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