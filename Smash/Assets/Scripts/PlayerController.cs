using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector3 playerVelocity;

    private float playerSpeed = 30.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -30f;

    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    private Rigidbody2D _rigidBody2D;
    private Transform _transform;
    public Transform _raycastOrigin;
    private Vector2 gravity = new Vector2(0f, -30f);
    public float raycastDist = 0.005f;
    private bool grounded = false;
    private float jumpCooldown = 0.5f;
    private float jumpTimer;

    private void Awake()
    {
        _transform = transform;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        jumpTimer = Time.time;
        jumpCooldown = 1f;
    }

    void FixedUpdate()
    {
        grounded = IsGrounded();
        _rigidBody2D.AddForce(gravity);
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, 0);
        Debug.Log(movementInput);

        if (move != Vector3.zero)
        {
            _rigidBody2D.AddForce(move * playerSpeed * Time.deltaTime,ForceMode2D.Impulse);
        }

        if (grounded)
        {
            _rigidBody2D.drag = 0.05f;
        }
        else
        {
            _rigidBody2D.drag = 2f;
        }

        if (jumped && grounded && Time.time > jumpTimer) 
        {
            playerVelocity.y = jumpHeight * -3.0f * gravityValue;
            _rigidBody2D.AddForce(playerVelocity);
            jumpTimer = Time.time + jumpCooldown;
            jumped = false;
        }else
        {
            jumped = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumped = true;
        }
    }

    public bool IsGrounded() 
    {
        
        RaycastHit2D hit = Physics2D.Raycast(_raycastOrigin.position, Vector3.down, raycastDist);
        Debug.DrawLine(_raycastOrigin.position, new Vector3(_raycastOrigin.position.x, _raycastOrigin.position.y - raycastDist, _raycastOrigin.position.z), Color.red);
        if (hit.collider != null && hit.collider.gameObject.tag == "ground")
        {
            return true;
        }
        return false;
    }
}