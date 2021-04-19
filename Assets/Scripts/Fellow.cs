using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fellow : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 0.05f;
    [SerializeField] int pointsPerPellet = 100;
    [SerializeField] int pointsPerPowerup = 250;
    [SerializeField] int pointsPerGhost = 500;
    [SerializeField] float powerupDuration = 10.0f;

    [SerializeField] AudioClip deathSound;

    float powerupTime = 0.0f;
    float speed;
    int score = 0;
    int pelletsEaten = 0;
    int lives = 3;
    
    string playerDirection = "left";
    Vector3 spawnLocation;

    AudioSource audioSource;

    void Start()
    {
        speed = defaultSpeed;
        spawnLocation = gameObject.transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
           
            pos.x += speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            
            pos.z += speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
           
            pos.z -= speed;
        }
        transform.position = pos;

        powerupTime = Mathf.Max(0.0f, powerupTime - Time.deltaTime);
    }

    

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pellet"))
        {
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
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        // add animation and maybe particles
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

    public void setSpeed(float newSpeed)
    {
        defaultSpeed = newSpeed;
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
        Resume();
        gameObject.layer = LayerMask.NameToLayer("Fellow");
        gameObject.transform.position = spawnLocation;
        gameObject.SetActive(true);
    }
}
