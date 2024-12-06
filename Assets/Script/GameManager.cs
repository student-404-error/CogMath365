using System;
using System.Threading.Tasks;
using Firebase;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class GameManager : MonoBehaviour
{
    private int mAttend;
    public int levelNum;
    public string userName;
    private string mDate;
    public float showTime;
    private DatabaseReference mReference;
    private FirebaseAuth mAuth;

    public async void Start()
    {
        DontDestroyOnLoad(gameObject);

        DateTime dateTime = DateTime.Now;

        mDate = dateTime.ToString("yyyy-MMMM-dd");
        FirebaseApp app = FirebaseApp.DefaultInstance;
        mReference = FirebaseDatabase.GetInstance(app, "https://calcgame-fffc9-default-rtdb.firebaseio.com")
            .RootReference;
        mAuth = FirebaseAuth.DefaultInstance;
        if (mAuth.CurrentUser == null) return;

        userName = mAuth.CurrentUser.Email.Replace("@gmail.com", "");
        Debug.Log(mAuth.CurrentUser.Email);
        await UpdateAttendanceAsync();
    }
    private async Task UpdateAttendanceAsync()
    {
        
        try
        {
            await mReference
                .Child(userName)
                .Child("Attend")
                .Child(mDate)
                .SetValueAsync(true);
            Debug.Log("출석 데이터가 업데이트되었습니다.");
        }
        catch (Exception e)
        {
            // 오류 처리
            Debug.LogError("Firebase 데이터베이스 출석 데이터 업데이트 오류: " + e.Message);
        }
    }

}
