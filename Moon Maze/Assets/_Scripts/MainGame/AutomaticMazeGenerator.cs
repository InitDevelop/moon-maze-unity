using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMazeGenerator : MonoBehaviour
{
    
    public bool[,] downWall;
    public bool[,] rightWall;
    public bool[,] visited;
    public List<Tuple> visitHistory;
    public List<GameObject> walls;
    public int historyMarker;

    public int x;
    public int y;

    int randInt;

    public const int MAZE_SIZE = 16;

    public GameObject wall_X_prefab;
    public GameObject wall_Z_prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        downWall = new bool[MAZE_SIZE, MAZE_SIZE];
        rightWall = new bool[MAZE_SIZE, MAZE_SIZE];
        visited = new bool[MAZE_SIZE, MAZE_SIZE];
        visitHistory = new List<Tuple>();
        historyMarker = 0;
        walls = new List<GameObject>();

        for (int x = 0; x < MAZE_SIZE; x++) {
            for (int y = 0; y < MAZE_SIZE; y++) {
                downWall[x, y] = true;
                rightWall[x, y] = true;
                visited[x, y] = false;
            }
        }

        x = 0;
        y = 0;
        visited[x, y] = true;
        visitHistory.Add(new Tuple(0, 0));

        generateMaze();

        rightWall[MAZE_SIZE - 1, MAZE_SIZE - 1] = false;

        for (int x = 0; x < MAZE_SIZE; x++) {
            GameObject newObj = Instantiate(wall_X_prefab) as GameObject;
            newObj.name = "wx(" + (x * 4 + 2) + ",0)";
            newObj.SetActive(true);
            newObj.GetComponent<Transform>().position = new Vector3(x * 4 + 2, 1.5f, 0);
            walls.Add(newObj);
        }

        for (int z = 0; z < MAZE_SIZE; z++) {
            GameObject newObj = Instantiate(wall_Z_prefab) as GameObject;
            newObj.name = "wz(0," + (z * 4 + 2) + ")";
            newObj.SetActive(true);
            newObj.GetComponent<Transform>().position = new Vector3(0, 1.5f, z * 4 + 2);
            walls.Add(newObj);
        }

        for (int x = 0; x < MAZE_SIZE; x++) {
            for (int z = 0; z < MAZE_SIZE; z++) {
                if (downWall[z, x]) {
                    GameObject newObj = Instantiate(wall_Z_prefab) as GameObject;
                    newObj.name = "wx(" + (x * 4 + 4) + "," + (z * 4 + 2) + ")";
                    newObj.SetActive(true);
                    newObj.GetComponent<Transform>().position = new Vector3(x * 4 + 4, 1.5f, z * 4 + 2);
                    walls.Add(newObj);
                }
                if (rightWall[z, x]) {
                    GameObject newObj = Instantiate(wall_X_prefab) as GameObject;
                    newObj.name = "wz(" + (x * 4 + 2) + "," + (z * 4 + 4) + ")";
                    newObj.SetActive(true);
                    newObj.GetComponent<Transform>().position = new Vector3(x * 4 + 2, 1.5f, z * 4 + 4);
                    walls.Add(newObj);
                }
            }
        }



    }

    void generateMaze() {
        while (!isCompleted()) {
            if (!isSurrounded()) {
                historyMarker = visitHistory.Count - 1;
                visited[x, y] = true;
                randInt = Random.Range(0, 4);
                if (randInt == 0) {
                    if (y > 0) {
                        if (downWall[x, y - 1] && !visited[x, y - 1]) {
                            downWall[x, y - 1] = false;
                            y -= 1;
                            visitHistory.Add(new Tuple(x, y));
                        }
                    }
                } else if (randInt == 1) {
                    if (x > 0) {
                        if (rightWall[x - 1, y] && !visited[x - 1, y]) {
                            rightWall[x - 1, y] = false;
                            x -= 1;
                            visitHistory.Add(new Tuple(x, y));
                        }
                    }
                } else if (randInt == 2) {
                    if (y < MAZE_SIZE - 1) {
                        if (downWall[x, y] && !visited[x, y + 1]) {
                            downWall[x, y] = false;
                            y += 1;
                            visitHistory.Add(new Tuple(x, y));
                        }
                    }
                } else {
                    if (x < MAZE_SIZE - 1) {
                        if (rightWall[x, y] && !visited[x + 1, y]) {
                            rightWall[x, y] = false;
                            x += 1;
                            visitHistory.Add(new Tuple(x, y));
                        }
                    }
                }
                visited[x, y] = true;
            } else {
                historyMarker--;
                Tuple mark = visitHistory[historyMarker];
                x = mark.getX();
                y = mark.getY();
            }
        }

    }

    public bool isCompleted() {
        bool complete = true;
        if (x == 0 && y == 0) {
            for (int i = 0; i < MAZE_SIZE; i++) {
                for (int j = 0; j < MAZE_SIZE; j++) {
                    if (!visited[i, j]) {
                        complete = false;
                        break;
                    }
                }
            }
        } else {
            complete = false;
        }
        return complete;
    }


    public bool isSurrounded() {

        bool upVisited = false;
        bool downVisited = false;
        bool leftVisited = false;
        bool rightVisited = false;

        if (x == 0) {
            leftVisited = true;
            rightVisited = visited[x + 1, y];
        } else if (x == MAZE_SIZE - 1) {
            leftVisited = visited[x - 1, y];
            rightVisited = true;
        } else {
            leftVisited = visited[x - 1, y];
            rightVisited = visited[x + 1, y];
        }

        if (y == 0) {
            upVisited = true;
            downVisited = visited[x, y + 1];
        } else if (y == MAZE_SIZE - 1) {
            upVisited = visited[x, y - 1];
            downVisited = true;
        } else {
            upVisited = visited[x, y - 1];
            downVisited = visited[x, y + 1];
        }

        return (upVisited && downVisited && leftVisited && rightVisited);
    }




    // Update is called once per frame
    void Update()
    {
        
    }

    



}
