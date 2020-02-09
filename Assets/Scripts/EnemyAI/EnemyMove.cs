using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class EnemyMove : MonoBehaviour
{
    public string PlayerTag;  //玩家的标签
    public Rigidbody2D rigid;
    public bool IsAttack;//攻击
    public LayerMask MaskLayer; //障碍层
    public LayerMask PlayerLayer; //玩家层
    public float Point1Up; //敌人距头顶点的距离
    public float Point2Down; //敌人距地面点的距离
    public float enemyRadius; //敌人半径,记得略比实际大
    public float Speed = 6; //敌人速度
    public float AttackMoveSpeed = 8; //敌人攻击移动速度
    public float AttackStopDefense = 1; //敌人攻击停止距离,避免穿模
    public Animator anim;
    public int Health = 1; //生命值
    private void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        IsAttack = false;
    }
    private void Update()
    {
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y + Point1Up), this.transform.right * enemyRadius, Color.blue);
        if (!IsAttack)
        {
            if (Physics2D.Raycast(new Vector2(this.transform.position.x,this.transform.position.y+Point1Up),this.transform.right, enemyRadius, MaskLayer) ||
              Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + Point2Down), this.transform.right, enemyRadius, MaskLayer))
            {
                this.transform.Rotate(0, 180, 0);
            }
            rigid.velocity = new Vector2(this.transform.right.x * Speed, rigid.velocity.y);
            if (Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + Point1Up), this.transform.right * this.transform.localScale.x, enemyRadius, PlayerLayer) ||
                Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + Point2Down), this.transform.right * this.transform.localScale.x, enemyRadius, PlayerLayer))
            {
                IsAttack = true;
                StartCoroutine(EnemyAttack());
            }
        }
        if (Health <= 0) Dead();
    }
    /// <summary>
    /// 协程，可以写攻击过程
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyAttack()
    {
        for(int i = 0; i < 120; i++) //时间,每30为1秒,当然下面条件满足会强制退出
        {
            yield return new WaitForFixedUpdate();
            if (Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + Point1Up), this.transform.right, enemyRadius+AttackStopDefense, MaskLayer) ||
               Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + Point2Down), this.transform.right, enemyRadius+AttackStopDefense, MaskLayer))
            {
                this.transform.Rotate(0, 180, 0);
                break;
            }
            rigid.velocity = new Vector2(this.transform.right.x * Speed, rigid.velocity.y);
        }
        IsAttack = false;
    }
    private void Dead()
    {
        StopCoroutine(EnemyAttack());
        this.gameObject.SetActive(false);
    }


}
