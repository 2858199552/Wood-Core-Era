using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    [SerializeField]
    public AudioClip jumpSource, climbSource, dashSource, walkSource, deadSource,jarSource, leafSource;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    public void JumpAudio()
    {
        audioSource.clip = jumpSource;
        audioSource.Play();
    }

    public void ClimbAudio()
    {
        audioSource.clip = climbSource;
        audioSource.Play();
    }

    public void DashAudio()
    {
        audioSource.clip = dashSource;
        audioSource.Play();
    }
    public void WalkAudio()
    {
        audioSource.clip = walkSource;
        audioSource.Play();
    }

    public void DeadAudio()
    {
        audioSource.clip = deadSource;
        audioSource.Play();
    }

    public void JarAudio()
    {
        audioSource.clip = jarSource;
        audioSource.Play();
    }
    public void LeafAudio()
    {
        audioSource.clip = leafSource;
        audioSource.Play();
    }
}
