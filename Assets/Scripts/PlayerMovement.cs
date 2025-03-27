using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

    void Update()
    {
        if(!isAlive) return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value){
        if(!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(!isAlive) return;
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if(value.isPressed){
            rb.linearVelocity += new Vector2(0f, jumpSpeed); 
        }
    }

    void OnAttack(InputValue value){
        if(!isAlive) return;
        Instantiate(bullet, bulletSpawnPoint.position, transform.rotation);
    }
    // Hàm chạy
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
        // Kiểm tra xem người chơi có đang chạm vào thang không
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
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
    // Kiểm tra xem người chơi có chết không
    void Die(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))){
            isAlive = false;
            animator.SetTrigger("Dying");
            rb.linearVelocity = deathKick;
            FindFirstObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
}
