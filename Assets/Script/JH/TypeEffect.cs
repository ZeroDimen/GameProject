using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeEffect : MonoBehaviour
{
    string targetMsg;
    public int charPerSeconds;
    AudioSource audioSource;
    TMP_Text text;
    float interval;
    int index;
    float delay;
    public bool isActive;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        text = GetComponent<TMP_Text>();
        isActive = false;
        delay = 0f;
    }
    public void SetMsg(string msg) // Talk 스크립트에서 실행
    {
        if (isActive) // 실행중이면 입력중인 대사 다 입력후 타다닥 효과는 중지
        {
            text.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else // 타다닥 진행시켜
        {
            targetMsg = msg;
            EffectStart();
        }
    }
    void EffectStart() // 타다닥 시작
    {
        text.text = "";
        index = 0;
        text.text += targetMsg[index]; // 여기는 굳이 안해줘도 되는데 안하니깐 "" 일때 말풍선이
        index++;                          // 너무 작아져서 한글자라도 남기도록 함

        isActive = true;
        interval = 1.0f / charPerSeconds; // 1초에 몇글
        Invoke("Effecting", interval);
    }
    void Effecting()
    {
        if (text.text.Length == targetMsg.Length) // 모든 글자 다쓰면 리턴
        {
            EffectEnd();
            return;
        }
        if (targetMsg[index] == '\\')
            text.text += '\n';
        else if (targetMsg[index] == '`')
        {
            text.text += ' ';
            delay = 2;
        }
        else
            text.text += targetMsg[index]; // 한글자씩 가져오기

        if (targetMsg[index] != ' ' || targetMsg[index] != '.')
            audioSource.Play();
        index++;

        Invoke("Effecting", interval + delay); // 재귀적으로 계속 돌아가게 함
        delay = 0;
    }
    void EffectEnd()
    {
        isActive = false;
    }
}
