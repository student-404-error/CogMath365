using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainBTN : MonoBehaviour
{
    private GameManager mGameManager;
    private SceneManager mSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        mSceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnButtonClick(Button clickedButton)
    {
        string buttonText = clickedButton.GetComponentInChildren<TMP_Text>().name;
        mGameManager.levelNum = int.Parse(buttonText);
        mSceneManager.moveStage();
        Debug.Log(mGameManager.levelNum);
    }

    public void GoRank()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Rank");
    }
    // Update is called once per frame
}
