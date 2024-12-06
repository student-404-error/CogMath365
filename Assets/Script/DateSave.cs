using UnityEngine;
using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class DateSave : MonoBehaviour
{
    private string mUserName;
    private string mDate;
    private DatabaseReference mReference;
    private GameManager mGameManager;

    private async void Start()
    {
        // await DoAsyncWork();
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            mReference = FirebaseDatabase.GetInstance(app, "https://calcgame-fffc9-default-rtdb.firebaseio.com").RootReference;
            // Firebase 초기화 및 종속성 확인 작업이 완료된 후에 실행될 코드
            // InitializeFirebaseData();
        });
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private async Task DoAsyncWork()
    {
        // 비동기 작업 수행
        await Task.Delay(500); // 예시로 1초 대기
        Debug.Log("Async work completed.");
    }
    public async void InitializeFirebaseData()
    {
        await DoAsyncWork();
        DateTime dateTime = DateTime.Now;
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mUserName = mGameManager.userName;
        mDate = dateTime.ToString("yyyy-MMMM-dd");
        Debug.Log(mUserName);
        if (mUserName == null) return;
        await mReference
            .Child(mUserName)
            .Child(mDate)
            .Child("Problem")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result.Value == null)
                {
                    InitData();
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Data Initialized Successful");
                }
            });
        await mReference
            .Child(mUserName)
            .Child("Score")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result.Value == null)
                {
                    SetScore();
                    Debug.Log("set Score 0!!");
                }
            });
    }

    // public void SetName(string name)
    // {
    //     mReference
    //         .Child(mUserName)
    //         .Child("Name")
    //         .SetValueAsync(name);
    // }

    private void SetScore()
    {
        mReference.Child(mUserName).Child("Score").SetValueAsync(0.0);
    }
    private void InitData()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
            }
            else
            {
                mReference = FirebaseDatabase.DefaultInstance.RootReference;

                // 데이터 생성
                DateData dateData = new DateData
                {
                    Problem = new ProblemData
                    {
                        S01 = new StageData(),
                        S02 = new StageData(),
                        S03 = new StageData(),
                        S04 = new StageData(),
                        S05 = new StageData(),
                        S06 = new StageData(),
                        S07 = new StageData(),
                        S08 = new StageData(),
                        S09 = new StageData(),
                        S10 = new StageData(),
                        S11 = new StageData(),
                        S12 = new StageData(),
                        S13 = new StageData(),
                        S14 = new StageData(),
                    }
                };
                string json = JsonUtility.ToJson(dateData);
                mReference.Child(mUserName).Child(mDate).SetRawJsonValueAsync(json).ContinueWithOnMainThread(saveTask =>
                {
                    if (saveTask.Exception != null)
                    {
                        Debug.Log("Failed to save data: " + saveTask.Exception);
                    }
                    else
                    {
                        Debug.Log("Data saved successfully!");
                    }
                });
            }
        });
    }
}
