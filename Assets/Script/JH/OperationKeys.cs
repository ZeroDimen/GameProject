using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class OperationKeys : MonoBehaviour
{
    public GameObject D;
    public GameObject A;
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
        StartCoroutine(Click());
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.activeSelf)
        {
            rectTransform.position = mainCamera.WorldToScreenPoint(transform.position);
        }
    }
    IEnumerator Click()
    {
        while (true)
        {
            Image image = D.GetComponent<Image>();
            image.color = new Color(0.5f, 0.5f, 0.5f);
            transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 1);
            transform.GetChild(0).GetComponent<Animator>().SetBool("isRun", true);

            yield return new WaitForSeconds(2f);

            image.color = new Color(1, 1, 1);
            transform.GetChild(0).GetComponent<Animator>().SetBool("isRun", false);


            yield return new WaitForSeconds(1f);


            image = A.GetComponent<Image>();
            image.color = new Color(0.5f, 0.5f, 0.5f);
            transform.GetChild(0).GetComponent<Animator>().SetBool("isRun", true);
            transform.GetChild(0).localScale = new Vector3(-0.6f, 0.6f, 1);

            yield return new WaitForSeconds(2f);

            image.color = new Color(1, 1, 1);
            transform.GetChild(0).GetComponent<Animator>().SetBool("isRun", false);

            yield return new WaitForSeconds(1f);
        }

    }
}
