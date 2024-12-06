using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonSpacing : MonoBehaviour
{
    public GameObject buttonPrefab;   // ��ư ������
    public RectTransform content;     // ��ũ�� ������ ���빰
    public int numberOfButtons = 14;  // ��ü ��ư ����
    public int buttonsPerRow = 3;     // �� �ٿ� ��ġ�� ��ư ����
    public float spacingRatio = 0.2f; // ��ư ������ ȭ�� ���� ���̿� ���� ������ ����

    private void Start()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        float screenWidth = Screen.width;

        // ���� ���̿� ������ ��ư ���� ���
        float buttonSpacing = screenWidth * spacingRatio;

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, content);
            RectTransform buttonRectTransform = buttonObj.GetComponent<RectTransform>();

            // ��ư ũ�� �� ��ġ ����
            int rowIndex = i / buttonsPerRow;  // ���� �� �ε���
            int colIndex = i % buttonsPerRow;  // ���� �� �ε���
            float xPos = colIndex % 2 == 0 ? buttonRectTransform.sizeDelta.x * 0.7f : -buttonRectTransform.sizeDelta.x * 0.7f;
            float yPos = Screen.height * 0.5f - rowIndex * (buttonRectTransform.sizeDelta.y + buttonSpacing);
            buttonRectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }

        // Content�� ũ�� ����
        int rowCount = Mathf.CeilToInt((float)numberOfButtons / buttonsPerRow);
        float contentHeight = rowCount * (buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + buttonSpacing);
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
    }
}
