using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public LocalizeStringEvent b1;
    private void Update()
    {
    }

    private void Start()
    {
        //b1.SetEntry("UI_Continue");

        b1.SetEntry("CutScene_1-2");
        b1.SetEntry("CutScene_1-3");
        b1.SetEntry("CutScene_1-4");
    }
}
