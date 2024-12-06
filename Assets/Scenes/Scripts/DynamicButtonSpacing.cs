using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonSpacing : MonoBehaviour
{
    public GameObject buttonPrefab;   // 버튼 프리팹
    public RectTransform content;     // 스크롤 가능한 내용물
    public int numberOfButtons = 14;  // 전체 버튼 개수
    public int buttonsPerRow = 3;     // 한 줄에 배치될 버튼 개수
    public float spacingRatio = 0.2f; // 버튼 간격을 화면 가로 길이에 대한 비율로 설정

    private void Start()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        float screenWidth = Screen.width;

        // 가로 길이와 비율로 버튼 간격 계산
        float buttonSpacing = screenWidth * spacingRatio;

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, content);
            RectTransform buttonRectTransform = buttonObj.GetComponent<RectTransform>();

            // 버튼 크기 및 위치 설정
            int rowIndex = i / buttonsPerRow;  // 현재 줄 인덱스
            int colIndex = i % buttonsPerRow;  // 현재 열 인덱스
            float xPos = colIndex % 2 == 0 ? buttonRectTransform.sizeDelta.x * 0.7f : -buttonRectTransform.sizeDelta.x * 0.7f;
            float yPos = Screen.height * 0.5f - rowIndex * (buttonRectTransform.sizeDelta.y + buttonSpacing);
            buttonRectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }

        // Content의 크기 설정
        int rowCount = Mathf.CeilToInt((float)numberOfButtons / buttonsPerRow);
        float contentHeight = rowCount * (buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + buttonSpacing);
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
    }
}
