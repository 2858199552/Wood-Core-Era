using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomArrangement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("RandomNumble")]
    public int dashMin = 1;
    public int dashMax = 3;
    public int attackMin = 1;
    public int attackMax = 3;

    [Header("Monitor")]
    public int dashA;
    public int attackA;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (Input.GetKeyDown(KeyCode.V))
        {
            print("attack");
            AttackRandom();
        }

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
    public void Dash2()
    {

    }
    public void Dash3()
    {

    }

    public void Attack1()
    {

    }
    public void Attack2()
    {

    }
    public void Attack3()
    {

    }
}
