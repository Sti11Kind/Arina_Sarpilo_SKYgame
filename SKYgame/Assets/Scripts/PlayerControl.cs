using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private InputAction move;
    [SerializeField] private float rotSpeed = 30, speed = 20;
    private Rigidbody rb;

    void Awake()
    {
        move = InputSystem.actions.FindAction("Player/Move");
        rb =  GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       rb.AddForce(transform.forward*speed*Time.fixedDeltaTime);
       Vector2 moveInput = move.ReadValue<Vector2>();
       Debug.Log(" x: "+moveInput.x + " y: " + moveInput.y);
       transform.Rotate(0, -moveInput.x  * rotSpeed * Time.fixedDeltaTime, 0);
       float turnAngle = Mathf.Abs(180- transform.localEulerAngles.y);
       float speedMult = Mathf.Cos(turnAngle*Mathf.Deg2Rad);
       Debug.Log("turn Angle: " + turnAngle);
    }
}
