using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Talk : MonoBehaviour
{
    public TypeEffect talk;
    public GameObject tem_TextMeshProObj;
    public GameObject talkBubble;
    public TMP_Text tem_Text;
    public TMP_Text text;
    public Transform[] colleague;
    RectTransform rectTransform;
    Camera mainCamera;
    public static int talkIndex;
    int cutSceneIndex;
    bool flag;
    private void Awake()
    {
        talkIndex = 0;
        cutSceneIndex = 1;
        flag = false;
    }
    private void Start()
    {
        rectTransform = talkBubble.GetComponent<RectTransform>();
        CinemachineBrain cinemachineBrain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        mainCamera = cinemachineBrain.OutputCamera;
    }
    void Update()
    {
        if (GameObject.Find("colleague") != null)
            Colleague();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < 4; i++)
                colleague[i].gameObject.SetActive(false);
            for (int i = 4; i < 6; i++)
                colleague[i].gameObject.SetActive(true);
            flag = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!talkBubble.activeSelf && !flag)
            {
                talkBubble.SetActive(true);
                tem_TextMeshProObj.SetActive(true);
            }

            if (tem_TextMeshProObj.activeSelf)
            {
                if (!talk.isActive)
                {
                    talkIndex++;
                    switch (cutSceneIndex)
                    {
                        case 1:
                            tem_TextMeshProObj.GetComponent<Tem_TextMeshPro>().localize(Tem_TextMeshPro.CutScene.CutScene_1, talkIndex);
                            break;
                        case 2:
                            tem_TextMeshProObj.GetComponent<Tem_TextMeshPro>().localize(Tem_TextMeshPro.CutScene.CutScene_2, talkIndex);
                            break;
                    }


                    if (tem_TextMeshProObj.activeSelf)
                    {
                        switch (cutSceneIndex)
                        {
                            case 1:
                                if (talkIndex == 1 || talkIndex == 2 || talkIndex == 3)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[0].transform.position + Vector3.up * 2);
                                else if (talkIndex == 4 || talkIndex == 6 || talkIndex == 7 || talkIndex == 9 || talkIndex == 10)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[1].transform.position + Vector3.up * 2);
                                else if (talkIndex == 5)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[2].transform.position + Vector3.up * 2);
                                else if (talkIndex == 8)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[3].transform.position + Vector3.up * 2);
                                break;
                            case 2:
                                if (talkIndex == 1 || talkIndex == 3 || talkIndex == 5 || talkIndex == 8 || talkIndex == 9 || talkIndex == 10
                                || talkIndex == 12 || talkIndex == 15 || talkIndex == 17 || talkIndex == 19 || talkIndex == 21)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[4].transform.position + Vector3.up * 2);
                                else if (talkIndex == 2 || talkIndex == 6 || talkIndex == 7 || talkIndex == 11 || talkIndex == 13 || talkIndex == 18)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[5].transform.position + Vector3.up * 2);
                                else if (talkIndex == 4 || talkIndex == 14 || talkIndex == 16 || talkIndex == 20)
                                    rectTransform.position = mainCamera.WorldToScreenPoint(colleague[4].transform.position + Vector3.up * 2 + Vector3.right);
                                break;
                        }

                        talk.SetMsg(tem_Text.text);
                    }
                    else
                    {
                        talkBubble.SetActive(false);
                        flag = true;
                        talkIndex = 0;
                        cutSceneIndex++;
                    }
                }
                else
                    talk.SetMsg("");
            }
        }
    }
    void Colleague()
    {
        colleague = new Transform[6];

        for (int i = 0; i < 6; i++)
            colleague[i] = GameObject.Find("colleague").transform.GetChild(i);
    }
}
