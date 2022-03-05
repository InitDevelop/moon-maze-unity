using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayButtonEvent : MonoBehaviour
{
    public void onButtonPressed() {
        SceneManager.LoadScene(1);
    }
}
