using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 네임스페이스 추가

public class MissionManager : MonoBehaviour
{
    private int missionMode;
    private float missionProgress;
    private float missionInit;
    private Text missionProgressText;
    private Vector3 missionInitPos;
    private GameObject monk;
    private bool checkCleared = false;
    public static int killCount = 0;
    public GameObject Mission;
    public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        missionMode = Random.Range(1, 4);
        monk = GameObject.Find("Monk");

        // 현재 오브젝트의 Text 컴포넌트를 찾아서 초기화
        missionProgressText = GetComponent<Text>();

        switch (missionMode)
        {
            case 1: // 골드 획득 미션
                missionInit = GameManager.bitCoin;
                break;

            case 2: // 이동 거리 미션
                missionInitPos = monk.transform.position;
                break;

            case 3: // 몬스터 처치 미션
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (missionMode)
        {
            case 1: // 골드 획득 미션
                missionProgress = GameManager.bitCoin - missionInit;
                missionProgressText.text = "골드 획득: " + Mathf.FloorToInt(missionProgress) + " / 1000";
                missionProgress /= 10;
                break;

            case 2: // 이동 거리 미션
                missionProgress = (missionInitPos - monk.transform.position).magnitude;
                missionProgressText.text = "거리 이동: " + Mathf.FloorToInt(missionProgress) + " / 100";
                break;

            case 3: // 몬스터 처치 미션
                missionProgress = killCount;
                missionProgressText.text = "몬스터 처치: " + missionProgress + " / 100";
                break;
        }
        if (missionProgress >= 100 && !checkCleared)
        {
            checkCleared = true;
            GetComponent<AudioSource>().Play();
            DropItem();
            StartCoroutine(DisableMissionAfterAudio());
        }
    }

    void DropItem()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(itemPrefab, monk.transform.position + Random.insideUnitSphere * 5, Quaternion.identity);
        }
    }

    private IEnumerator DisableMissionAfterAudio()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
            yield return new WaitForSeconds(audioSource.clip.length - 1);
            Debug.Log(audioSource.clip.length);
            Mission.SetActive(false);
    }
}
