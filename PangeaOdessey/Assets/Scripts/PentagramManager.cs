using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramManager : MonoBehaviour
{
    private GameObject monk;
    private SpriteRenderer spriteRenderer;
    private Color targetColor; // 투명도를 포함한 색상 저장
    public GameObject Pentagram;
    private AudioSource audioSource;
    public AudioClip audioClipMissionStart;
    
    public GameObject Mission;

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        this.monk = GameObject.Find("Monk");
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        targetColor = spriteRenderer.color; // 초기 색상 저장
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClipMissionStart;
    }

    // 매 프레임마다 호출됩니다.
    void Update()
    {
        
    }

    // 다른 Collider가 이 오브젝트의 트리거 Collider에 들어오면 호출됩니다.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetColor.a < 1)
            {
                targetColor.a += 0.01f;
                spriteRenderer.color = targetColor;
            }
            else
            {
                if (!audioSource.isPlaying) // 오디오가 재생 중이지 않을 때만 실행
                {
                    Mission.SetActive(true);
                    audioSource.Play();
                    StartCoroutine(DisablePentagramAfterAudio()); // 오디오 재생 후 비활성화
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetColor.a < 1)
        {
            targetColor.a = 0f;
            spriteRenderer.color = targetColor;
        }
    }

    private IEnumerator DisablePentagramAfterAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length); // 오디오 클립의 길이만큼 대기
        Pentagram.SetActive(false); // 오디오 재생이 끝난 후 비활성화
    }
}
