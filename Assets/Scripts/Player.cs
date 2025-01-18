using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    public string sceneName1 = "Level";
    public string sceneName2 = "Level 2";
    public string sceneName3 = "Level 3";
    public string sceneName4 = "Level 4";
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 50f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    private bool doubleJump;

    Vector2 checkPointPos;
    [Header("Sound")]
    private AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip jumpSound;
    
    [Header("Something")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
     public Material Night;
     public Material Day;

    private int key = 0;
    private float scale;

    void Start()
    {
        checkPointPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        scale = transform.localScale.x;
        RenderSettings.skybox = Day;
    }


    void Update()
    {
        if (isDashing == true)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        if(isGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                doubleJump = !doubleJump;
            }
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
       
        
        if(!isWallJumping)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        if (transform.position.y < -4)
        {

            Die();
            Flip();
        }
        if(transform.position.y >= 146)
        {
            RenderSettings.skybox = Night;
        }
        else
        {
            RenderSettings.skybox = Day;
        }
        WallJump();
        WallSlide();
        Flip();


    }
    private void FixedUpdate()
    {
        if (isDashing == true)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if(!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Spike"))
        {
            Die();
            Flip();
        }

        else if (collision.CompareTag("Wind"))
        {
            Vector3 newPosition2 = transform.position;
            newPosition2.x += -20f;
            transform.position = newPosition2;
        }
        else if (collision.CompareTag("Super wind"))
        {
            Vector3 newPosition3 = transform.position;
            newPosition3.x += 126f;
            transform.position = newPosition3;
        }
        else if (collision.gameObject.CompareTag("key"))
        {
            Destroy(collision.gameObject);
            key++;
        }
        else if (key == 2)
        {
            if (collision.gameObject.CompareTag("Door"))
            {
                Destroy(collision.gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("Mini portal"))
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (collision.gameObject.CompareTag("Big portal"))
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    private void Die()
    {

        audioSource.PlayOneShot(deathSound);
        StartCoroutine(Respawn(0.1f));
    }
    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos = pos;
    }
    IEnumerator Respawn(float duration)
    {
        if(transform.localScale.x == 0.5)
        {
            rb.velocity = new Vector2(0, 0);
            rb.simulated = false;
            transform.localScale = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(duration);
            transform.position = checkPointPos;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            rb.simulated = true;
        }
        else if(transform.localScale.x == 0.25)
        {
            rb.velocity = new Vector2(0, 0);
            rb.simulated = false;
            transform.localScale = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(duration);
            transform.position = checkPointPos;
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            rb.simulated = true;
        }
        else if (transform.localScale.x == -0.25)
        {
            rb.velocity = new Vector2(0, 0);
            rb.simulated = false;
            transform.localScale = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(duration);
            transform.position = checkPointPos;
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            rb.simulated = true;
        }



    }
    private IEnumerator Dash()
    {
        Flip();
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        Flip();

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if (isWalled() && !isGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
    
        
            if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                wallJumpingCounter = 0;
                if(transform.localScale.x != wallJumpingDirection)
                {
                    isFacingRight = !isFacingRight;
                    Vector3 localScale = transform.localScale;
                    localScale.x *= -1f;
                    transform.localScale = localScale;
                }
                Invoke(nameof(StopWallJumping), wallJumpingDuration);
            }
        }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}