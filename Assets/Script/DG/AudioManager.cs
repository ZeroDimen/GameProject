using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")] 
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public AudioSource bgmPlayer;
    
    [Header("#SFX")] 
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    private AudioSource[] sfxPlayers;
    public int channelIndex;
    
    public enum Bgm
    {
        CutScene_Old, CutScene_Now, CutScene2, Village, Forest, Dark_Forest, Boss
    }

    public enum Sfx
    {
        attack
    }

    private void Awake()
    {
        instance = this;
        Init();
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }
    
    private void Init()
    {
        // bgm player init
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = true;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip[3];
        bgmPlayer.Play();
        
        // sfx Player init
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClip[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
        
    }
    
    public void ChangeBgm(Bgm bgm)
    {
        if (bgmPlayer.clip != bgmClip[(int)bgm])
        {
            bgmPlayer.clip = bgmClip[(int)bgm];
            bgmPlayer.Play();
        }
    }
    
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void BgmSwittch(Bgm bgm1, Bgm bgm2)
    {
        if (bgmPlayer.clip == bgmClip[(int)bgm1])
        {
            bgmPlayer.clip = bgmClip[(int)bgm2];
        }
        else
        {
            bgmPlayer.clip = bgmClip[(int)bgm1];
        }
        bgmPlayer.Play();
    }
}
