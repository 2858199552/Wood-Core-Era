using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    int trapsLayer;
    int arrowLayer;
    int deadLineLayer;
    public string EnemyTag; //敌人标签
    public bool IsDamage; //是否受伤(硬核英语)
    public Animator anim;
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
        arrowLayer = LayerMask.NameToLayer("Arrow");
        deadLineLayer = LayerMask.NameToLayer("DeadLine");
        anim = this.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameManager.instance.Health = 3;
        IsDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer || collision.gameObject.layer == arrowLayer || collision.gameObject.layer == deadLineLayer)
        {
            SoundManager.instance.DeadAudio();
            gameObject.SetActive(false);

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.PlayerDied();
        }
        if(collision.tag == EnemyTag && !IsDamage)
        {
            print("ssssss");
            GameManager.instance.Health--;
            if(GameManager.instance.Health <= 0)
            {
                SoundManager.instance.DeadAudio();
                gameObject.SetActive(false);
                GameManager.PlayerDied();
            }
            else
            {
                IsDamage = true;
                //动画播放处，记得在最后一帧设置设置IsDamage=false;
            }
        }
    }
}
