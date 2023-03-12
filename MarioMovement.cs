using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Joahan Javier Garcia Fernandez 
 * email : A01748222@tec.mx
 * Set the functions and parameters for vertical (jump) and horizontal movements and dead */

public class MarioMovement : MonoBehaviour
{

    //Instance variables
    [SerializeField]
    private float velocityX;
    [SerializeField]
    private float velocityY;
    [SerializeField]
    private float jumpForce;
    private bool canJump = true;
    public static bool isJumping = true;
    public static bool death;

    //Player's rigidbody 
    private Rigidbody2D rb;

    //Player's collider 
    private CapsuleCollider2D c;

    //Player's animator
    private Animator animador;

    //Player's sprite renderer
    private SpriteRenderer flip;

    //Physics 
    private Vector2 movement = new Vector2(0, 0);
    private float countdown=0.5f;

    // Object's audio sources
    public AudioSource jumpSound;
    public AudioSource backgroundSound;
    public AudioSource dieSound;


    // Start is called before the first frame update
    void Start()
    {


        // Child components
        backgroundSound = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        dieSound = gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>();

        //Object components
        c = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animador = GetComponent<Animator>();
        flip = GetComponent<SpriteRenderer>();
        jumpSound = GetComponent<AudioSource>();
        death = false;
    }


    void FixedUpdate()
    {
        // Player's horizontal movement
        float horizontalMov = Input.GetAxis("Horizontal");
        movement.x = horizontalMov * velocityX;
        movement.y = rb.velocity.y;
        rb.velocity = movement;


        // Set parameter "velocity" in animator and flip the sprite if the player is going left
        if (rb.velocity.x > 0)
        {
            animador.SetFloat("velocity", rb.velocity.x);
            flip.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            animador.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
            flip.flipX = true;
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Play the jump sound and set the jump function
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (canJump == true)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                canJump = false;
                isJumping = true;
                animador.SetBool("isJumping", isJumping);
                jumpSound.Play();
            }

        }


        // If the player falls out map dies, plays the death sound, and disables all movement controls
        if (transform.position.y < -5.35f)
        {
            if (death == false)
            {
                dieSound.Play();

            }
            MarioMovement.death = true;
            velocityX = 0;
            animador.speed = 0f;
        }


        // Set the dead function with their animation and reload the game scene
        if (death)
        {
            animador.SetTrigger("death");
            backgroundSound.Stop();
            countdown -= Time.deltaTime;
            canJump = false;
            velocityX = 0;
            if (countdown > 0f)
            {
                Invoke("Death", 0.5f);
            }
            if (transform.position.y < -30)
            SceneManager.LoadScene("SampleScene");
        }
    }


    // Allows the player to jump again if it lands off in a "platform" tag
    void OnCollisionEnter2D(Collision2D col)
    {
        // Triying this (transform.position.y > col.transform.position.y) next chance
        if (col.gameObject.CompareTag("platform"))
        {
            canJump = true;
            isJumping = false;
            animador.SetBool("isJumping", isJumping);
        }
    }

    // Destroy the collider and set the (go up and down) transition 
    void Death()
    {
        rb.velocity = new Vector2(rb.velocity.x, 5f);
        Destroy(c);
    }



}


