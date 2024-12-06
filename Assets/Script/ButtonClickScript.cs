using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickScript : MonoBehaviour
{
    public LevelManager levelManager;
    public Button button; // 버튼 컴포넌트 참조
    private Color originalColor; // 원래 색상 저장
    public Color colorFromHex;
    private bool isChangingColor = false; // 색상 변경 중 여부
    public void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        originalColor = button.image.color;
    }

    public void OnLeftButtonClicked() 
    {
        levelManager.CheckAnswer(true);
    }

    public void OnRightButtonClicked() 
    {
        levelManager.CheckAnswer(false);
    }

    public void ChangeColor()
    {
        if (!isChangingColor)
        {
            // 16진수 컬러 코드를 Color 객체로 변환
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#928A5B", out newColor))
            {
                // 버튼 색상을 변경
                button.image.color = newColor; // 원하는 16진수 컬러 코드를 여기에 넣어주세요

                // 2초 후에 색상을 원래 색상으로 복원
                StartCoroutine(RestoreOriginalColorAfterDelay(1f));
            }
            else
            {
                Debug.LogError("Invalid 16-bit color code."); // 16진수 컬러 코드가 잘못된 경우 오류 메시지 출력
            }
        }
    }
    private IEnumerator RestoreOriginalColorAfterDelay(float delay)
    {
        isChangingColor = true;
        yield return new WaitForSeconds(delay);

        // 원래 색상으로 복원
        button.image.color = originalColor;

        isChangingColor = false;
    }
}
