using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public class UserData
    {
        public string Username { get; set; }
        public float Score { get; set; }

        public UserData(string username, float score)
        {
            Username = username;
            Score = score;
        }
    }

    private DatabaseReference mReference;
    private GameManager mGameManager;
    private string userName;
    public GameObject RankingImagePrefab;
    public Transform contentPanel;
    public List<TextMeshProUGUI> textComponents = new List<TextMeshProUGUI>(3);
    private int rankIndex = 1;
    private string username;
    private float score;
    
    public TMP_Text currentRankText;
    public TMP_Text currentUserText;
    public TMP_Text currentScoreText;
    private int currentRank = 0;
    private float currentScore = 0;

    private List<UserData> allUsers = new List<UserData>(); // 전체 유저 정보 저장

    async void Start()
    {
        Debug.Log("start Ranking Manager");
        await FirebaseApp.CheckAndFixDependenciesAsync();
        FirebaseApp app = FirebaseApp.DefaultInstance;
        mReference = FirebaseDatabase.GetInstance(app, "https://calcgame-fffc9-default-rtdb.firebaseio.com").RootReference;

        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        userName = mGameManager.userName;
        LoadLeaderboard();
    }

    // 1등부터 5등까지의 랭킹 창을 만드는 함수
    void MakeRankBar(string userName, float userScore)
    {
        GameObject clone = Instantiate(RankingImagePrefab, contentPanel);
        TextMeshProUGUI[] tmpTextComponents = clone.GetComponentsInChildren<TextMeshProUGUI>();
        textComponents.Add(tmpTextComponents[0]);
        textComponents.Add(tmpTextComponents[1]);
        textComponents[0 + (rankIndex - 1) * 2].text = userName;
        textComponents[1 + (rankIndex - 1) * 2].text = userScore.ToString("F1");
    }

    // 리더보드를 불러오는 함수
    void LoadLeaderboard()
    {
        Debug.Log("start LoadLeaderboard"); //comment
        try
        {
            mReference.GetValueAsync().ContinueWithOnMainThread(task => {
                if (!task.IsCompleted)
                {
                    Debug.Log("task is not complete"); //comment
                    return;
                }
                Debug.Log("task is complete"); //comment

                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    Debug.Log("userSnapshot : " + userSnapshot); //comment
                    string tempUsername = userSnapshot.Key;
                    Debug.Log("1"); //comment
                    
                    // 모든 유저정보 하위에 있는 Score 값을 가져온 후, allUsers 배열에 집어 넣기
                    if (userSnapshot.HasChild("Score"))
                    {
                        Debug.Log("2"); //comment
                        float tempScore = float.Parse(userSnapshot.Child("Score").Value.ToString());
                        allUsers.Add(new UserData(tempUsername, tempScore));
                        Debug.Log("tempUsername : " + tempUsername + "  tempScore : " + tempScore); //comment
                    }
                }
                
                // 유저 정보를 점수를 기준으로 오름차순 정렬(점수가 음수이기 때문에 오름차순 정렬하면 절댓값이 큰 수가 첫 번째로 온다)
                allUsers.Sort((a, b) => a.Score.CompareTo(b.Score));
                Debug.Log("sorted Data!"); //comment

                for (int i = 0; i < Mathf.Min(5, allUsers.Count); i++)
                {
                    Debug.Log("Making RankingBar " + (i + 1)); //comment
                    MakeRankBar(allUsers[i].Username, -allUsers[i].Score); // 점수가 음수이기 때문에 출력할때는 음수로 출력
                    rankIndex++;
                }

                // 현재 유저의 정보를 탐색
                for (int i = 0; i < allUsers.Count; i++)
                {
                    Debug.Log("Finding Username..."); //comment
                    if (allUsers[i].Username == userName)
                    {
                        Debug.Log("Found Username!"); //comment
                        currentRank = i + 1;
                        currentScore = -allUsers[i].Score; // 데이터의 점수는 음수이므로 양수로 바꾸어 저장
                        break;
                    }
                }
                
                LoadPersonalBoard(); // 유저 개인의 정보 출력
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // 유저 개인의 정보 출력
    void LoadPersonalBoard()
    {
        Debug.Log("Complete Loading PersonalBoard!");
        currentRankText.SetText(currentRank.ToString("N0"));
        currentUserText.SetText(userName);
        currentScoreText.SetText(currentScore.ToString("F1"));
    }
}
