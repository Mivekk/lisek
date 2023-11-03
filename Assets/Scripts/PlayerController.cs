using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parametrs")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 3.0f;
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 9.0f;

    [Space(10)]

    private int lives = 3;
    private Vector2 startPosition;

    private Rigidbody2D rigidBody;
    private Animator animator;
    public LayerMask groundLayer;

    private const float rayLength = 1.3f;
    private bool isRunning = false;
    private bool facingRight = true;

    private const int keysNumber = 3;
    private int keysFound = 0;

    public TMP_Text endText;

    int score = 0;

    void Death()
    {
        lives--;

        Debug.Log("masz " + lives + " zyc");

        transform.position = startPosition;
    }

    void Awake()
    {
        startPosition = transform.position;

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            score++;

            Debug.Log("score " + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Meta"))
        {
            endText.fontSize = 32;

            if (keysFound == keysNumber)
            {
                Debug.Log("wygrana");
            } else
            {
                Debug.Log("nie zebrano wszystkich kluczy PORAZKA");
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                score += 5;
                Debug.Log("zabito orla");
            }
            else {
                score -= 10;
                
                if (lives <= 0)
                {
                    Debug.Log("umarles gosciu");
                }
                else
                {
                    Debug.Log("masz " + lives + " zyc");

                    transform.position = startPosition;
                }

                lives--;
            }
            
        }
        else if (other.CompareTag("Klucz"))
        {
            keysFound++;

            other.gameObject.SetActive(false);

            Debug.Log("zebrano klucz");
        }
        else if (other.CompareTag("Heart"))
        {
            lives++;

            Debug.Log("dodano zycie masz " + lives);

            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("FallLevel"))
        {
            Death();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Debug.Log("uhu jestem na  gdzies na szczycie ");
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isRunning = false;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!facingRight)
            {
                Flip();
            }

            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isRunning = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (facingRight)
            {
                Flip();
            }

            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isRunning = true;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", IsGrounded());
    }
}
