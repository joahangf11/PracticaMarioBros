using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Joahan Javier Garcia Fernandez 
 * email : A01748222@tec.mx
 * Set the functions for enemies movement and dead */


public class Enemy : MonoBehaviour
{

    //Instance variables
    public float velocityX = 1;
    private bool goRight;
    private bool isCrushed;
    private Animator animador;
    GameObject player;


    // Object's audio sources
    public AudioSource dieSound;
    public AudioSource dieMario;

    // Object collider
    private BoxCollider2D rb;


    // Start is called before the first frame update
    void Start()
    {
        //Object components
        rb = GetComponent<BoxCollider2D>();
        animador = GetComponent<Animator>();
        dieSound = GetComponent<AudioSource>();

        // Mario
        player = GameObject.FindGameObjectWithTag("Player");

        //Child components
        dieMario = gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
    }

    // Set the function for the enemy can go back 
    void FixedUpdate()
    {
        if (goRight)
        {
            transform.Translate(2 * Time.deltaTime * velocityX, 0, 0);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * velocityX, 0, 0);
        }

    }

    // Delimit the area the enemy goes through the map and movement in case of a collision with another enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Enemy"))
        {
            if (goRight)
            {
                goRight = false;
            }
            else
            {
                goRight = true;
            }
        }


        // Set the cases of collision with the player
        if (collision.gameObject.tag == "Player")
        {
            float yOffset = 0.3f;
            // If the player hits the top section of the enemy, it disappears and plays a sound
            if(transform.position.y + yOffset < collision.transform.position.y)
            {
                dieSound.Play();
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 4; 
                isCrushed = true;
                velocityX = 0;
                animador.SetBool("isCrushed",isCrushed);
                Destroy(gameObject,0.17f);
            }
            // If the player hits another section of the enemy, the player dies
            else
            {
                MarioMovement.death = true;
                dieMario.Play();
                velocityX = 0;
                animador.speed = 0f;
            }
        }

        // If the player falls out map dies, plays the death sound, and disables all movement controls
        if ( player.transform.position.y < -5.35f)
        {
            if (MarioMovement.death == false)
            {
                dieMario.Play();

            }
            MarioMovement.death = true;
            velocityX = 0;
            animador.speed = 0f;
        }
    }

}
