using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    float gravityScaleAtStart;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if(value.isPressed){
            rb.linearVelocity += new Vector2(0f, jumpSpeed); 
        }
    }

    void Run(){
        Vector2 playerVelocity = new(moveInput.x * runSpeed, rb.linearVelocityY);
        rb.linearVelocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.linearVelocityX) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite(){
        // Kiểm tra xem người chơi có đang chạy không
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.linearVelocityX) > Mathf.Epsilon;
        // Nếu có thì đảo hình ảnh của người chơi
        if(playerHasHorizontalSpeed) transform.localScale = new Vector2(Mathf.Sign(rb.linearVelocityX), 1f);
    }

    void ClimbLadder(){
        if(!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            rb.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new(rb.linearVelocityX, moveInput.y * climbSpeed);
        rb.linearVelocity = climbVelocity;
        rb.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rb.linearVelocityY) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
