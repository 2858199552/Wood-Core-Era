using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomArrangement : MonoBehaviour
{
    /*private Rigidbody2D rb;
    private Animator anim;
    private Movement movement;
    [Header("RandomNumble")]
    public int dashMin = 1;
    public int dashMax = 3;
    public int attackMin = 1;
    public int attackMax = 3;

    [Header("Monitor")]
    public int dashA;
    public int attackA;
    private bool isDash1;
    private bool isDash2;
    private bool isDash3;
    private bool isAttack1;
    private bool isAttack2;
    private bool isAttack3;

    void Start()
    {
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dashA = Random.Range(dashMin, dashMax);
        attackA = Random.Range(attackMin, attackMax);
    }

    void Update()
    {

        //检测关卡数，及玩家特殊效果从而改变random大小
        if(Input.GetKeyDown(KeyCode.X))
        {
            print("dash");
            DashRandom();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("attack");
            AttackRandom();
        }
        SwitchAnim();
    }

    public void DashRandom()
    {
        switch (dashA)
        {
            case 1:
                {
                    Dash1();
                    break;
                }
            case 2:
                {
                    Dash2();
                    break;
                }
            case 3:
                {
                    Dash3();
                    break;
                }
        }
        dashA = Random.Range(dashMin, dashMax);
    }
    public void AttackRandom()
    {
        switch (attackA)
        {
            case 1:
                {
                    Attack1();
                    break;
                }
            case 2:
                {
                    Attack2();
                    break;
                }
            case 3:
                {
                    Attack3();
                    break;
                }
        }
        attackA = Random.Range(attackMin, attackMax);
    }

    public void Dash1()
    {

    }
    //此方法类似于闪现
    public void Dash2()
    {
        Invoke("Dash2Process", 1f);
    }
    //此方法附带伤害
    public void Dash3()
    {

    }

    public void Attack1()
    {

    }
    public void Attack2()
    {

    }
    //这其实是一个防御招式，方向格挡，按键操作S+方向键，对应相应动作
    public void Attack3()
    {
        rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y / 2);
        GameManager.instance.DEF *= 2;
        isAttack3 = true;
    }

    void SwitchAnim()
    {
        if(isDash1)
        {
            anim.SetBool("dash1ing", true);
            anim.SetBool("dash2ing", false);
            anim.SetBool("dash3ing", false);
        }
        else if (isDash2)
        {
            anim.SetBool("dash2ing", true);
            anim.SetBool("dash1ing", false);
            anim.SetBool("dash3ing", false);
        }
        else if (isDash3)
        {
            anim.SetBool("dash3ing", true);
            anim.SetBool("dash1ing", false);
            anim.SetBool("dash2ing", false);
        }
        else
        {
            anim.SetBool("dash1ing", true);
            anim.SetBool("dash2ing", false);
            anim.SetBool("dash3ing", false);
        }

        if (isAttack1)
        {
            anim.SetBool("attack1ing", true);
            anim.SetBool("attack2ing", false);
            anim.SetBool("attack3ing", false);
        }
        else if (isAttack2)
        {
            anim.SetBool("attack2ing", true);
            anim.SetBool("attack1ing", false);
            anim.SetBool("attack3ing", false);
        }
        else if (isAttack3)
        {
            anim.SetBool("attack3ing", true);
            anim.SetBool("attack1ing", false);
            anim.SetBool("attack2ing", false);
        }
        else
        {
            anim.SetBool("attack1ing", true);
            anim.SetBool("attack2ing", false);
            anim.SetBool("attack3ing", false);
        }
    }
    public void Dash2Process()
    {
        transform.position = new Vector3(Vector2().
    }*/
}
