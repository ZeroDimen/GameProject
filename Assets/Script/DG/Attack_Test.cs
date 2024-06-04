using System;
using UnityEngine;

public class Attack_Test : MonoBehaviour
{
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //ani.SetTrigger("IsAttack");
            ani.Play("Attack_Sample");
        }
    }

    public void Ani_Off()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }
}
