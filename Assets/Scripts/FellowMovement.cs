using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellowMovement : MonoBehaviour
{

    public YellowFellowGame game;
    string playerDirection = "left";

    public AudioClip moveSound;
    public AudioSource moveAudioSource;

    public float currentSpeed;
    public float defaultSpeed = 6f;

    void Start()
    {
        currentSpeed = defaultSpeed;
    }
    
    // player will move in one of four directions via the WASD keys
    void FixedUpdate()
    {
        // If game is paused via YellowFellowGame.cs, player won't move
        if (game.paused)
        {
            currentSpeed = 0;
            return;
        }

        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -currentSpeed;
            playerDirection = "left";
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = currentSpeed;
            playerDirection = "right";
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = currentSpeed;
            playerDirection = "up";
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -currentSpeed;
            playerDirection = "down";
            PlayMovementSound();
        }
        b.velocity = velocity;
    }
    // when player is moving, sound will play
    private void PlayMovementSound()
    {
        if (!moveAudioSource.isPlaying)
            moveAudioSource.PlayOneShot(moveSound);
    }

    // for the ghosts target methods, they use the current direction of player
    public string GetDirection()
    {
        return playerDirection;
    }
}
