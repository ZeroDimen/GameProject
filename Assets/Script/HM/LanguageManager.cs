using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public enum GameLanguage {ENGLISH,KOREAN}
public class LanguageManager : MonoBehaviour
{
    GameLanguage gameLanguage; // 이 변수언어를 확인해서 게임컨텐츠를 해당 언어로 변역합니다.
    
    // 출시 될 3개의 언어만 확인합니다.
    void Awake()
    {
        if(Application.systemLanguage == SystemLanguage.English)
        {
            gameLanguage = GameLanguage.ENGLISH;
        }
        else if(Application.systemLanguage == SystemLanguage.Korean)
        {
            gameLanguage = GameLanguage.KOREAN;
        }
        else
        {
            gameLanguage = GameLanguage.ENGLISH;
        }

        Debug.Log(gameLanguage);
        ChangeLocale((int)gameLanguage);
    }

    bool isChanging = false;

    public void ChangeLocale(int index)
    {
        if (isChanging)
            return;

        StartCoroutine(LocaleChange(index));
    }

    IEnumerator LocaleChange(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
    }
}
