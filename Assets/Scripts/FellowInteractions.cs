using System.Collections;
using UnityEngine;

public class FellowInteractions : MonoBehaviour
{
    public AudioClip deathSound, munchSound, munchGhostSound;
    public AudioSource interactionAudioSource;

    public Animator deathAnimation;

    ParticleSystem PlayerParticleSystem;

    Material currentPelletParticle;
    Material currentPowerUpParticle;
    Material currentEatGhostParticle;

    public Material[] allParticles;

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
        PlayerParticleSystem = GetComponent<ParticleSystem>();
        CheckDoubleScorePowerup();
        spawnLocation = gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        ReducePowerupTime();
       
        CheckDoubleScorePowerup();
     
        // if timeslow powerup is no longer active, player speed returns to normal
        if (!IsTimeslowActive())
        {
            GetComponent<FellowMovement>().currentSpeed = GetComponent<FellowMovement>().defaultSpeed;
        }
    }

    // reduces time of all powerups
    private void ReducePowerupTime()
    {
        eatGhostsPowerupTime = Mathf.Max(0.0f, eatGhostsPowerupTime - Time.fixedDeltaTime);
        timeSlowPowerupTime = Mathf.Max(0.0f, timeSlowPowerupTime - Time.fixedDeltaTime);
        doubleScorePowerupTime = Mathf.Max(0.0f, doubleScorePowerupTime - Time.fixedDeltaTime);
    }

    // depending on if DoubleScorePowerup is active, the certain particles emmitted will change as will score recieved
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

        GetComponent<FellowMovement>().currentSpeed = GetComponent<FellowMovement>().currentSpeed / 2;
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

    // Coroutine for when the player dies
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

    // checks if normal powerup for eating ghosts is active
    public bool IsPowerupActive()
    {
        return eatGhostsPowerupTime > 0.0f;
    }

    // checks if the double points powerup is active
    public bool IsDoublePointsActive()
    {
        return doubleScorePowerupTime > 0.0f;
    }

    // checks if the time slow poerup is active
    public bool IsTimeslowActive()
    {
        return timeSlowPowerupTime > 0.0f;
    }

    // Called by YellowFellowGame to check how many pellets have been eaten, if equal to total number, game is won
    public int PelletsEaten()
    {
        return collectablesEaten;
    }

    // Called by InGameScores to update score visually and output score to file
    public int GetScore()
    {
        return score;
    }

    // Called by YellowFellowGame to update lives visuallt and check if player has lost
    public int GetLives()
    {
        return lives;
    }
      
}
