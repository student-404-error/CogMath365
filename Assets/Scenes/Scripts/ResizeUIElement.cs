using UnityEngine;
using UnityEngine.UI;

public class ResizeUIElement : MonoBehaviour
{
    public ScrollRect scrollView;
    private RectTransform canvasRectTransform;

    private void Start()
    {
        // 스크롤 뷰의 RectTransform 컴포넌트 가져오기
        RectTransform scrollViewRectTransform = scrollView.GetComponent<RectTransform>();

        // 스크롤 뷰의 부모 캔버스의 RectTransform 컴포넌트 가져오기
        canvasRectTransform = scrollViewRectTransform.parent.GetComponent<RectTransform>();

        // 스크롤 뷰 크기를 캔버스 크기로 조정
        scrollViewRectTransform.sizeDelta = canvasRectTransform.sizeDelta;
    }
}
