using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneBattle_Monster_Dead : MonoBehaviour
{
    Animator anime;
    bool flag = false;
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Death") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon") && !flag)
        {
            anime.SetBool("Flag", true);
            flag = true;
        }
    }
}
