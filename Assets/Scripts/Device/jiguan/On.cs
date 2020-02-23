using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On : MonoBehaviour
{
    public Animator anim;
    public AudioSource source;
    public GameObject movePlatform;
    int playerLayer;
    bool isOn;

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            //show
            if(!isOn)
            {
                source.Play();
                anim.SetBool("on", true);
                movePlatform.SetActive(true);
            }
            isOn = true;
        }
    }
}
