using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public class Logout : MonoBehaviour
{
    private FirebaseAuth auth;
    public void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    // Start is called before the first frame update
    public void LogOut()
    {
        auth.SignOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        // FirebaseAuthManager.Instance.Logout();
    }

    public void Main()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }
}
