using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMazeGenerator : MonoBehaviour
{

    public bool[,] downWall;
    public bool[,] rightWall;
    public bool[,] visited;
   // public List<Tuple> visitHistory;
    public Dictionary<Wall, GameObject> wallPool;
   // public List<Tuple> activeCellMap;

    public int historyMarker;
    
    public int x;
    public int y;

    public GameObject player;
    public int playerChunkX = 0;
    public int playerChunkZ = 0;

    int randInt;

    public const int MAZE_SIZE = 60;
    public const int CHUNK_SIZE = 6;

    public GameObject wall_X_prefab;
    public GameObject wall_Z_prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        downWall = new bool[MAZE_SIZE, MAZE_SIZE];
        rightWall = new bool[MAZE_SIZE, MAZE_SIZE];
        visited = new bool[MAZE_SIZE, MAZE_SIZE];
        visitHistory = new List<Tuple>();
        historyMarker = 0;
        wallPool = new Dictionary<Wall, GameObject>();
        activeCellMap = new List<Tuple>();

        int cnt = 0;

        for (int x = 0; x < CHUNK_SIZE * 3; x++) {
            for (int y = 0; y < CHUNK_SIZE * 3; y++) {
                GameObject newObjx = Instantiate(wall_X_prefab) as GameObject;
                newObjx.name = cnt.ToString();
                newObjx.SetActive(true);
                newObjx.GetComponent<Transform>().position = new Vector3(0, -10f, 0);
                wallPool.Add(new Wall(cnt, 0, 0, 0, false), newObjx);
                cnt++;

                GameObject newObjz = Instantiate(wall_Z_prefab) as GameObject;
                newObjz.name = cnt.ToString();
                newObjz.SetActive(true);
                newObjz.GetComponent<Transform>().position = new Vector3(0, -10f, 0);
                wallPool.Add(new Wall(cnt, 0, 0, 1, false), newObjz);
                cnt++;
            }
        }

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
            newObj.name = "borderX " + x.ToString();
            newObj.SetActive(true);
            newObj.GetComponent<Transform>().position = new Vector3(x + 0.5f, 0.5f, 0);
        }

        for (int z = 0; z < MAZE_SIZE; z++) {
            GameObject newObj = Instantiate(wall_Z_prefab) as GameObject;
            newObj.name = "borderZ " + z.ToString();
            newObj.SetActive(true);
            newObj.GetComponent<Transform>().position = new Vector3(0f, 0.5f, z + 0.5f);
        }

        checkPlayerChunk(player);
        reloadChunks();
        */
    }

    void generateMaze() {
        /*
        while (!isCompleted()) {
            if (!isSurrounded()) {
                historyMarker = visitHistory.Count - 1;
                visited[x, y] = true;
                randInt = UnityEngine.Random.Range(0, 4);
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
        */

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
        int prevX = playerChunkX;
        int prevZ = playerChunkZ;
        if (isPlayerChunkChanged(prevX, prevZ)) {
            reloadChunks();
            Debug.Log("Change in Chunk!");
        }
    }

    private void checkPlayerChunk(GameObject plr) {
        playerChunkX = (int)(plr.GetComponent<Transform>().position.x / CHUNK_SIZE);
        playerChunkZ = (int)(plr.GetComponent<Transform>().position.z / CHUNK_SIZE);
    }

    private bool isPlayerChunkChanged(int prevX, int prevZ) {
        bool retVal = false;
        // Checks the player's current chunk
        checkPlayerChunk(player);
        if ((prevX != playerChunkX) || (prevZ != playerChunkZ)) {
            // Player's current chunk is different from the previous chunk
            retVal = true;
        }
        return retVal;
    }

    private void mapActiveChunks() {
        /*
        activeCellMap = new List<Tuple>();
        
        for (int px = Math.Max(CHUNK_SIZE * (playerChunkX - 1), 0); px < CHUNK_SIZE * playerChunkX + CHUNK_SIZE * 2; px++) {
            for (int pz = Math.Max(CHUNK_SIZE * (playerChunkZ - 1), 0); pz < CHUNK_SIZE * playerChunkZ + CHUNK_SIZE * 2; pz++) {
                activeCellMap.Add(new Tuple(px, pz));
            }
        }
        */
    }






    private void reloadChunks() {
        /*
        mapActiveChunks();

        foreach (KeyValuePair<Wall, GameObject> item in wallPool) {
            item.Key.deactivate();
        }


        foreach (KeyValuePair<Wall, GameObject> item in wallPool) {
            if (item.Value.GetComponent<Transform>().position.y < 0) {
                continue;
            } else if (!isInRange(item.Key.X, item.Key.Z, playerChunkX, playerChunkZ)) {
                item.Value.GetComponent<Transform>().position = new Vector3(0, -10f, 0);
                item.Key.deactivate();
            }
        }




        foreach (Tuple item in activeCellMap) {

            Debug.Log(item.getX() + " " + item.getY());

            if (downWall[item.getX(), item.getY()] && rightWall[item.getX(), item.getY()]) {
                foreach (KeyValuePair<Wall, GameObject> wall in wallPool) {
                    if (!wall.Key.isActive && wall.Key.TYPE == 0) {
                        wall.Key.activate();
                        wall.Key.X = item.getX();
                        wall.Key.Z = item.getY();
                        wall.Value.GetComponent<Transform>().position = new Vector3(wall.Key.X + 0.5f, 0.5f, wall.Key.Z + 1f);
                        break;
                    }
                }

                foreach (KeyValuePair<Wall, GameObject> wall in wallPool) {
                    if (!wall.Key.isActive && wall.Key.TYPE == 1) {
                        wall.Key.activate();
                        wall.Key.X = item.getX();
                        wall.Key.Z = item.getY();
                        wall.Value.GetComponent<Transform>().position = new Vector3(wall.Key.X + 1f, 0.5f, wall.Key.Z + 0.5f);
                        break;
                    }
                }
            } else if (downWall[item.getX(), item.getY()] && !rightWall[item.getX(), item.getY()]) {
                foreach (KeyValuePair<Wall, GameObject> wall in wallPool) {
                    if (!wall.Key.isActive && wall.Key.TYPE == 1) {
                        wall.Key.activate();
                        wall.Key.X = item.getX();
                        wall.Key.Z = item.getY();
                        wall.Value.GetComponent<Transform>().position = new Vector3(wall.Key.X + 1f, 0.5f, wall.Key.Z + 0.5f);
                        break;
                    }
                }
            } else if (!downWall[item.getX(), item.getY()] && rightWall[item.getX(), item.getY()]) {
                foreach (KeyValuePair<Wall, GameObject> wall in wallPool) {
                    if (!wall.Key.isActive && wall.Key.TYPE == 0) {
                        wall.Key.activate();
                        wall.Key.X = item.getX();
                        wall.Key.Z = item.getY();
                        wall.Value.GetComponent<Transform>().position = new Vector3(wall.Key.X + 0.5f, 0.5f, wall.Key.Z + 1f);
                        break;
                    }
                }
            } else {
                // Ignore
            }

            
        }
        */
    }

    private bool isInRange(int tgX, int tgZ, int playerChunkX, int playerChunkZ) {
        return (Math.Abs((int)(tgX / CHUNK_SIZE) - playerChunkX) < 2)
                    && (Math.Abs((int)(tgZ / CHUNK_SIZE) - playerChunkZ) < 2);
    }


/**

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
**/


}
