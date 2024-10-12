using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageAndCustomTextChanger : MonoBehaviour
{
    public Image targetImage;     // 변경될 이미지
    public Text targetText;       // 표시할 텍스트 컴포넌트
    [TextArea]
    public string customText;     // 각 버튼마다 지정할 개별적인 문구

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // 현재 버튼의 이미지(Sprite)를 가져오기
        Image btnImage = GetComponent<Image>();
        if (btnImage != null && targetImage != null)
        {
            // 타겟 이미지의 Sprite를 버튼의 Sprite로 변경
            targetImage.sprite = btnImage.sprite;

            // 텍스트를 개별적인 문구로 변경
            if (targetText != null)
            {
                targetText.text = customText;
            }
        }
    }
}