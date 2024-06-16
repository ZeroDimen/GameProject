using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CutSceneBattle_Jump : MonoBehaviour
{
    public GameObject space;
    public GameObject panel;
    RectTransform rectTransform;
    Camera mainCamera;
    Rigidbody2D rigid;
    Animator anime;
    RaycastHit2D hit;
    void Start()
    {
        anime = transform.GetChild(0).GetComponent<Animator>();
        rigid = transform.GetChild(0).GetComponent<Rigidbody2D>();
        rectTransform = panel.GetComponent<RectTransform>();
        CinemachineBrain cinemachineBrain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        mainCamera = cinemachineBrain.OutputCamera;

        panel.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(Jump());
    }

    void Update()
    {
        hit = Physics2D.Raycast(transform.GetChild(0).position, Vector3.down, 0.1f, 1 << LayerMask.NameToLayer("Platform"));
        Debug.DrawRay(transform.GetChild(0).position, Vector3.down * 0.1f);
        if (panel.activeSelf)
        {
            rectTransform.position = mainCamera.WorldToScreenPoint(transform.position);
        }

        if (rigid.velocity.y > 0)
            anime.SetBool("isUp", true);
        else if (rigid.velocity.y < 0)
        {
            anime.SetBool("isUp", false);
            anime.SetBool("isDown", true);
        }
        if (hit.collider != null)
        {
            anime.SetBool("isUp", false);
            anime.SetBool("isDown", false);
        }

    }
    IEnumerator Jump()
    {
        while (true)
        {
            while (true)
            {
                Image image = space.GetComponent<Image>();
                rigid.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
                image.color = new Color(0.5f, 0.5f, 0.5f);
                yield return new WaitForSeconds(0.5f);
                image.color = new Color(1, 1, 1);
                yield return new WaitForSeconds(3f);
            }

        }
    }
}
