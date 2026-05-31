using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private InputAction move;
    [SerializeField] private float rotSpeed = 30, speed = 20;
    private Rigidbody rb;

    [SerializeField] private bool grounded = true;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector3 pushbackForce;
    [SerializeField] private bool disabledControl = false;
    [SerializeField] private float disabledTime = 1;
    private float lastCollisionTime;
    public static Transform player;
    private Animator animator;

    void Awake()
    {
        move = InputSystem.actions.FindAction("Player/Move");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = transform;
    }

    private void OnEnable()
    {
        Obstacles.OnPlayerHit += TakeDamage;
    }

    private void OnDisable()
    {
        Obstacles.OnPlayerHit -= TakeDamage;
    }

    void TakeDamage()
    {
        rb.AddForce(pushbackForce);
        disabledControl = true;
        lastCollisionTime = Time.timeSinceLevelLoad;
        Debug.Log("PLAYER GOT HURT");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.timeSinceLevelLoad > lastCollisionTime + disabledTime)
            disabledControl = false;
        grounded = Physics.Linecast(transform.position, 
            transform.position + Vector3.down, groundMask);

        Color lineCol = grounded ? Color.green : Color.red;

            Debug.DrawLine(transform.position,
                transform.position + Vector3.down, lineCol);
        if (grounded && !disabledControl)
        {
            Vector3 newVelocity = transform.forward * speed;
            newVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = newVelocity;

            Vector2 moveInput = move.ReadValue<Vector2>();
            Debug.Log("x: " + moveInput.x + " y: " + moveInput.y);
            transform.Rotate(0, -moveInput.x * rotSpeed * Time.fixedDeltaTime, 0);
            float turnAngle = Mathf.Abs(180 - transform.localEulerAngles.y);
            float speedMult = Mathf.Cos(turnAngle * Mathf.Deg2Rad);
            Debug.Log("turn angle: " + turnAngle);
        }

        animator.SetFloat("playerSpeed", rb.linearVelocity.magnitude);
        animator.SetBool("grounded", grounded);
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public bool IsMoving()
    {
        return rb.linearVelocity.magnitude > 0.5f;
    }
}
