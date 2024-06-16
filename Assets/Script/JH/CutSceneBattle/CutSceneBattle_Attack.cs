using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneBattle_Attack : MonoBehaviour
{
    public GameObject mouse;
    public GameObject panel;
    RectTransform rectTransform;
    Camera mainCamera;
    void Start()
    {
        rectTransform = panel.GetComponent<RectTransform>();
        CinemachineBrain cinemachineBrain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        mainCamera = cinemachineBrain.OutputCamera;

        panel.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        Animator effectAnime = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        if (panel.activeSelf)
        {
            rectTransform.position = mainCamera.WorldToScreenPoint(transform.position);
        }

        if (transform.GetChild(0).GetChild(0).gameObject.activeSelf && effectAnime.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

    }
    IEnumerator Attack()
    {
        Animator playerAnime = transform.GetChild(0).GetComponent<Animator>();
        Animator effectAnime = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        Animator damageAnime = transform.GetChild(1).GetComponent<Animator>();
        while (true)
        {
            playerAnime.Play("Attack");
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            effectAnime.Play("Effect");
            yield return new WaitForSeconds(0.15f);
            mouse.GetComponent<Image>().color = Color.black;
            damageAnime.Play("Damage");
            yield return new WaitForSeconds(0.25f);
            mouse.GetComponent<Image>().color = Color.white;

            yield return new WaitForSeconds(1.6f);
        }
    }
}
