using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFPS : MonoBehaviour
{
    // Start is called before the first frame update
    float FPS = 0f;
    int TICKS = 0;

    public Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TICKS++;
        if (TICKS % 100 == 0) {
            FPS = 1 / Time.deltaTime;
        text.text = "FPS = " + (int)FPS;
        }
        
    }
}
