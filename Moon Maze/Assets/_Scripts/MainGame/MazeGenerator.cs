using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{   
    
    public GameObject wallPrefab;
    public GameObject[] wallPool;
    public Texture2D[] prefabs = new Texture2D[1];

    // Start is called before the first frame update
    void Start()
    {

        wallPool = new GameObject[400];

        int count = 0;

        for (int i = 0; i < prefabs.Length; i++) {
            print(prefabs[i].width + " / " + prefabs[i].height);
            for (int x = 0; x < prefabs[i].width; x++) {
                for (int z = 0; z < prefabs[i].height; z++) {
                    if (prefabs[i].GetPixel(x, z) == Color.black) {
                        print("Found!");
                        GameObject newObj = Instantiate(wallPrefab) as GameObject;
                        newObj.SetActive(true);
                        newObj.GetComponent<Transform>().position = new Vector3(x, 0.5f, z);
                        wallPool[count] = newObj;
                        count++;
                    }
                }
            }
        }

        print(count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
