using UnityEngine;
using System;
using TMPro;

public class RealTimeClock : MonoBehaviour
{
    public TMP_Text playTimeText;
    private GameManager mGameManager;
    private void Start()
    {
        // Text 컴포넌트를 가져옵니다.
        playTimeText= GameObject.Find("Time").GetComponent<TMP_Text>();
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // 매 초마다 현재 시간을 업데이트합니다.
        // InvokeRepeating("UpdateClock", 0f, 1f);
    }

    private void Update()
    {
        mGameManager.showTime += Time.deltaTime;
        UpdatePlayTimeText();
    }

    // private void UpdateClock()
    // {
    //     // 현재 시간을 가져와서 텍스트로 변환하여 업데이트합니다.
    //     DateTime currentTime = DateTime.Now;
    //     string timeString = currentTime.ToString("HH:mm:ss");
    //     timeText.text = timeString;
    // }
    private void UpdatePlayTimeText()
    {
        // 총 게임 진행 시간을 시, 분, 초로 변환하여 텍스트로 업데이트합니다.
        int hours = Mathf.FloorToInt( mGameManager.showTime / 3600);
        int minutes = Mathf.FloorToInt((mGameManager.showTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(mGameManager.showTime % 60);
        playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}