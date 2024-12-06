using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public GameObject popup;
    public GameObject correctNumberGauge;
    public GameObject explainImageChoose;
    public GameObject explainImageCalc;
    public GameObject goodFaceprefab;
    public GameObject badFaceprefab;
    private GameObject instantPrefab;
    private Vector2 createPoint;
    
    public TMP_Text problemText;
    public TMP_Text leftButtonText;
    public TMP_Text rightButtonText;
    public TMP_Text levelText;
    public TMP_Text scoreText;
    
    
    public int playedRound = 0;
    public float accuracy = 0;
    private bool isFeedbackShowing = false;    
    // private int num1;
    // private int num2;
    private bool mIsRightSign;
    private char mStrictInequality;
    public int score = 0;
    private bool mCorrectAnswerBool;
    private int mCorrectAnswerInt;
    public int levelNum;
    public float startTime;
    public float playTime;
    public float solvedSpeed;

    // 게임 종료 후 출력하는 텍스트
    public TMP_Text AccuracyText;
    public TMP_Text QuestionNumberText;
    public TMP_Text PlayTimeText;
    public TMP_Text AverageSolveTimeText;
    public TMP_Text CorrectNumberPercent;
    private GameManager mGameManager;
    private void Start()
    {
        popup.SetActive(false); // 팝업창 끄기
        if (instantPrefab != null)
        {
            Destroy(instantPrefab);
        }
        // 프리팹 생성 위치
        createPoint.x = 0.0f;
        createPoint.y = 0.0f;
        
        startTime = Time.realtimeSinceStartup; // 시작시간 측정
        
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        levelNum = mGameManager.levelNum; // 임시로 레벨을 지정 (추후 스테이지 선택에서 값을 받을예정)
        levelText.SetText(levelNum.ToString()); // 현재 레벨 표시
        
        if (levelNum is 1 or 3 or 5 or 8) // 대소비교 연산 게임인 경우
        {
            explainImageChoose.SetActive(true);
            explainImageCalc.SetActive(false);
        }
        else
        {
            explainImageChoose.SetActive(false);
            explainImageCalc.SetActive(true);
        }
        
        switch (levelNum)
        {
            case 1: GenerateStageOne(); break;
            case 2: GenerateStageTwo(); break;
            case 3: GenerateStageThree(); break;
            case 4: GenerateStageFour(); break;
            case 5: GenerateStageFive(); break;
            case 6: GenerateStageSix(); break;
            case 7: GenerateStageSeven(); break;
            case 8: GenerateStageEight(); break;
            case 9: GenerateStageNine(); break;
            case 10: GenerateStageTen(); break;
            case 11: GenerateStageEleven(); break;
            case 12: GenerateStageTwelve(); break;
            case 13: GenerateStageThirteen(); break;
            case 14: GenerateStageFourteen(); break;
            default: Debug.LogError("Invalid level"); break;
        }
    }

    // 원형 그래프 채우는 애니메이션
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator AnimateGraph(float targetAccuracy, float duration)
    {
        float tempGraphPercent = 0;
        float percentage = 0;

        for (float time = duration; time > 0; time -= Time.deltaTime)
        {
            float deltaAmount = (1 - time * time * 4) * targetAccuracy;
            this.correctNumberGauge.GetComponent<Image>().fillAmount += deltaAmount - tempGraphPercent;
            percentage += (deltaAmount - tempGraphPercent) * 100;
            tempGraphPercent = deltaAmount; // 이전 프레임까지 늘어난 양

            
            CorrectNumberPercent.SetText("Accuracy\n" + percentage.ToString("N0") + "%");

            yield return null;
        }
    }

    private async Task DoAsyncWork(int sec)
    {
        await Task.Delay(sec);
        Debug.Log("Async work completed.");
    }
    // 끝나는 게임창을 보여주는 함수
    public void ShowEndResult()
    {
        
        playTime = Time.realtimeSinceStartup - startTime; // 끝나는 시간 측정

        if (score != 0)
        {
            accuracy = (float)score / playedRound * 100;
            accuracy = Mathf.Round((accuracy * 10f) / 10f);
        }
        
        solvedSpeed = ((float)playTime / playedRound); // 1 문제당 걸린 시간
        StartCoroutine(AnimateGraph(accuracy / 100, 0.5f));  

        AccuracyText.SetText("Accuracy\n" + accuracy.ToString("F1") + "%");                     // 정확도
        QuestionNumberText.SetText("# of Question\n" + playedRound);                                  // 문제 개수
        PlayTimeText.SetText("PlayTime\n" + playTime.ToString("N0") + "sec");                   // 플레이 시간
        AverageSolveTimeText.SetText("Avg. Speed\n" + solvedSpeed.ToString("N0") + "sec");      // 평균 풀이 시간
        
        

    }
    public void CheckAnswer(bool playerChoice)
    {
        playedRound++; // 플레이 횟수 증가
        if (playerChoice == mCorrectAnswerBool)
        {
            Debug.Log("Correct!");
            // 정답일 때 실행할 코드를 여기에 추가
            score++;
            scoreText.SetText(score.ToString());
        }
        else
        {
            Debug.Log("Wrong!");
            // 오답일 때 실행할 코드를 여기에 추가
        }

        // 반응 화면 출력
        ShowFeedback(playerChoice == mCorrectAnswerBool);
    }

    private void ShowFeedback(bool isCorrect)
    {
        if (isFeedbackShowing) return; // 이미 피드백 중이면 중복 실행 방지
        
        isFeedbackShowing = true; // 피드백 표시 중임을 표시
        
        StartCoroutine(DisplayFeedback(isCorrect));
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DisplayFeedback(bool isCorrect)
    {
        instantPrefab = Instantiate(isCorrect ? goodFaceprefab : badFaceprefab, createPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
        // 일정 시간(예: 1초) 동안 대기
        yield return new WaitForSecondsRealtime(1f);

        isFeedbackShowing = false; // 피드백 종료
        CreateNewQuestion();
    }
    private void CreateNewQuestion()
    {
        if (instantPrefab != null)
        {
            Destroy(instantPrefab);
        }
        
        switch (levelNum)
        {
            case 1: GenerateStageOne(); break;
            case 2: GenerateStageTwo(); break;
            case 3: GenerateStageThree(); break;
            case 4: GenerateStageFour(); break;
            case 5: GenerateStageFive(); break;
            case 6: GenerateStageSix(); break;
            case 7: GenerateStageSeven(); break;
            case 8: GenerateStageEight(); break;
            case 9: GenerateStageNine(); break;
            case 10: GenerateStageTen(); break;
            case 11: GenerateStageEleven(); break;
            case 12: GenerateStageTwelve(); break;
            case 13: GenerateStageThirteen(); break;
            case 14: GenerateStageFourteen(); break;
            default: Debug.LogError("Invalid level"); break;
        }
    }
    private void GenerateStageOne()
    {
        int num1 = Random.Range(0, 10);
        int num2 = num1;
        while (true)
        {
            num2 = Random.Range(0, 10);
            if (num1 != num2) break;
        }
        
        mCorrectAnswerBool = num1 > num2;

        // 문제를 출력
        problemText.SetText(num1.ToString() + "     " + num2.ToString());
        leftButtonText.SetText(num1.ToString());
        rightButtonText.SetText(num2.ToString());
    }
    private void GenerateStageTwo()
    {
        int num1 = Random.Range(0, 10);
        int num2 = Random.Range(0, 10);

        mCorrectAnswerInt = num1 + num2;
        int wrongAnswer = mCorrectAnswerInt;

        while(mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(0, 20); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n +   " + num2.ToString());

        if (mIsRightSign) // right is Correct
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // left is Correct
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageThree()
    {
        int num1 = Random.Range(0, 10);
        int num2 = Random.Range(0, 10);
        int num3 = num1;
        int num4 = num2;
        while (true)
        {
            num3 = Random.Range(0, 10);
            num4 = Random.Range(0, 10);
            if (num1 + num2 != num3 + num4) break;
        }

        int leftResult = num1 + num2;
        int rightResult = num3 + num4;

        mCorrectAnswerBool = leftResult > rightResult;

        // 문제를 출력
        problemText.SetText(num1.ToString() + " + " + num2.ToString() + "   " + num3.ToString() + " + " + num4.ToString());
        leftButtonText.SetText(num1.ToString() + " + " + num2.ToString());
        rightButtonText.SetText(num3.ToString() + " + " + num4.ToString());
    }
    private void GenerateStageFour()
    {
        int num1 = Random.Range(0, 10);
        int num2 = num1 + 1; // 무조건 while문을 돌게 값을 설정

        while (num1 < num2)  // num1이 항상 크도록 num2 설정
        {
            num2 = Random.Range(0, 10);
        }
        

        mCorrectAnswerInt = num1 - num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(0, 10); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n -   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageFive()
    {
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        int num3 = Random.Range(1, 10);
        int num4 = Random.Range(1, 10);
        
        while (num1 - num2 == num3 - num4 || num1 < num2 || num3 < num4)
        {
            num2 = Random.Range(0, 10);
            num4 = Random.Range(0, 10);
        }

        int leftResult = num1 - num2;
        int rightResult = num3 - num4;

        mCorrectAnswerBool = leftResult > rightResult;

        // 문제를 출력
        problemText.SetText(num1.ToString() + " - " + num2.ToString() + "   " + num3.ToString() + " - " + num4.ToString());
        leftButtonText.SetText(num1.ToString() + " - " + num2.ToString());
        rightButtonText.SetText(num3.ToString() + " - " + num4.ToString());
    }
    private void GenerateStageSix()
    {
        int num1 = Random.Range(10, 100);
        int num2 = num1;
        while (true)
        {
            num2 = Random.Range(10, 100);
            if (num1 % 10 >= num2 % 10 && num1 > num2) break;
        }

        mCorrectAnswerInt = num1 - num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(0, 100); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n -   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }

    }
    private void GenerateStageSeven()
    {
        int num1 = Random.Range(10, 100);
        int num2 = num1;
        while (true)
        {
            num2 = Random.Range(10, 100);
            if (num1 % 10 >= num2 % 10 && num1 > num2) break;
        }


        mCorrectAnswerInt = num1 - num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(0, 100); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n -   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageEight()
    {
        int num1 = Random.Range(10, 100);
        int num2 = Random.Range(10, 100);
        int num3 = num1;
        int num4 = num2;
        while (true)
        {
            num3 = Random.Range(10, 100);
            num4 = Random.Range(10, 100);
            if (num3 >= num4 && num3 % 10 >= num4 % 10) break;
        }

        int leftResult = num1 + num2;
        int rightResult = num3 - num4;

        mCorrectAnswerBool = leftResult > rightResult;

        // 문제를 출력
        problemText.SetText(num1.ToString() + " + " + num2.ToString() + "   " + num3.ToString() + " - " + num4.ToString());
        leftButtonText.SetText(num1.ToString() + " + " + num2.ToString());
        rightButtonText.SetText(num3.ToString() + " - " + num4.ToString());
    }
    private void GenerateStageNine()
    {
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);

        mCorrectAnswerInt = num1 * num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(1, 82); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n X   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageTen()
    {
        int num1 = Random.Range(100, 1000);
        int num2 = Random.Range(100, 1000);

        mCorrectAnswerInt = num1 + num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(200, 2000); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n +   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageEleven()
    {
        int num1 = Random.Range(100, 1000);
        int num2 = num1;
        while (true)
        {
            num2 = Random.Range(100, 1000);
            if (num1 % 10 >= num2 % 10 && (num1 / 10) % 10 >= (num2 / 10) % 10 && num1 > num2) break; // 받아내림 제거
        }

        mCorrectAnswerInt = num1 - num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(100, 1000); // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n -   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageTwelve()
    {
        int num1 = Random.Range(10, 100);
        int num2 = Random.Range(1, 10);

        mCorrectAnswerInt = num1 * num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(1, 100) * 10 + mCorrectAnswerInt % 10; // 잘못된 정답 만들기 (정답과 일의자리 수 일치)
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n X   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageThirteen()
    {
        int num1 = Random.Range(1000, 5000);
        int num2 = Random.Range(1000, 5000);

        mCorrectAnswerInt = num1 + num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(100, 1000) * 10 + mCorrectAnswerInt % 10; // 잘못된 정답 만들기 (정답과 일의자리 수 일치)
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n +   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
    private void GenerateStageFourteen()
    {
        int num1 = Random.Range(1000, 10000);
        int num2 = num1;
        while (true)
        {
            num2 = Random.Range(1000, 10000);
            if (num1 % 10 >= num2 % 10 && (num1 / 10) % 10 >= (num2 / 10) % 10 
                                       && (num1 / 100) % 10 >= (num2 / 100) % 10 && num1 > num2) break; // 받아내림 제거
        }

        mCorrectAnswerInt = num1 - num2;
        int wrongAnswer = mCorrectAnswerInt;

        while (mCorrectAnswerInt == wrongAnswer) // 중복 방지
        {
            wrongAnswer = Random.Range(0, 900) * 10 + mCorrectAnswerInt % 10; // 잘못된 정답 만들기
        }

        mIsRightSign = Random.Range(0, 2) == 0; // 정답의 방향을 결정 (참이면 오른쪽)

        // 문제를 출력
        problemText.SetText(num1.ToString() + "\n -   " + num2.ToString());

        if (mIsRightSign) // 정답이 오른쪽
        {
            mCorrectAnswerBool = true;
            leftButtonText.SetText(mCorrectAnswerInt.ToString());
            rightButtonText.SetText(wrongAnswer.ToString());
        }
        else             // 정답이 왼쪽
        {
            mCorrectAnswerBool = false;
            leftButtonText.SetText(wrongAnswer.ToString());
            rightButtonText.SetText(mCorrectAnswerInt.ToString());
        }
    }
}
