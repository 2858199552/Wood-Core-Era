using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator anim;

    #region 玩家属性数值
    [Header("玩家属性数值")]
    public float hAccel;                            //水平方向行走加速度  
    public float hMaxVel;                           //水平方向行走最大速度
    public float vJmpBeginVel;                      //垂直方向跳跃初始速度
    public float hJmpBeginAccel;                    //跳跃的水平方向加速度
    public float vJmpDurAccel;                      //垂直方向跳跃过程中加速度
    public float jmpTime;                           //跳跃时长
    public Vector2 wallJmpStartVel;                 //蹬墙跳初始速度 (默认为向右(蹬左墙))
    public Vector2 wallJmpDurAccel;                 //蹬墙跳过程中加速度 (默认为向右(蹬左墙))
    public float wallJmpHMoveLockTime;              //蹬墙跳禁止横向移动的时间
    public float hGroundDragAccel;                  //水平方向地面阻力
    public float hAirDragAccel;                     //水平方向空气阻力
    public float dashSpd;                           //冲刺速度
    public float dashEndSpd;                        //冲刺结束后的速度
    public float dashTime;                          //冲刺时间
    public float dashDelayTime;                     //冲刺延迟时间
    public float dashDeadTime;                      //冲刺不响应时间
    public float dashCDTime;                        //冲刺冷却时间
    public float hDashCancelAccel;                  //大跳的横向加速度
    public float hDashDownCancelAccel;              //斜下冲大跳的横向加速度
    public float vDashDownCancelBeginVel;           //斜下冲大跳的纵向初速度
    public float vGravityAccel;                     //垂直方向重力加速度
    public float vNormalMaxFallVel;                 //垂直方向正常最大下落速度
    //public float vFastFallMaxFallVel;               //垂直方向加速下落最大速度(按↓)
    public float vClimbUpVel;                       //垂直方向向上攀爬速度
    public float vClimbDownVel;                     //垂直方向向下攀爬速度
    public float hClimbVel;                         //爬墙时的水平速度(离墙有一段距离时抓到墙会贴上去)
    public float vClimbCornerUpVel;                 //爬到墙角时上到墙上面的速度
    public float hClimbCornerCompleteVel;           //爬到墙角时上到墙上面的速度
    public float climbOppJumpClimbLockTime;         //为了防止单面上墙...
    public float jmpDelayTime;                      //延迟跳跃时间(在落地前按跳跃键的容忍时间)
    public float willyWolfTime;                     //威利狼时间(在离开地面后按跳跃键的容忍时间)
    public Vector2 groundCheckCenter;               //地面检测中心
    public float groundCheckRadius;                 //地面检测半径
    public Vector2 wallCheckCenter;                 //墙壁检测中心
    public float wallCheckRadius;                   //墙壁检测半径
    public Vector2 wallDownCornerCheckCenter;       //下墙角检测中心
    public float wallDownCornerCheckRadius;         //下墙角检测半径
    public Vector2 wallUpCornerCheckCenter;         //上墙角检测中心
    public float wallUpCornerCheckRadius;           //上墙角检测半径
    public LayerMask solidLayer;                    //固体层
    #endregion

    #region 玩家状态数值
    [Header("玩家状态数值")]
    public float jmpTimer = 0f;                 //跳跃计时器
    public int jmpDir = 0;                      //跳跃方向(-1:左方向蹬墙跳(蹬右墙), 0:蹬地跳, 1:右方向蹬墙跳(蹬左墙))
    public Vector2 dashDir = Vector2.zero;
    public float dashTimer = 0f;                //冲刺计时器
    public int maxDashCount = 1;                //冲刺数量上限
    public int dashCounter = 0;                 //冲刺计数器
    public bool dashJumpCancelEnabled = false;  //启用取消冲刺(大跳, 冲墙跳)
    public bool isOnGround = false;             //是否在地面上
    public int dashState = 0;                   //冲刺状态(0:没冲刺, 1:在冲刺的延迟时间, 2:冲刺移动中)
    public int OnWallState = 0;                 //贴墙状态(-1:左墙, 0:没有墙, 1:右墙)
    public float wallJmpHMoveLockTimer = 0f;    //蹬墙跳横向移动锁计时器
    public bool isClimbing = false;             //是否抓墙
    public bool isClimbingUpCorner = false;     //是否在爬上墙角
    public float climbJumpOppLockTimer = 0f;    //防止单面上墙
    public bool isFacingRight = false;          //玩家是否朝右
    public float jmpDelayTimer = 0f;            //延迟跳跃计时器
    public float groundWillyWolfTimer = 0f;     //地面威利狼计时器
    public float wallWillyWolfTimer = 0f;       //墙壁威利狼计时器(正:右墙, 负:左墙, 0:没有墙)
    public float dashOldXVel = 0f;              //冲刺前的横速度
    public float dashOldYVel = 0f;              //冲刺前的纵速度
    public Collider2D attachedCol;              //玩家会继承该碰撞体的速度
    #endregion

    #region 操作输入数值
    private float hAxis = 0;
    private float vAxis = 0;
    private bool jmpButton = false;
    private bool climbButton = false; //"Climb"
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void Update() {

        #region 输入判断
        hAxis = Input.GetAxisRaw("Horizontal");
        if(!isClimbing)
            isFacingRight = hAxis == 0f ? isFacingRight : hAxis == 1f ? true : false;
        vAxis = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump")) {
            jmpDelayTimer = jmpDelayTime;
            jmpButton = true;
        }
        if (Input.GetButtonUp("Jump")) {
            jmpButton = false;
        }
        if (Input.GetButtonDown("Climb")) {
            climbButton = true;
        }
        if (Input.GetButtonUp("Climb")) {
            climbButton = false;
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (dashState == 0 && dashCounter > 0 && dashTimer <= 0f)
            {
                dashTimer = dashDelayTime;
                dashCounter--;
                dashState = 1;
            }
        }
        #endregion

        #region 动画
        anim.SetBool("jumping", jmpTimer > 0 && !isOnGround && !isClimbing);
        anim.SetBool("falling", !(jmpTimer > 0) && !isOnGround && !isClimbing);
        anim.SetFloat("running", Mathf.Abs(hAxis));
        anim.SetBool("climbing", isClimbing);
        #endregion
    }

    void FixedUpdate() {

        float xVel = rb.velocity.x;
        float yVel = rb.velocity.y;

        #region 地面&墙壁检测
        //检测是否贴墙
        attachedCol = Physics2D.OverlapCircle(new Vector2(wallCheckCenter.x + transform.position.x, wallCheckCenter.y + transform.position.y), wallCheckRadius, solidLayer);
        if (!isClimbingUpCorner)
        {
            if (attachedCol != null)
            {
                //设置数值
                OnWallState = 1;
                wallWillyWolfTimer = willyWolfTime;
                //判断是否要抓墙
                if (climbButton && isFacingRight && climbJumpOppLockTimer <= 0f/*&& 判断体力是否大于0*/)
                {
                    isOnGround = false;
                    isClimbing = true;
                    goto AttachDetectionEnd;
                }
                else
                    attachedCol = null;
            }
            else
            {
                attachedCol = Physics2D.OverlapCircle(new Vector2(-wallCheckCenter.x + transform.position.x, wallCheckCenter.y + transform.position.y), wallCheckRadius, solidLayer);
                if (attachedCol != null)
                {
                    //设置数值
                    OnWallState = -1;
                    wallWillyWolfTimer = -willyWolfTime;
                    //判断是否要抓墙
                    if (climbButton && !isFacingRight && climbJumpOppLockTimer <= 0f/*&& 判断体力是否大于0*/)
                    {
                        isOnGround = false;
                        isClimbing = true;
                        goto AttachDetectionEnd;
                    }
                    else
                        attachedCol = null;
                }
                else
                {
                    OnWallState = 0;
                }
            }

            isClimbing = false;
            //检测地面
            Collider2D tmp = Physics2D.OverlapCircle(isFacingRight ? new Vector2(groundCheckCenter.x + transform.position.x, groundCheckCenter.y + transform.position.y) : new Vector2(-groundCheckCenter.x + transform.position.x, groundCheckCenter.y + transform.position.y), groundCheckRadius, solidLayer);
            //设置数值
            if (tmp != null)
            {
                isOnGround = true;
                groundWillyWolfTimer = willyWolfTime;
                attachedCol = tmp;
            }
            else
            {
                isOnGround = false;
            }
        }
        else
        {
            if (attachedCol == null)
            {
                OnWallState = 0;
                attachedCol = Physics2D.OverlapCircle(isFacingRight ? new Vector2(wallDownCornerCheckCenter.x + transform.position.x, wallDownCornerCheckCenter.y + transform.position.y) : new Vector2(-wallDownCornerCheckCenter.x + transform.position.x, wallDownCornerCheckCenter.y + transform.position.y), wallDownCornerCheckRadius, solidLayer);
                if (attachedCol == null)
                {
                    isClimbingUpCorner = false;
                    isClimbing = false;
                    xVel += isFacingRight ? hClimbCornerCompleteVel - hClimbVel : -hClimbCornerCompleteVel + hClimbVel;
                }
            }
        }
    AttachDetectionEnd:
        if (climbJumpOppLockTimer > 0f)
            climbJumpOppLockTimer -= Time.deltaTime;
        #endregion

            #region 冲刺
        if (isOnGround && dashState == 0 && dashTimer <= 0f)
            dashCounter = maxDashCount;
        if (dashState == 0)
        {
            if(dashTimer > 0)
                dashTimer -= Time.fixedDeltaTime;
        }
        else if (dashState == 1)
        {
            if (dashTimer > 0)
            {
                if (xVel != 0f)
                    dashOldXVel = xVel;
                if (yVel != 0f)
                    dashOldYVel = yVel;
                xVel = 0f;
                yVel = 0f;
                dashTimer -= Time.fixedDeltaTime;
            }
            else
            {
                dashTimer = dashDeadTime;
                dashState = 2;
                if (hAxis == 0f)
                {
                    if (vAxis == 0f)
                        dashDir = new Vector2(isFacingRight ? 1f : -1f, 0f);
                    else
                        dashDir = new Vector2(0f, vAxis);
                }
                else
                {
                    if (vAxis == 0f)
                        dashDir = new Vector2(hAxis, 0f);
                    else
                        dashDir = new Vector2(hAxis, vAxis) * 0.707f;
                }
            }
        }
        else if (dashState == 2)
        {
            if (dashTimer > 0)
            {
                if (dashDir.x > 0f)
                {
                    if (xVel < dashDir.x * dashSpd)
                        xVel = dashDir.x * dashSpd;
                }
                else if (dashDir.x < 0f)
                {
                    if (xVel > dashDir.x * dashSpd)
                        xVel = dashDir.x * dashSpd;
                }
                else
                {
                    xVel = 0f;
                }

                if (dashDir.y > 0f)
                {
                    if (yVel < dashDir.y * dashSpd)
                        yVel = dashDir.y * dashSpd;
                }
                else if (dashDir.y < 0f)
                {
                    if (yVel > dashDir.y * dashSpd)
                        yVel = dashDir.y * dashSpd;
                }
                else
                {
                    yVel = 0f;
                }
                dashTimer -= Time.fixedDeltaTime;
            }
            else
            {
                dashState = 0;
                xVel = dashDir.x * dashEndSpd;
                yVel = dashDir.y * dashEndSpd;
                dashOldXVel = 0f;
                dashOldYVel = 0f;
                dashTimer = dashCDTime;
            }
        }
        #endregion

        #region 从接触的碰撞体上得到速度
        float attachedXVel = 0f;
        float attachedYVel = 0f;
        if (dashState == 0)
        {
            Rigidbody2D attachedRb = null;
            try { attachedRb = attachedCol.GetComponent<Rigidbody2D>(); }
            catch (System.NullReferenceException) { }
            if (attachedRb != null)
            {
                attachedXVel = attachedRb.velocity.x;
                attachedYVel = attachedRb.velocity.y;
            }
        }
        #endregion

        #region 摩擦
        //抓墙和冲刺的时候不添加阻力
        if (!isClimbing && dashState == 0)
        {
            float drag = isOnGround ? hGroundDragAccel : hAirDragAccel;
            if (xVel > drag * Time.fixedDeltaTime + attachedXVel)
            {
                xVel -= drag * Time.fixedDeltaTime;
            }
            else if (xVel < -drag * Time.fixedDeltaTime + attachedXVel)
            {
                xVel += drag * Time.fixedDeltaTime;
            }
            else
            {
                xVel = attachedXVel;
            }

            #endregion

        #region 行走
            //抓墙和冲刺的时候也不能左右移动, 为了防止单面上墙在蹬墙跳时不能朝墙方向移动
                if (hAxis == 1f)
                {
                    if (wallJmpHMoveLockTimer > 0 && jmpDir == -1)                 
                        goto ApplyHMovementEnd;
                    if (xVel < hMaxVel - hAccel * Time.fixedDeltaTime + attachedXVel)
                    {
                        xVel += hAccel * Time.fixedDeltaTime;
                    }
                    else if (xVel < hMaxVel + attachedXVel)
                    {
                        xVel = hMaxVel + attachedXVel;
                    }
                }
                else if (hAxis == -1f)
                {
                    if (wallJmpHMoveLockTimer > 0 && jmpDir == 1)
                        goto ApplyHMovementEnd;
                    if (xVel > hAccel * Time.fixedDeltaTime - hMaxVel + attachedXVel)
                    {
                        xVel -= hAccel * Time.fixedDeltaTime;
                    }
                    else if (xVel > -hMaxVel + attachedXVel)
                    {
                        xVel = -hMaxVel + attachedXVel;
                    }
                }
        }
    ApplyHMovementEnd:
        if (wallJmpHMoveLockTimer > 0)
            wallJmpHMoveLockTimer -= Time.fixedDeltaTime;
        transform.localScale = isFacingRight ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        #endregion

        #region 跳跃
        if (jmpDelayTimer > 0f && jmpTimer <= 0f)
        {
            //判断是否可以跳跃并施加初始速度
            if (dashState == 1)
            {
                jmpDelayTimer -= Time.fixedDeltaTime;
            }
            else if (dashState == 2)
            {
                if (dashJumpCancelEnabled)
                {
                    if (groundWillyWolfTimer > 0f)
                    {
                        dashState = 0;
                        dashTimer = dashCDTime;
                        jmpTimer = jmpTime;
                        jmpDelayTimer = 0f;
                        groundWillyWolfTimer = 0f;
                        if (dashDir.x != 0f)
                        {
                            if (dashDir.y < 0f)
                            {
                                xVel += dashDir.x > 0f ? hDashDownCancelAccel - dashSpd : -hDashDownCancelAccel + dashSpd;
                                yVel = vDashDownCancelBeginVel;
                            }
                            else
                            {
                                xVel += (hDashCancelAccel - dashSpd) * dashDir.x;
                                yVel = vJmpBeginVel;
                            }
                        }
                        else
                        {
                            
                        }
                        jmpDir = 0;
                    }

                }
                else
                    jmpDelayTimer -= Time.fixedDeltaTime;
            }
            else if (isClimbing)
            {
                jmpTimer = jmpTime;
                jmpDelayTimer = 0f;
                groundWillyWolfTimer = 0f;
                yVel = vJmpBeginVel;
                if (hAxis != (isFacingRight ? -1f : 1f))
                {
                    jmpDir = 0;
                    //此处消耗在爬墙时蹬墙跳的体力
                }
                else
                {
                    jmpDir = isFacingRight ? -1 : 1;
                    isClimbing = false;
                    climbJumpOppLockTimer = climbOppJumpClimbLockTime;
                    xVel = isFacingRight ? -wallJmpStartVel.x : wallJmpStartVel.x;
                    wallJmpHMoveLockTimer = wallJmpHMoveLockTime;
                    wallWillyWolfTimer = 0f;
                }
            }
            else if (groundWillyWolfTimer > 0f)
            {
                jmpTimer = jmpTime;
                jmpDelayTimer = 0f;
                groundWillyWolfTimer = 0f;
                xVel += hJmpBeginAccel * hAxis;
                yVel = vJmpBeginVel;
                jmpDir = 0;
            }
            else if (wallWillyWolfTimer > 0f)
            {
                jmpTimer = jmpTime;
                jmpDelayTimer = 0f;
                wallWillyWolfTimer = 0f;
                xVel = -wallJmpStartVel.x;
                yVel = wallJmpStartVel.y;
                jmpDir = -1;
                wallJmpHMoveLockTimer = wallJmpHMoveLockTime;
            }
            else if (wallWillyWolfTimer < 0f)
            {
                jmpTimer = jmpTime;
                jmpDelayTimer = 0f;
                wallWillyWolfTimer = 0f;
                xVel = wallJmpStartVel.x;
                yVel = wallJmpStartVel.y;
                jmpDir = 1;
                wallJmpHMoveLockTimer = wallJmpHMoveLockTime;
            }
            else
            {
                jmpDelayTimer -= Time.fixedDeltaTime;
            }
        }
        //威利狼计时器随时间递减
        if (!isOnGround && groundWillyWolfTimer > 0f)
        {
            groundWillyWolfTimer -= Time.fixedDeltaTime;
        }

        if (OnWallState == 0)
            if (wallWillyWolfTimer > Time.fixedDeltaTime)
            {
                wallWillyWolfTimer -= Time.deltaTime;
            }
            else if (wallWillyWolfTimer < -Time.fixedDeltaTime)
            {
                wallWillyWolfTimer += Time.deltaTime;
            }
            else
            {
                wallWillyWolfTimer = 0f;
            }
        //施加跳跃的力        
        if (jmpTimer > 0)
        {
            if (!jmpButton)
            {
                jmpTimer = 0f;
                goto JumpingEnd;
            }
            groundWillyWolfTimer = 0f;
            if (jmpDir == -1f)
            {
                xVel -= (jmpTimer / jmpTime) * wallJmpDurAccel.x * Time.fixedDeltaTime;
                yVel += (jmpTimer / jmpTime) * wallJmpDurAccel.y * Time.fixedDeltaTime;
            }
            else if (jmpDir == 1f)
            {
                xVel += (jmpTimer / jmpTime) * wallJmpDurAccel.x * Time.fixedDeltaTime;
                yVel += (jmpTimer / jmpTime) * wallJmpDurAccel.y * Time.fixedDeltaTime;
            }
            else
            {
                yVel += (jmpTimer / jmpTime) * vJmpDurAccel * Time.fixedDeltaTime;
                /*if(isClimbing)
                    * {(此处消耗在爬墙跳跃中的一固定帧(Time.fixedDeltaTime)的体力)}*/
            }
            jmpTimer -= Time.fixedDeltaTime;
        }
        
    JumpingEnd:
        #endregion

        #region 攀爬
        if (isClimbing)
        {            
            bool cornerReached = Physics2D.OverlapCircle(isFacingRight ? new Vector2(wallUpCornerCheckCenter.x + transform.position.x, wallUpCornerCheckCenter.y + transform.position.y) : new Vector2(-wallUpCornerCheckCenter.x + transform.position.x, wallUpCornerCheckCenter.y + transform.position.y), wallUpCornerCheckRadius, solidLayer) == null;
            if (!isClimbingUpCorner && vAxis == 1f && cornerReached)
                isClimbingUpCorner = true;
            if (isClimbingUpCorner) 
            {
                yVel += vGravityAccel * Time.fixedDeltaTime;
                if (yVel < vClimbCornerUpVel)
                    yVel = vClimbCornerUpVel + attachedYVel;
                //爬上墙角...不应该消耗体力
            }
            else if (vAxis == -1f)
            {
                yVel += vGravityAccel * Time.fixedDeltaTime;
                if (yVel < vClimbDownVel + attachedYVel)
                    yVel = vClimbDownVel + attachedYVel;
                //此处消耗往下爬的以固定帧(Time.fixedDeltaTime)的体力
            }
            else if (vAxis == 1f && !cornerReached)
            {
                yVel += vGravityAccel * Time.fixedDeltaTime;
                if (yVel < vClimbUpVel + attachedYVel)
                    yVel = vClimbUpVel + attachedYVel;
                //此处消耗往上爬的以固定帧(Time.fixedDeltaTime)的体力
            }
            else
            {
                yVel += vGravityAccel * Time.fixedDeltaTime;
                if (yVel < attachedYVel)
                    yVel = attachedYVel;
                //此处消耗抓住墙不动的一固定帧(Time.fixedDeltaTime)的体力
            }
            xVel = (isFacingRight ? hClimbVel : -hClimbVel) + attachedXVel;
        }

        #endregion

        #region 施加重力
        if (!isClimbing && dashState == 0)
        {
            if (yVel > vNormalMaxFallVel - vGravityAccel * Time.fixedDeltaTime)
                yVel += vGravityAccel * Time.fixedDeltaTime;
            else
                yVel = vNormalMaxFallVel;
        }
        #endregion

        rb.velocity = new Vector2(xVel, yVel);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(isFacingRight ? new Vector2(wallCheckCenter.x + transform.position.x, wallCheckCenter.y + transform.position.y) : new Vector2(-wallCheckCenter.x + transform.position.x, wallCheckCenter.y + transform.position.y), wallCheckRadius);
        Gizmos.DrawWireSphere(isFacingRight ? new Vector2(wallDownCornerCheckCenter.x + transform.position.x, wallDownCornerCheckCenter.y + transform.position.y) : new Vector2(-wallDownCornerCheckCenter.x + transform.position.x, wallDownCornerCheckCenter.y + transform.position.y), wallDownCornerCheckRadius);
        Gizmos.DrawWireSphere(isFacingRight ? new Vector2(groundCheckCenter.x + transform.position.x, groundCheckCenter.y + transform.position.y) : new Vector2(-groundCheckCenter.x + transform.position.x, groundCheckCenter.y + transform.position.y), groundCheckRadius);
        Gizmos.DrawWireSphere(isFacingRight ? new Vector2(wallUpCornerCheckCenter.x + transform.position.x, wallUpCornerCheckCenter.y + transform.position.y) : new Vector2(-wallUpCornerCheckCenter.x + transform.position.x, wallUpCornerCheckCenter.y + transform.position.y), wallDownCornerCheckRadius);
    }

}
