using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector3 playerVelocity;

    private float playerSpeed = 100.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -30;

    private Vector2 movementInput = Vector2.zero;
    private bool initJump = false;
    private bool endJump = false;
    private Rigidbody2D _rigidBody2D;
    private Transform _transform;
    public Transform _raycastOrigin;
    private Vector2 gravity = new Vector2(0f, -30f);
    private Vector2 counterForce = new Vector2(0f, 0f);
    private float groundedDist = 0.01f;
    private bool grounded = false;
    private bool nextToTheGround = false;
    private bool doubleJump = true;
    private float nextToTheGroundDist = 1f;
    private float jumpCooldown = 0.5f;
    private float jumpTimer;
    private Vector2 move = Vector2.zero;
    private float controllerDeadZone = 0.3f;
    private float maxSpeedX = 10f;
    private RaycastHit2D hit;

    private void Awake()
    {
        _transform = transform;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        jumpTimer = Time.time;
    }

    void FixedUpdate()
    {
        Debug.Log(gravity.y);
        hit = Physics2D.Raycast(_raycastOrigin.position, Vector3.down, 2);
        grounded = IsGrounded();

        if (grounded && !doubleJump)
        {
            doubleJump = true;
        }

        nextToTheGround = IsNextToTheGround();

        if (initJump)
        {
            gravity.y = -30f;
        }
        if (endJump || _rigidBody2D.velocity.y < 0 && gravity.y == -30)
        {
            gravity.y = -65f;
            endJump = false;
        }

        _rigidBody2D.AddForce(gravity);
        if (movementInput.magnitude < controllerDeadZone && movementInput.magnitude > -controllerDeadZone)
        {
            counterForce.x = -_rigidBody2D.velocity.x * 10f;
            _rigidBody2D.AddForce(counterForce);
        }
        else
        {
            move.x = movementInput.x;
            move.y = 0;
            _rigidBody2D.AddForce(move * playerSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (grounded && _rigidBody2D.velocity.y < 0)
        {
            move.Set(_rigidBody2D.velocity.x, 0);
            _rigidBody2D.velocity = move;
        }

        move.Set(Math.Clamp(_rigidBody2D.velocity.x, -maxSpeedX, maxSpeedX), _rigidBody2D.velocity.y);
        _rigidBody2D.velocity = move;

        if (grounded)
        {
            _rigidBody2D.drag = 0.05f;
        }
        else
        {
            _rigidBody2D.drag = 3f;
        }

        if (initJump && (grounded || doubleJump))
        {
            move.Set(_rigidBody2D.velocity.x, 0);
            _rigidBody2D.velocity = move;
            playerVelocity.y = jumpHeight * -3.0f * gravityValue;
            _rigidBody2D.AddForce(playerVelocity);
            jumpTimer = Time.time + jumpCooldown;
            initJump = false;
            if (!grounded && doubleJump)
            {
                doubleJump = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && (nextToTheGround || doubleJump))
        {
            initJump = true;
        }
        if (context.canceled)
        {
            endJump = true;
        }
    }

    public bool IsGrounded()
    {
        if (hit.collider != null && hit.collider.gameObject.tag == "ground" && Vector2.Distance(hit.point, _raycastOrigin.position) < groundedDist)
        {
            return true;
        }
        return false;
    }

    public bool IsNextToTheGround()
    {
        if (hit.collider != null && hit.collider.gameObject.tag == "ground" && Vector2.Distance(hit.point, _raycastOrigin.position) < nextToTheGroundDist)
        {
            return true;
        }
        return false;
    }
}