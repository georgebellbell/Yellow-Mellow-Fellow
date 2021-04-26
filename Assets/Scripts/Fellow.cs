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
    public float timeSlowDuration = 2.5f, doublePointsDuration = 5.0f;

    public AudioClip deathSound, munchSound, munchGhostSound, moveSound;
    public Material consumableColour;
    public Animator deathAnimation;

    float eatGhostsPowerupTime = 0.0f;
    float timeSlowPowerupTime = 0.0f;
    float doubleScorePowerupTime = 0.0f;
    int multiplier = 1;
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
       
        eatGhostsPowerupTime = Mathf.Max(0.0f, eatGhostsPowerupTime - Time.fixedDeltaTime);
        timeSlowPowerupTime = Mathf.Max(0.0f, timeSlowPowerupTime - Time.fixedDeltaTime);
        doubleScorePowerupTime = Mathf.Max(0.0f, doubleScorePowerupTime - Time.fixedDeltaTime);

        if (!(timeSlowPowerupTime > 0.0f))
        {
            Time.timeScale = 1f;
        }
        if (!DoublePointsActive())
        {
            multiplier = 1;
        }
           
              

    }
    private void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Pellet":
                eatAudioSource.PlayOneShot(munchSound);
                pelletsEaten++;
                score += pointsPerPellet * multiplier;
                break;
            case "Powerup":
                pelletsEaten++;
                score += pointsPerPowerup * multiplier;
                eatGhostsPowerupTime = powerupDuration;
                break;
            case "Timeslow":
                pelletsEaten++;
                timeSlowPowerupTime = timeSlowDuration;
                Time.timeScale = 0.5f;
                break;
            case "DoubleScore":
                doubleScorePowerupTime = doublePointsDuration;
                multiplier = 2;
                break;
            case "ExtraLife":
                lives = lives + 1;
                break;
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
        return eatGhostsPowerupTime > 0.0f;
    }

    public bool DoublePointsActive()
    {
        return doubleScorePowerupTime > 0.0f;
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
