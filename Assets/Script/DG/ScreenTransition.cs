using System;
using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour
{
    public float moveSpeed = 1f; // 이동 속도 조절을 위한 변수
    public GameObject Changer;

    // private void Awake()
    // {
    //     Changer.transform.position = new Vector3(-45f, 0f, 20f);
    // }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(MoveObjectToPosition());
        }
    }

    IEnumerator MoveObjectToPosition()
    {
        Vector3 targetPosition = Vector3.zero; // 목표 위치 (0, 0, 0)
        float elapsedTime = 0f;
        float duration = 1f; // 1초 동안 이동

        while (elapsedTime < duration)
        {
            // Lerp 함수를 사용하여 부드럽게 이동
            Changer.transform.position = new Vector3(1f,0,0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확히 목표 위치에 도달하도록 설정
        Changer.transform.position = targetPosition;
        Debug.Log(targetPosition);
    }
    
}
