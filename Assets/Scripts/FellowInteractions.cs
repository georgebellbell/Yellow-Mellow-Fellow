using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellowInteractions : MonoBehaviour
{
    public AudioClip deathSound, munchSound, munchGhostSound;
    public AudioSource interactionAudioSource;

    public Animator deathAnimation;

    public ParticleSystem PlayerParticleSystem;

    Material currentPelletParticle;
    Material currentPowerUpParticle;
    Material currentEatGhostParticle;

    float speed;

    public Material[] allParticles;

    public YellowFellowGame game;

    int pointsPerPellet = 100;
    int pointsPerPowerup = 250;
    int pointsPerGhost = 500;

    float eatGhostsPowerupTime = 0.0f;
    float eatGhostsPowerupDuration = 5.0f;

    float timeSlowPowerupTime = 0.0f;
    float timeSlowDuration = 5.0f;

    float doubleScorePowerupTime = 0.0f;
    float doublePointsDuration = 5.0f;

    int multiplier = 1;
    int score = 0;
    int collectablesEaten = 0;

    int lives = 3;
    
    Vector3 spawnLocation;

    void Start()
    {
        CheckDoubleScorePowerup();
        speed = GetComponent<FellowMovement>().speed;
        spawnLocation = gameObject.transform.position;
    }

    // Movement and monitors time left on powerups
    private void FixedUpdate()
    {
        ReducePowerupTime();
       
        CheckDoubleScorePowerup();
     
        if (!IsTimeslowActive())
        {
            GetComponent<FellowMovement>().speed = GetComponent<FellowMovement>().defaultSpeed;
        }
    }

    // reduces powerup time
    private void ReducePowerupTime()
    {
        eatGhostsPowerupTime = Mathf.Max(0.0f, eatGhostsPowerupTime - Time.fixedDeltaTime);
        timeSlowPowerupTime = Mathf.Max(0.0f, timeSlowPowerupTime - Time.fixedDeltaTime);
        doubleScorePowerupTime = Mathf.Max(0.0f, doubleScorePowerupTime - Time.fixedDeltaTime);
    }

    // depending on if DoubleScorePowerup is active, certain particles emmitted will change
    void CheckDoubleScorePowerup()
    {
        if (IsDoublePointsActive())
        {
            multiplier = 2;
            currentPelletParticle = allParticles[3];
            currentPowerUpParticle = allParticles[4];
            currentEatGhostParticle = allParticles[5];
        }
        else
        {
            multiplier = 1;
            currentPelletParticle = allParticles[0];
            currentPowerUpParticle = allParticles[1];
            currentEatGhostParticle = allParticles[2];
        }

    }


    // ---------------------------------- CONSUMABLES -------------------------------------------------------


    private void OnTriggerEnter(Collider other)
    {
        //Several items in game that the player can eat
        switch (other.gameObject.tag)
        {
            // normal pellets that increment score
            case "Pellet":
                EatPellet();
                break;

            // allows player to eat ghosts, within a set time
            case "Powerup":
                EatPowerup();
                break;

            // Slows movement down briefly
            case "Timeslow":
                EatTimeslowPowerup();
                break;

            // All score based consumables are worth twice as much
            case "DoubleScore":
                EatDoubleScorePowerup();
                break;

            // Player gets an extra life
            case "ExtraLife":
                EatExtraLifePowerup();
                break;
        }
    }
    void EatPellet()
    {
        PlayParticle(currentPelletParticle);
        interactionAudioSource.PlayOneShot(munchSound);

        collectablesEaten++;
        score += pointsPerPellet * multiplier;
    }
    void EatPowerup()
    {  
        PlayParticle(currentPowerUpParticle);

        collectablesEaten++;
        score += pointsPerPowerup * multiplier;

        eatGhostsPowerupTime = eatGhostsPowerupDuration;
    }
    void EatTimeslowPowerup()
    {
        PlayParticle(allParticles[6]);

        collectablesEaten++;

        GetComponent<FellowMovement>().speed = GetComponent<FellowMovement>().speed / 2;
        timeSlowPowerupTime = timeSlowDuration;
    }
    void EatDoubleScorePowerup()
    {
        PlayParticle(allParticles[7]);

        collectablesEaten++;
        
        doubleScorePowerupTime = doublePointsDuration;
        CheckDoubleScorePowerup();
    }
    void EatExtraLifePowerup()
    {
        PlayParticle(allParticles[8]);

        collectablesEaten++;

        lives = lives + 1;
    }

    // depending on item eaten, a specific particle will be emitted by particle system
    void PlayParticle(Material particleToPlay)
    {
        // if a powerup is eaten, all current particles are cleared
        if (particleToPlay != allParticles[0] && particleToPlay != allParticles[3])
        {
            PlayerParticleSystem.Clear();
        }
            
        PlayerParticleSystem.GetComponent<ParticleSystemRenderer>().material = particleToPlay;
        PlayerParticleSystem.Play();
    }


    // ---------------------------------- GHOSTS -------------------------------------------------------


    // If Player gets hit by ghost and has no powerup, they die, otherwise they get points and sound plays
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            if(IsPowerupActive())
            {
                PlayParticle(currentEatGhostParticle);
                interactionAudioSource.PlayOneShot(munchGhostSound);
                score += pointsPerGhost * multiplier;
            }
            else
                StartCoroutine(PlayerDies());
        }
    }

    IEnumerator PlayerDies()
    {
        // player set to same layer of ghost, to prevent player losing anymore lives during death and eat any more collectables
        gameObject.layer = LayerMask.NameToLayer("Ghost");

        // If player dies, time on any active powerups ends
        timeSlowPowerupTime = 0;
        doubleScorePowerupTime = 0;

        // any sound playing replaced by player dying sound
        interactionAudioSource.Stop();
        interactionAudioSource.PlayOneShot(deathSound);

        deathAnimation.SetTrigger("Dies");

        yield return new WaitForSeconds(1);

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
    }


    // ---------------------------------- GETTERS AND SETTERS -------------------------------------------------------


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
      
}
