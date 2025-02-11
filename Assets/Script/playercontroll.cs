using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float dashSpeed = 20f; // 冲刺速度
    [SerializeField] float dashDuration = 0.2f; // 冲刺持续时间
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private float dashTimeLeft; // 冲刺剩余时间
    private bool canDash;
    private float isRight;
    private float fixedY;
    private bool isMoving;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        canDash = true;
        anim = GetComponentInChildren<Animator>();
    }
    void Update() {
        // 水平移动
        float moveX = Input.GetAxisRaw("Horizontal"); // 使用 Raw 以获得即时响应
        if (!isDashing) {
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        }

        if (moveX > 0) {
                transform.localScale = new Vector3(1, 1, 1); // 面向右
                isRight = 1;
            } else if (moveX < 0) {
                transform.localScale = new Vector3(-1, 1, 1); // 面向左
                isRight = -1;
            }
    
        // 跳跃
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartDash(isRight);
        }
        // 更新冲刺状态


        if (isDashing) {
            if (dashTimeLeft > 0) {
                dashTimeLeft -= Time.deltaTime;
            } else {
                isDashing = false;
            }
        }
        if(isGrounded && !canDash && !isDashing) canDash = true;

        AnimatorController();

    }

        void StartDash(float direction) {
            isDashing = true;
            canDash = false;
            dashTimeLeft = dashDuration;
            fixedY = rb.position.y; // 记录当前 Y 轴位置
            // 根据方向施加冲刺力
            rb.linearVelocity = new Vector2(direction * dashSpeed, 0);
        }

    void FixedUpdate() {
        if (isDashing) {
            // 固定 Y 轴，确保角色不会掉落或上浮
            rb.position = new Vector2(rb.position.x, fixedY);
        }
    }
    
    void AnimatorController(){
        isMoving = rb.linearVelocity.x != 0;
        anim.SetFloat("yVelocity",rb.linearVelocityY);
        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isGround",isGrounded);
    }

}