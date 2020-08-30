using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    private float movementInputDirection;

    private int amountOfJumpsLeft;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool checkJumpMultiplier;
    private bool canJump;
    private bool canMove = true;
    private bool canFlip = true;
    private float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }
    
    void Update()
    {
        CheckInput();
        MovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        UpdateMovingState();
        IsGround();
    }

    //------ Funkcja odpowiedzialna za sprawdzanie czy jesteśmy na ziemi ------
    private void IsGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    //------ Funkcja odpowiedzialna za sprawdzanie czy można skoczyć ------
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

    }

    //------ Funkcje odpowiedzialne za sterowanie ------
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(movementInputDirection));

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0))
            {
                Jump();
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {

            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    //------ Funkcja odpowiedzialna za skok ------
    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            checkJumpMultiplier = true;
        }
    }

    //------ Funkcje odpowiedzialne za poruszanie się ------
    private void UpdateMovingState()
    {

        if (!isGrounded && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
    }

    //------ Funkcje odpowiedzialne za obrót w przeciwny kierunek ------
    private void MovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void Flip()
    {
        if (canFlip)
        {
            isFacingRight = !isFacingRight;
            Vector3 _setFlip = new Vector3(isFacingRight ? 1:-1, 1, 1);
            this.transform.localScale = _setFlip;
            //transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    //------ INNE FUNKCJE ------------------------------------------------------------

    //------ Funkcje odpowiedzialna za animacje ------
    private void UpdateAnimations()
    {
        //anim.SetBool("isWalking", isWalking);
        //anim.SetBool("isGrounded", isGrounded);
        //anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public bool GetIsFacingRight()
    {
        return isFacingRight;
    }

    public bool GetIsWalking()
    {
        return isWalking;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
}
