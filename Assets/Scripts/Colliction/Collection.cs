using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    int jarLayer;
    int goldenLeafLayer;
    int silverLeafLayer;
    private Animator anim;
    [Header("收集个数")]
    public int jarNum = 0;
    public int goldenLeafNum = 0;
    public int silverLeafNum = 0;
    [Header("目标数")]
    int jarLimit;
    int goldenLeafLimit;
    int silverLeafLimit;
    void Start()
    {
        jarLayer = LayerMask.NameToLayer("Jar");
        goldenLeafLayer = LayerMask.NameToLayer("GoldenLeaf");
        silverLeafLayer = LayerMask.NameToLayer("SilverLeaf");
        anim = GetComponent<Animator>();
    }

    public void OnEnable()
    {
        jarLimit = 1;
        goldenLeafLimit = 1;
        silverLeafLimit = 2;
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == goldenLeafLayer)
        {
            SoundManager.instance.LeafAudio();
            goldenLeafNum++;
            GameManager.instance.PowerFall();
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.layer == silverLeafLayer)
        {
            SoundManager.instance.LeafAudio();
            silverLeafNum++;
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.layer == jarLayer)
        {
            //anim.SetBool("righting", true);
            SoundManager.instance.JarAudio();
            jarNum++;
            collision.gameObject.SetActive(false);
        }
    }
}
