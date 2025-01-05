using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    public string sceneName2 = "Level";
    [Header("Sound")]
    private AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip jumpSound;
    public AudioClip stageSound;
    [Header("Something")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private int key = 0;
    
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        Flip();
        if (transform.position.y < -4)
        {
           
            Die();
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
        if (collision.CompareTag("Start"))
        {
            SceneManager.LoadScene(sceneName1);
        }
        else if (collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene(sceneName2);
        }
        else if (collision.CompareTag("Spike"))
        {
            Die();
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
    

    }
    private void Die()
    {
       
        audioSource.PlayOneShot(deathSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}



