using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    //public GameManager gmg;

    [Space]
    [Header("基本设置")]
    public float speed;
    private float tmp;
    public float jumpForce;/*跳跃力*/
    public float climbSpeed;
    public float dashSpeed;//冲刺力
    public float dashTime;
    private float dashTimeLeft;//冲刺剩余时间
    private float lastDash = -10f;//上一次冲刺时间点
    public float dashCoolDown;//冷却时间
    public Transform groundCheck;
    public LayerMask groundLaye;
    public float leftOffset = -0.29f;//左侧偏移


    [Space]
    [Header("监视")]
    public bool isGround;
    public bool isJump;
    public bool isWallLeft;
    public bool isWallRight;
    public bool isClimb;
    public bool Dead;
    public bool isUpClimb;
    public bool isClimbJump;
    public bool isDashing;
    public bool isDash;

    bool jumpPressed;
    int jumpCount;//跳跃次数
    [Space]

    [Header("状态")]
    //public float power = 60f;
    public float hp = 100f;
    public float x;
    public float y;
    //private Vector2 dir;
    //private Vector2 dirRaw;
    public float xRaw;
    public float yRaw;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        //gmg = GetComponent<GameManager>();
        tmp = speed;//保存speed;
        Dead = true;
    }
    void Update()
    {
        if(hp <= 0)
        {
            Dead = false;
        }
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        rb.gravityScale = 2f;

        //更新检查数据
        isUpClimb = false;
        isClimbJump = false;

        if (GameManager.instance.power > 0 && isClimb && y > 0)//向上爬
        {
            GameManager.instance.power -= 0.8f;
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed * y * Time.deltaTime);
            isUpClimb = true;
        }

        ClimbJump();
        //Dash();
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(Time.time >= (lastDash + dashCoolDown))
            {
                //可以执行dash
                ReadyToDash();
            }
        }
    }
    private void FixedUpdate()
    {

        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLaye);//检测是否在地面
        RaycastHit2D leftCheck = Raycast(new Vector2(leftOffset, 0), Vector2.left, 0.2f, groundLaye);
        RaycastHit2D rightCheck = Raycast(new Vector2(-leftOffset, 0), Vector2.right, 0.2f, groundLaye);
        if(leftCheck)
        {
            isWallLeft = true;
        }
        else
        {
            isWallLeft = false;
        }
        if (rightCheck)
        {
            isWallRight = true;
        }
        else
        {
            isWallRight = false;
        }
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        //dir = new Vector2(x, y);
        //dirRaw = new Vector2(xRaw, yRaw);

        GroundMovement();

        Jump();
        Climb();
        Dash();
        if(isDashing)
            return;

        SwitchAnim();
    }

    void GroundMovement()//左右移动
    {
        
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        if(xRaw != 0)
        {
            transform.localScale = new Vector3(xRaw, 1, 1);//翻转，尺寸不变

        }

    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            GameManager.instance.PowerFall();//恢复体力值
            isJump = false;//跳跃动画
            isDash = true;
        }
        if (jumpPressed && isGround)
        {
            SoundManager.instance.JumpAudio();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;//判断update已经检测完jumpPressed，保证手感
        }
        else if (jumpPressed && jumpCount > 0 && isJump)//二段跳
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void ReadyToDash()
    {
        SoundManager.instance.DashAudio();

        isDashing = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;
    }

    void Dash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                if(rb.velocity.y > 0 && !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * x, jumpForce);
                }
                
                rb.velocity = new Vector2(dashSpeed * x, rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFormPool();
            }
            else
            {
                isDashing = false;
                if (!isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * x, jumpForce);
                }
            }
        }
    }

    //void Dash()
    //{
        /*if (Input.GetButtonDown("Dash"))
        {
            //Camera.main.transform.DOComplete();//目前进度，complete完成 
            //Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);//震动.第二个相机无法抖动了
            //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));//RippleEffect涟漪效应
            print("ggggg");
            rb.velocity = dir.normalized * dashSpeed;
            isDashing = true;//动画
            isDash = false;//未完成++++++++++++++++++++++++++++++++++++++++++++
        }*/
    //}

    void Climb()
    {
        isClimb = false;
        rb.gravityScale = 2f;
        if(GameManager.instance.power > 0)
        {
            if((isWallLeft || isWallRight) && Input.GetKey(KeyCode.C))/*(isWallLeft && x < 0) || (isWallRight && x > 0)*/ 
            {
                SoundManager.instance.ClimbAudio();
                isClimb = true;
                GameManager.instance.power -= 0.05f;
                if (rb.velocity.y < 0)
                    rb.gravityScale = 0.1f;
            }
        }
    }

    void ClimbJump()
    {
        if (GameManager.instance.power > 20f && isClimb)//爬的时候跳
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //SoundManager.instance.JumpAudio();
                isClimbJump = true;
                GameManager.instance.PowerChange(-20);
                rb.velocity = new Vector2(10, 10);
            }
        }
    }

    void SwitchAnim()//动作逻辑管理
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
            anim.SetBool("climbing", false);
        }
        else if (!isGround && rb.velocity.y > 0 && isClimb == false) // 
        {
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
            anim.SetBool("climbing", false);
        }
        else if (rb.velocity.y < 0 && isClimb == false)// 
        {
            anim.SetBool("falling", true);
            anim.SetBool("jumping", false);
            anim.SetBool("climbing", false);
        }
        if (isClimb)
        {
            anim.SetBool("climbing", true);
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float len, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, len, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction * len, color);
        return hit;
    }

    private void OnTriggerEnter2D(Collider2D collision)//移动平台带人
    {
        if(collision.tag == "MovePlatform")
        {
            transform.parent = collision.gameObject.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "MovePlatform")
        {
            transform.parent = null;
        }
    }

}
