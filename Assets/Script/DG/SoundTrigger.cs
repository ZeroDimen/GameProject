using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioManager.Bgm bgm;
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            // AudioManager.instance.BgmSwittch(bgm1, bgm2);
            AudioManager.instance.ChangeBgm(bgm);
        }
    }    
}
