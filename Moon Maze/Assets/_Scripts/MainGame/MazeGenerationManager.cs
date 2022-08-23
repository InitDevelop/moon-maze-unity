using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerationManager : MonoBehaviour
{
    // Maze Generation

    public bool[,] downWall;
    public bool[,] rightWall;
    public List<Tuple<int, int>> visitHistory;

    public int MAZE_SIZE = 60;

    int x = 0;
    int y = 0;

    int randInt;

    bool[,] visited;
    int historyMarker;


    // Maze Loading

    public int CHUNK_SIZE = 6;
    public GameObject[] wallPool;

    public GameObject wall_X_prefab;
    public GameObject wall_Z_prefab;

    public GameObject player;
    public int playerChunkX = 0;
    public int playerChunkZ = 0;

    void Start()
    {

        // For Maze Generation
        visitHistory = new List<Tuple<int, int>>();
        downWall = new bool[MAZE_SIZE, MAZE_SIZE];
        rightWall = new bool[MAZE_SIZE, MAZE_SIZE];
        visited = new bool[MAZE_SIZE, MAZE_SIZE];
        historyMarker = 0;

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
        visitHistory.Add(new Tuple<int, int>(x, y));
        GenerateMaze();

        rightWall[MAZE_SIZE - 1, MAZE_SIZE - 1] = false;

        // For Maze Loading

        for (int x = 0; x < MAZE_SIZE; x++)
        {
            GameObject newObj = Instantiate(wall_X_prefab) as GameObject;
            newObj.name = "borderX";
            newObj.SetActive(true);
            newObj.GetComponent<Transform>().position = new Vector3(x + 0.5f, 0.5f, 0);
        }

        for (int z = 0; z < MAZE_SIZE; z++)
        {
            GameObject newObj = Instantiate(wall_Z_prefab) as GameObject;
            newObj.name = "borderZ";
            newObj.SetActive(true);
            newObj.GetComponent<Transform>().position = new Vector3(0f, 0.5f, z + 0.5f);
        }


        wallPool = new GameObject[CHUNK_SIZE * CHUNK_SIZE * 18];
        for (int i = 0; i < CHUNK_SIZE * CHUNK_SIZE * 9; i++)
        {
            GameObject obj = Instantiate(wall_X_prefab) as GameObject;
            obj.name = "WallX";
            obj.SetActive(true);
            obj.GetComponent<Transform>().position = new Vector3(0, -10f, 0);
            wallPool[i] = obj;
        }

        for (int i = CHUNK_SIZE * CHUNK_SIZE * 9; i < CHUNK_SIZE * CHUNK_SIZE * 18; i++)
        {
            GameObject obj = Instantiate(wall_Z_prefab) as GameObject;
            obj.name = "WallZ";
            obj.SetActive(true);
            obj.GetComponent<Transform>().position = new Vector3(0, -10f, 0);
            wallPool[i] = obj;
        }

        CheckPlayerChunk(player);
        ReloadChunks();

        for (int i = 0; i < CHUNK_SIZE; i++)
        {
            Debug.Log(rightWall[i, MAZE_SIZE - 1]);
        }

    }

    bool IsCompleted()
    {
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
        }
        else
        {
            complete = false;
        }
        return complete;
    }

    bool IsSurrounded()
    {
        bool upVisited;
        bool downVisited;
        bool leftVisited;
        bool rightVisited;

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

    void GenerateMaze()
    {
        while (!IsCompleted())
        {
            visited[x, y] = true;
            if (!IsSurrounded())
            {
                historyMarker = visitHistory.Count - 1;
                
                randInt = UnityEngine.Random.Range(0, 4);
                if (randInt == 0)
                {
                    if (y > 0)
                    {
                        if (downWall[x, y - 1] && !visited[x, y - 1])
                        {
                            downWall[x, y - 1] = false;
                            y -= 1;
                            visitHistory.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }
                else if (randInt == 1)
                {
                    if (x > 0)
                    {
                        if (rightWall[x - 1, y] && !visited[x - 1, y])
                        {
                            rightWall[x - 1, y] = false;
                            x -= 1;
                            visitHistory.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }
                else if (randInt == 2)
                {
                    if (y < MAZE_SIZE - 1)
                    {
                        if (downWall[x, y] && !visited[x, y + 1])
                        {
                            downWall[x, y] = false;
                            y += 1;
                            visitHistory.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }
                else
                {
                    if (x < MAZE_SIZE - 1)
                    {
                        if (rightWall[x, y] && !visited[x + 1, y])
                        {
                            rightWall[x, y] = false;
                            x += 1;
                            visitHistory.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }
            }
            else
            {
                historyMarker--;
                Tuple<int, int> vec = visitHistory[historyMarker];
                x = vec.Item1;
                y = vec.Item2;
            }
        }

    }

    void CheckPlayerChunk(GameObject plr)
    {
        playerChunkX = (int)(plr.GetComponent<Transform>().position.x / CHUNK_SIZE);
        playerChunkZ = (int)(plr.GetComponent<Transform>().position.z / CHUNK_SIZE);
    }

    bool IsInRange(int tgX, int tgZ, int playerChunkX, int playerChunkZ)
    {
        return (tgX >= (playerChunkX - 1) * CHUNK_SIZE && tgX < (playerChunkX + 2) * CHUNK_SIZE)
                    && (tgZ >= (playerChunkZ - 1) * CHUNK_SIZE && tgZ < (playerChunkZ + 2) * CHUNK_SIZE);
    }

    void ReloadChunks()
    {
        for (int i = 0; i < wallPool.Length; i++)
        {
            if (wallPool[i].name == "WallX")
            {
                if (!IsInRange((int)(wallPool[i].GetComponent<Transform>().position.x - 0.5f),
                    (int)(wallPool[i].GetComponent<Transform>().position.z - 1f),
                       playerChunkX, playerChunkZ))
                {
                    wallPool[i].GetComponent<Transform>().position = new Vector3(0, -10f, 0);
                }
            }
            else if (wallPool[i].name == "WallZ")
            {
                if (!IsInRange((int)(wallPool[i].GetComponent<Transform>().position.x - 1f),
                    (int)(wallPool[i].GetComponent<Transform>().position.z - 0.5f),
                    playerChunkX, playerChunkZ))
                {
                    wallPool[i].GetComponent<Transform>().position = new Vector3(0, -10f, 0);
                }

            }
        }

        List<Vector2> missing = new List<Vector2>();

        for (int x = Math.Max(CHUNK_SIZE * (playerChunkX - 1), 0); x < Math.Min((playerChunkX + 2) * CHUNK_SIZE, MAZE_SIZE); x++)
        {
            for (int z = Math.Max(CHUNK_SIZE * (playerChunkZ - 1), 0); z < Math.Min((playerChunkZ + 2) * CHUNK_SIZE, MAZE_SIZE); z++)
            {
                if (!missing.Contains(new Vector2(x, z)))
                {
                    missing.Add(new Vector2(x, z));
                }
                
            }
        }

        for (int i = 0; i < wallPool.Length; i++)
        {
            if (wallPool[i].GetComponent<Transform>().position.y >= 0)
            {
                if (wallPool[i].name == "WallX")
                {
                    missing.Remove(new Vector2(
                        (int)(wallPool[i].GetComponent<Transform>().position.x - 0.5f),
                        (int)(wallPool[i].GetComponent<Transform>().position.z - 1f)
                        ));
                }
                else if (wallPool[i].name == "WallZ")
                {
                    missing.Remove(new Vector2(
                        (int)(wallPool[i].GetComponent<Transform>().position.x - 1f),
                        (int)(wallPool[i].GetComponent<Transform>().position.z - 0.5f)
                        ));
                }
            }
        }

        foreach (Vector2 vec in missing)
        {

            // CAUTION!! The x-y system in rightWall and downWall is
            // different from the x-z system in the game coordinates.
            // x in generation code -> z in loading code
            // y in generation code -> x in loading code

            bool isDown = downWall[(int)vec.y, (int)vec.x];
            bool isRight = rightWall[(int)vec.y, (int)vec.x];

            if (!isDown && !isRight)
            {
                //Ignore
            }
            else if (!isDown && isRight)
            {
                for (int i = 0; i < wallPool.Length; i++)
                {
                    if (wallPool[i].GetComponent<Transform>().position.y < 0 && wallPool[i].name == "WallX")
                    {
                        wallPool[i].GetComponent<Transform>().position = new Vector3(vec.x + 0.5f, 0.5f, vec.y + 1f);
                        break;
                    }
                }
            }
            else if (isDown && !isRight)
            {
                for (int i = 0; i < wallPool.Length; i++)
                {
                    if (wallPool[i].GetComponent<Transform>().position.y < 0 && wallPool[i].name == "WallZ")
                    {
                        wallPool[i].GetComponent<Transform>().position = new Vector3(vec.x + 1f, 0.5f, vec.y + 0.5f);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < wallPool.Length; i++)
                {
                    if (wallPool[i].GetComponent<Transform>().position.y < 0 && wallPool[i].name == "WallX")
                    {
                        wallPool[i].GetComponent<Transform>().position = new Vector3(vec.x + 0.5f, 0.5f, vec.y + 1f);
                        break;
                    }
                }
                for (int i = 0; i < wallPool.Length; i++)
                {
                    if (wallPool[i].GetComponent<Transform>().position.y < 0 && wallPool[i].name == "WallZ")
                    {
                        wallPool[i].GetComponent<Transform>().position = new Vector3(vec.x + 1f, 0.5f, vec.y + 0.5f);
                        break;
                    }
                }
            }
        }


    }

    bool IsPlayerChunkChanged(int prevX, int prevZ)
    {
        bool retVal = false;
        // Checks the player's current chunk
        CheckPlayerChunk(player);
        if ((prevX != playerChunkX) || (prevZ != playerChunkZ))
        {
            // Player's current chunk is different from the previous chunk
            retVal = true;
        }
        return retVal;
    }

    void Update()
    {
        int prevX = playerChunkX;
        int prevZ = playerChunkZ;
        if (IsPlayerChunkChanged(prevX, prevZ))
        {
            ReloadChunks();
            Debug.Log("Change in Chunk!");
        }
    }
}
