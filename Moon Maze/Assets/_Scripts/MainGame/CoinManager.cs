using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{

    public Dictionary<(int, int), GameObject> coins;
    public int coinCount = 20;
    public List<(int, int)> tuples;
    public GameObject coin;
    public Transform playerTransform;
    public int UPDATE_TICKS;
    public Text coinCountText;
    public int points;

    void Start()
    {
        points = 0;
        UPDATE_TICKS = 0;
        coins = new Dictionary<(int, int), GameObject>();
        tuples = new List<(int, int)>();

        int randX;
        int randZ;

        for (int i = 0; i < coinCount; i++) {
            randX = Random.Range(0, AutomaticMazeGenerator.MAZE_SIZE);
            randZ = Random.Range(0, AutomaticMazeGenerator.MAZE_SIZE);

            if (!tuples.Contains((randX, randZ))) {
                tuples.Add((randX, randZ));
            }
        }

        foreach ((int, int) tup in tuples) {
            GameObject obj = Instantiate(coin) as GameObject;
            obj.name = "coin(" + (tup.Item1 * 4 + 2) + "," + (tup.Item2 * 4 + 2) + ")";
            obj.SetActive(true);
            obj.GetComponent<Transform>().position = new Vector3(tup.Item1 * 4 + 2, -0.45f, tup.Item2 * 4 + 2);
            coins.Add((tup.Item1, tup.Item2), obj);
        }

    }

    void Update()
    {
        UPDATE_TICKS++;

        if (UPDATE_TICKS % 20 == 0) {
            foreach ((int, int) tup in tuples) {
                if (Mathf.Abs(playerTransform.position.x - (4 * tup.Item1 + 2)) < 1.0f) {
                    if (Mathf.Abs(playerTransform.position.z - (4 * tup.Item2 + 2)) < 1.0f) {
                        coins[(tup.Item1, tup.Item2)].SetActive(false);
                        points += 50;
                        // Removing coin from tuple registry
                        tuples.Remove(tup);
                        coinCountText.text = "POINTS = " + points;
                    }
                }
            }
        }

        

        

    }



}
