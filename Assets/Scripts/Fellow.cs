using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fellow : MonoBehaviour
{
    [SerializeField]
    float speed = 0.05f;

    int score = 0;
    int pelletsEaten = 0;

    [SerializeField]
    int pointsPerPellet = 100;

    [SerializeField]
    int pointsPerPowerup = 250;

    [SerializeField]
    int pointsPerGhost = 500;

    [SerializeField]
    float powerupDuration = 10.0f;

    float powerupTime = 0.0f;

    int lives = 3;

    Vector3 spawnLocation;

    string playerDirection = "left";


    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = gameObject.transform.position;
        
    }

    // Update is called once per frame
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

    void FixedUpdate()
    {
        
        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            playerDirection = "left";
            velocity.x = -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerDirection = "right";
            velocity.x = speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerDirection = "up";
            velocity.z = speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerDirection = "down";
            velocity.z = -speed;
        }
        b.velocity = velocity *Time.deltaTime;
        
     }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pellet"))
        {
            pelletsEaten++;
            score += pointsPerPellet;
            Debug.Log("Score is: " + score);
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            pelletsEaten++;
            score += pointsPerPowerup;
            powerupTime = powerupDuration;
        }
    }
    public bool PowerupActive()
    {
        return powerupTime > 0.0f;
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
                gameObject.SetActive(false);
                lives = lives - 1;   
            }
        }
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
        speed = newSpeed;
    }

    public void respawn()
    { 
        gameObject.transform.position = spawnLocation;
        gameObject.SetActive(true);
    }
}
