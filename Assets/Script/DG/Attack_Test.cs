using UnityEngine;

public class Attack_Test : MonoBehaviour
{
    private Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    
    public void Ani_Off()
    {
        gameObject.SetActive(false);
    }

    public void Attack_Ani()
    {
        ani.Play("Attack_Sample");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.attack);
    }

    public void Attack_Effact_Speed_Change(float Speed)
    {
        ani.SetFloat("AttackEffactSpeed", Speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }
}
