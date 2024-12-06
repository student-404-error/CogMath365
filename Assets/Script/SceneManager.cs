using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    public string mvScene;
    public void MoveMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
    public void moveStage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
    }
    
}
