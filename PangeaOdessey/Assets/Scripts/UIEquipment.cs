using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageAndCustomTextChanger : MonoBehaviour
{
    public Image targetImage;     // ����� �̹���
    public Text targetText;       // ǥ���� �ؽ�Ʈ ������Ʈ
    [TextArea]
    public string customText;     // �� ��ư���� ������ �������� ����

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // ���� ��ư�� �̹���(Sprite)�� ��������
        Image btnImage = GetComponent<Image>();
        if (btnImage != null && targetImage != null)
        {
            // Ÿ�� �̹����� Sprite�� ��ư�� Sprite�� ����
            targetImage.sprite = btnImage.sprite;

            // �ؽ�Ʈ�� �������� ������ ����
            if (targetText != null)
            {
                targetText.text = customText;
            }
        }
    }
}