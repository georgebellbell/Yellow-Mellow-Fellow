using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fellow : MonoBehaviour
{
    public AudioClip deathSound, munchSound, munchGhostSound, moveSound;
    public AudioSource eatAudioSource, moveAudioSource;

    public Animator deathAnimation;

    int pointsPerPellet = 100;
    int pointsPerPowerup = 250;
    int pointsPerGhost = 500;

    float eatGhostsPowerupTime = 0.0f;
    float eatGhostsPowerupDuration = 5.0f;

    float timeSlowPowerupTime = 0.0f;
    float timeSlowDuration = 5.0f;

    float doubleScorePowerupTime = 0.0f;
    float doublePointsDuration = 5.0f;

    float speed;
    public float defaultSpeed = 6f;
    
    int multiplier = 1;
    int score = 0;
    int collectablesEaten = 0;

    int lives = 3;
    
    string playerDirection = "left";
    Vector3 spawnLocation;

    void Start()
    {
        speed = defaultSpeed;
        spawnLocation = gameObject.transform.position;
    }
    
    // Movement and monitors time left on powerups
    private void FixedUpdate()
    {
        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
            PlayMovementSound();
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
            PlayMovementSound();
        }
        b.velocity = velocity;

        ReducePowerupTime();

        if (!IsDoublePointsActive())
        {
            multiplier = 1;
        }
        if (!IsTimeslowActive())
        {
            speed = defaultSpeed;
        }
    }

    // reduces powerup time
    private void ReducePowerupTime()
    {
        eatGhostsPowerupTime = Mathf.Max(0.0f, eatGhostsPowerupTime - Time.fixedDeltaTime);
        timeSlowPowerupTime = Mathf.Max(0.0f, timeSlowPowerupTime - Time.fixedDeltaTime);
        doubleScorePowerupTime = Mathf.Max(0.0f, doubleScorePowerupTime - Time.fixedDeltaTime);
    }
    // As player moves, sound will play
    private void PlayMovementSound()
    {
        if (!moveAudioSource.isPlaying)
            moveAudioSource.PlayOneShot(moveSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Several items in game that the player can eat
        switch (other.gameObject.tag)
        {
            // normal pellets that increment score
            case "Pellet":
                eatAudioSource.PlayOneShot(munchSound);
                collectablesEaten++;
                score += pointsPerPellet * multiplier;
                break;
            // allows player to eat ghosts, within a set time
            case "Powerup":
                collectablesEaten++;
                score += pointsPerPowerup * multiplier;
                eatGhostsPowerupTime = eatGhostsPowerupDuration;
                break;
                // Slows movement down briefly
            case "Timeslow":
                collectablesEaten++;
                timeSlowPowerupTime = timeSlowDuration;
                speed = defaultSpeed / 2;
                break;
                // All score based consumables are worth twice as much
            case "DoubleScore":
                collectablesEaten++;
                doubleScorePowerupTime = doublePointsDuration;
                multiplier = 2;
                break;
                // Player gets an extra life
            case "ExtraLife":
                collectablesEaten++;
                lives = lives + 1;
                break;
        }

    }

    // If Player gets hit by ghost and has no powerup, they die, otherwise they get points and sound plays
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            if(IsPowerupActive())
            {
                eatAudioSource.PlayOneShot(munchGhostSound);
                score += pointsPerGhost;
            }
            else
            {
                StartDeath();
            }
        }
    }

    //Player begins dying with sound effect, animation and other parameters changing
    private void StartDeath()
    {
        timeSlowPowerupTime = 0;
        doubleScorePowerupTime = 0;

        Pause();
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        eatAudioSource.Stop();
        eatAudioSource.PlayOneShot(deathSound);
        deathAnimation.SetTrigger("Dies");
        Invoke(nameof(KillPlayer), 1);
    }

    // Removes life from player and deactivates them until respawn is called
    private void KillPlayer()
    {
        gameObject.SetActive(false);
        lives = lives - 1;
    }

    //Respawn the player if killed, called by YellowFellowGame.cs
    public void respawn()
    {
        deathAnimation.SetTrigger("Respawn");
        gameObject.layer = LayerMask.NameToLayer("Fellow");
        gameObject.transform.position = spawnLocation;
        gameObject.SetActive(true);
        Resume();
    }

    // Stops player from moving when game is paused, player is killed or endgame is reached
    public void Pause()
    {
        speed = 0;
    }

    // Resumes player when they are respawned or the player unpauses game
    public void Resume()
    {
        speed = defaultSpeed;
    }

    // GETTERS AND SETTERS

    public bool IsPowerupActive()
    {
        return eatGhostsPowerupTime > 0.0f;
    }

    public bool IsDoublePointsActive()
    {
        return doubleScorePowerupTime > 0.0f;
    }

    public bool IsTimeslowActive()
    {
        return timeSlowPowerupTime > 0.0f;
    }
    public int PelletsEaten()
    {
        return collectablesEaten;
    }

    public int GetScore()
    {
        return score;
    }
    
    public int GetLives()
    {
        return lives;
    }
    
    public string GetDirection()
    {
        return playerDirection;
    }
}
