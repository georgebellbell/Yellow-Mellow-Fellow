using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellowMovement : MonoBehaviour
{

    public YellowFellowGame game;
    string playerDirection = "left";

    public AudioClip moveSound;
    public AudioSource moveAudioSource;

    public float speed;
    public float defaultSpeed = 6f;

    void Start()
    {
        speed = defaultSpeed;
    }
    
    void FixedUpdate()
    {
        // If game is paused via YellowFellowGame.cs, player won't move
        if (game.paused)
        {
            speed = 0;
            return;
        }

        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
            playerDirection = "left";
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
            playerDirection = "right";
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
            playerDirection = "up";
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
            playerDirection = "down";
            PlayMovementSound();
        }
        b.velocity = velocity;
    }

    private void PlayMovementSound()
    {
        if (!moveAudioSource.isPlaying)
            moveAudioSource.PlayOneShot(moveSound);
    }

    public string GetDirection()
    {
        return playerDirection;
    }
}
