using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fellow : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 0.05f;
    [SerializeField] int pointsPerPellet = 100;
    [SerializeField] int pointsPerPowerup = 250;
    [SerializeField] int pointsPerGhost = 500;
    [SerializeField] float powerupDuration = 7.0f;

    public AudioClip deathSound, munchSound, munchGhostSound, moveSound;

    public Animator deathAnimation;

    float powerupTime = 0.0f;
    float speed;
    int score = 0;
    int pelletsEaten = 0;
    int lives = 3;
    
    string playerDirection = "left";
    Vector3 spawnLocation;

    public AudioSource eatAudioSource, moveAudioSource;

    void Start()
    {
        speed = defaultSpeed;
        spawnLocation = gameObject.transform.position;
        //audioSource = GetComponent<AudioSource>();

        deathAnimation.SetTrigger("Respawn");
    }
    
    private void FixedUpdate()
    {
        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
            if (!moveAudioSource.isPlaying)
                moveAudioSource.PlayOneShot(moveSound);
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
            if (!moveAudioSource.isPlaying)
                moveAudioSource.PlayOneShot(moveSound);
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
            if (!moveAudioSource.isPlaying)
                moveAudioSource.PlayOneShot(moveSound);
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
            if (!moveAudioSource.isPlaying)
                moveAudioSource.PlayOneShot(moveSound);
        }
        b.velocity = velocity;
       
        
    
        powerupTime = Mathf.Max(0.0f, powerupTime - Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pellet"))
        {
          
            eatAudioSource.PlayOneShot(munchSound);
         
           
            pelletsEaten++;
            score += pointsPerPellet;
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            pelletsEaten++;
            score += pointsPerPowerup;
            powerupTime = powerupDuration;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            if(PowerupActive())
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

    private void StartDeath()
    {
        Pause();
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        eatAudioSource.Stop();
        eatAudioSource.PlayOneShot(deathSound);
        // add animation and maybe particles
        deathAnimation.SetTrigger("Dies");
        Invoke(nameof(KillPlayer), 1);
    }

    private void KillPlayer()
    {
        gameObject.SetActive(false);
        lives = lives - 1;
    }

    public bool PowerupActive()
    {
        return powerupTime > 0.0f;
    }
    public int PelletsEaten()
    {
        return pelletsEaten;
    }

    public int getScore()
    {
        return score;
    }
    
    public int getLives()
    {
        return lives;
    }
    
    public string getDirection()
    {
        return playerDirection;
    }

    public void Pause()
    {
        speed = 0;
    }

    public void Resume()
    {
        speed = defaultSpeed;
    }

    public void respawn()
    {
        deathAnimation.SetTrigger("Respawn");
        Resume();
        gameObject.layer = LayerMask.NameToLayer("Fellow");
        gameObject.transform.position = spawnLocation;
        gameObject.SetActive(true);
    }
}
