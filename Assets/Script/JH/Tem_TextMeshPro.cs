using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Tem_TextMeshPro : MonoBehaviour
{
    public enum CutScene
    {
        CutScene_1,
        CutScene_2,
    }
    public LocalizeStringEvent b1;
    private void Start()
    {
        b1.SetEntry($"CutScene_1-1");
    }
    public void localize(CutScene cutScene, int talkIndex)
    {
        if (cutScene == CutScene.CutScene_1)
        {
            if (talkIndex <= 10)
                b1.SetEntry($"CutScene_1-{talkIndex}");
            else
            {
                b1.SetTable("CutScene_2");
                b1.SetEntry($"CutScene_2-1");
                Talk.talkIndex = 0;
                gameObject.SetActive(false);
            }
        }
        else if (cutScene == CutScene.CutScene_2)
        {
            if (talkIndex <= 21)
                b1.SetEntry($"CutScene_2-{talkIndex}");
            else
                gameObject.SetActive(false);
        }
    }
    void tem()
    {

    }
}
