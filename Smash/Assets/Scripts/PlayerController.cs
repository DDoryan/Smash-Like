using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum Hit
    {
        ligth,
        heavy
    }
    public Hit lastHit;
    private Vector3 playerVelocity;
    private Vector2 bumpVelocity;

    private float playerSpeed = 100.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -30;

    private Vector2 movementInput = Vector2.zero;
    private bool initJump = false;
    private bool endJump = false;
    private DamageManager _damageManager;
    private Rigidbody2D _rigidBody2D;
    private Transform _transform;
    public Transform _raycastOrigin;
    private Vector2 gravity = new Vector2(0f, -30f);
    private Vector2 counterForce = new Vector2(0f, 0f);
    private float groundedDist = 0.01f;
    public float playerScale = 0.36f;
    private Vector2 playerOrientation;
    private bool grounded = false;
    private bool nearToTheGround = false;
    private bool isStun = false;
    private bool doubleJump = true;
    private bool isLigthHit = false;
    private bool isHeavyHit = false;
    private float nextToTheGroundDist = 1f;
    private float inputEnableTimer = 0.2f;
    private float ligthHitCooldown = 0.5f;
    private float ligthHitTimer;
    private float heavyHitCooldown = 0.5f;
    private float heavyHitTimer;
    private float stunCooldown = 0.5f;
    private float stunTimer;
    private Vector2 move = Vector2.zero;
    private float controllerDeadZone = 0.3f;
    private float maxSpeedX = 10f;
    private RaycastHit2D hit;
    public Animator _Animator;
    private float angleBump = 0.65f;



    private void Awake()
    {
        _transform = transform;
        _transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        playerOrientation = new Vector2(playerScale, playerScale);
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _damageManager = GetComponent<DamageManager>();
        ligthHitTimer = Time.time;
        heavyHitTimer = Time.time;
        bumpVelocity = new Vector2(0f, 0f);
    }

    private void Update()
    {
        //Debug.Log(isStun);
        if (isLigthHit && Time.time > ligthHitTimer)
        {
            _Animator.SetTrigger("ligthHit");
            isLigthHit = false;
            lastHit = Hit.ligth;
            ligthHitTimer = Time.time + ligthHitCooldown;
        }

        if (isHeavyHit && Time.time > heavyHitTimer)
        {
            _Animator.SetTrigger("heavyHit");
            isHeavyHit = false;
            lastHit = Hit.heavy;
            heavyHitTimer = Time.time + heavyHitCooldown;
        }
    }

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(_raycastOrigin.position, Vector3.down, 2);
        grounded = IsGrounded();

        if (grounded && !doubleJump)
        {
            doubleJump = true;
        }

        nearToTheGround = IsNearToTheGround();

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

        if (isStun && Time.time > stunTimer)
        {
            isStun = false;
        }
        if (!isStun)
        {
            if (movementInput.magnitude < controllerDeadZone && movementInput.magnitude > -controllerDeadZone)
            {
                counterForce.x = -_rigidBody2D.velocity.x * 10f;
                _rigidBody2D.AddForce(counterForce);
                Debug.Log("c'est la merde");
            }
            else
            {
                move.x = movementInput.x;
                move.y = 0;
                _rigidBody2D.AddForce(move * playerSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            move.Set(Math.Clamp(_rigidBody2D.velocity.x, -maxSpeedX, maxSpeedX), _rigidBody2D.velocity.y);
            _rigidBody2D.velocity = move;
        }

        if (grounded && _rigidBody2D.velocity.y < 0)
        {
            move.Set(_rigidBody2D.velocity.x, 0);
            _rigidBody2D.velocity = move;
        }

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
            initJump = false;
            if (!grounded && doubleJump)
            {
                doubleJump = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isStun)
        {
            movementInput = context.ReadValue<Vector2>();
            if (movementInput.x > controllerDeadZone)
            {
                playerOrientation.Set(playerScale, playerScale);
            }
            else if (movementInput.x < -controllerDeadZone)
            {
                playerOrientation.Set(-playerScale, playerScale);
            }
            _transform.localScale = playerOrientation;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isStun)
        {
            if (context.performed && (nearToTheGround || doubleJump))
            {
                initJump = true;
            }
            if (context.canceled)
            {
                endJump = true;
            }
        }
    }

    public void OnLigthHit(InputAction.CallbackContext context)
    {
        if (!isStun)
        {
            if (context.performed && Time.time > (ligthHitTimer - inputEnableTimer))
            {
                isLigthHit = true;
            }
        }
    }

    public void OnHeavyHit(InputAction.CallbackContext context)
    {
        if (!isStun)
        {
            if (context.performed && Time.time > (heavyHitTimer - inputEnableTimer))
            {
                isHeavyHit = true;
            }
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

    public bool IsNearToTheGround()
    {
        if (hit.collider != null && hit.collider.gameObject.tag == "ground" && Vector2.Distance(hit.point, _raycastOrigin.position) < nextToTheGroundDist)
        {
            return true;
        }
        return false;
    }

    public void Bump(float power, bool isDirRigth)
    {
        float damageReceived = _damageManager.GetDamageReceived();
        bumpVelocity.Set((1 - angleBump) * power * damageReceived, angleBump * power * damageReceived);
        if (!isDirRigth)
        {
            bumpVelocity.Set(bumpVelocity.x * -1, bumpVelocity.y);
        }
        isStun = true;
        stunTimer = Time.time + stunCooldown;
        _rigidBody2D.AddForce(bumpVelocity);
    }
}