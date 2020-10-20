using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float jumpHeight = 9f;
    public float rotationFactor = 6f;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private Transform tf;
    public Animator animator;
    public float gravityScale = 1.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        tf = this.GetComponent<Transform>();
        startPosition = transform.position;
    }

    // Update is called once per frame 
    void Update()
    {

        moveBeforeGame();

        AddGravity();

        Jump();

        //Rotate();

    }

    void moveBeforeGame() 
    {
        if (!GameManager.instance.gameHasStarted && !GameManager.instance.gameHasEnded) // up and down motion
        {
            transform.position = startPosition + new Vector3(0.0f, Mathf.Sin(Time.time * 4) / 2, 0.0f) ;
        }
    }

    void AddGravity() 
    {
        if (SceneManager.GetActiveScene().name == "Game" && !GameManager.instance.gameHasStarted && Input.GetMouseButtonDown(0)) // start game
        {
            GameManager.instance.StartGame();
            rb.gravityScale = gravityScale;
        }
    }

    void Jump()
    {
        animator.SetFloat("YVelocity", rb.velocity.y);
        
        if (SceneManager.GetActiveScene().name == "Game" && Input.GetMouseButtonDown(0) && !GameManager.instance.gameHasEnded) { //jumping
             rb.velocity = Vector2.up * jumpHeight;
             GameEvents.current.Jump();
             AudioManager.instance.Play("Jump");
             animator.SetTrigger("Jumped");
        }
    }
    void Rotate()
    {
        float angle = rb.velocity.y * rotationFactor; // rotate around angle
        angle = Mathf.Clamp(angle, -90f, 60f);
        tf.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
    

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.layer == 8 && !GameManager.instance.gameHasEnded) // obstacle laver
        {
            animator.SetBool("IsDead", true);
            GameManager.instance.EndGame();
            AudioManager.instance.Play("Death");
        }
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.layer == 9) // jonko layer
        {
            GameManager.instance.PassedObstacle();
        }
 
    }

}
