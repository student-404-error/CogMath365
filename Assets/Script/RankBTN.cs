using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankBTN : MonoBehaviour
{
    private SceneManager mSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        mSceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }
    
    public void GoMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    } 
}
