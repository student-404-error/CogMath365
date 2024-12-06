using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using TMPro;
public class LoginSystem : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField email;
    public TMP_InputField pass;
    private TMP_Text outputText;
    private FirebaseAuth auth;
    private SceneManager sceneManager;
    private DateSave dataSave;
    private GameManager mGameManager;
    
    
    void Start()
    {
        // GameObject Setting
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        dataSave = GameObject.Find("DataManager").GetComponent<DateSave>();
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // auto Login
        auth = FirebaseAuth.DefaultInstance;
        // auth.SignOut();
        if (auth.CurrentUser != null)
        {
            // 이미 로그인된 유저가 있다면, 다음 화면으로 이동 등의 처리를 해줄 수 있습니다.
            sceneManager.MoveMain();
            // LogOut();
            // auth.SignOut();
        }
        else
        {
            // dataSave.SetName(nameInput.text);
            FirebaseAuthManager.Instance.loginState += OnChangedState;
            FirebaseAuthManager.Instance.Init();
        }
        // LogOut();
    }

    private void OnChangedState(bool sign)
    {
        outputText.text = sign ? "Login : " : "Logout : ";
        outputText.text += FirebaseAuthManager.Instance.UserId;

    }

    public void Create()
    {
        string e = email.text;
        string p = pass.text;
        
        FirebaseAuthManager.Instance.Create();
    }

    public void Login()
    {   
        FirebaseAuthManager.Instance.Login();
    }

    public void LogOut()
    {
        FirebaseAuthManager.Instance.Logout();
    }
}
