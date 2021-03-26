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
            velocity.x = -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
        }
        b.velocity = velocity;
     }

    [SerializeField]
    BoxCollider LeftTeleporter;

    [SerializeField]
    BoxCollider RightTeleporter;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 pos = transform.position;

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
            powerupTime += powerupDuration;
        }

        if (other.gameObject.CompareTag("LeftTeleporter"))
        {
            pos = RightTeleporter.ClosestPoint(pos);
            Debug.Log("LeftTeleporter found");
            pos.x = pos.x - 0.5f;
            transform.position = pos;
        }

        if (other.gameObject.CompareTag("RightTeleporter"))
        {
            pos = LeftTeleporter.ClosestPoint(pos);
            Debug.Log("RightTeleporter found");
            pos.x = pos.x + 0.5f;
            transform.position = pos;
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
                lives = lives - 1;
                gameObject.transform.position = spawnLocation;
                if(lives == 0)
                {
                    Debug.Log("You Died with a score of " + score);
                    gameObject.SetActive(false);
                }
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
    

}
