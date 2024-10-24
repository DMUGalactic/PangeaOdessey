using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramDirector : MonoBehaviour
{
    public Transform player; // 플레이어를 참조
    public Transform pentagramPos; // Pentagram의 위치를 참조
    public GameObject pentagram; // Pentagram 오브젝트 참조

    void Start()
    {

    }

    void Update()
    {
        // 플레이어의 위치보다 y축으로 2 더 위에 위치시킴
        transform.position = new Vector3(player.position.x, player.position.y + 2, player.position.z);

        // Pentagram의 방향을 가리키도록 회전
        Vector3 direction = pentagramPos.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Pentagram이 비활성화된 경우
        if (!pentagram.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
