using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBackChange : MonoBehaviour
{
    public AudioClip[] audios;
    public AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        this.GetComponent<AudioSource>().clip = audios[0];
        this.GetComponent<AudioSource>().Play();
    }

    void Update()
    {
        source.loop = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if(this.GetComponent<AudioSource>().clip == audios[0])
            {
                this.GetComponent<AudioSource>().clip = audios[1];
                this.GetComponent<AudioSource>().Play();
            }
            else
            {
                this.GetComponent<AudioSource>().clip = audios[0];
                this.GetComponent<AudioSource>().Play();
            }
            
        }
    }
}
