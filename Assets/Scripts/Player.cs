using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Rigidbody rBody;
    public GameObject ground;
    public float speed = 0.1f;
    //energy variables
    public float energy;
    public float minEnergy = 0.0f;
    public float maxEnergy = 1.0f;
    public float gameTimer = 0.0f;
    public Text playerName;
    public static int score;
    public bool enemyCollision;
    //colour variables affected by energy levels
    public Color emptyColour;
    public Color fullColour;
    //an instance of my camera class for following the player
    FollowCamera myCamera;
    //scene changer class
    MainMenu scenes;
    Mushroom mushroomScript;
    Vector3 bounds = new Vector3();
    //allows updating the vec3 positions of the playerwhen required
    Vector3 UpdatePosition(float x, float z)
    {
        return new Vector3(x, transform.position.y, z);
    }

    void Start()
    {
        score = 0;
        energy = minEnergy;
        rBody = GetComponent<Rigidbody>();
        myCamera = FindObjectOfType<FollowCamera>();
        bounds = ground.GetComponent<Renderer>().bounds.size / 2;
        scenes = FindObjectOfType<MainMenu>();
    }

    void Update()
    {
        //ensuring the player can not die by running off the edges of the screen on start
        WorldWrapping();
        Movement();
        gameTimer += Time.deltaTime;
        if (gameTimer > 90)
        {
            scenes.GameOver();
        }
    }
    //simply setting the players position to *-1 of itself based on the axis (x/z)
    void WorldWrapping()
    {
        if (transform.position.x > bounds.x || transform.position.x < -bounds.x)
        {
            transform.position = UpdatePosition(transform.position.x * -1, transform.position.z);
        }
        if (transform.position.z > bounds.z || transform.position.z < -bounds.z)
        {
            transform.position = UpdatePosition(transform.position.x, transform.position.z * -1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //checking for collisions with mushrooms
        if (collision.transform.name.StartsWith("collectable"))
        {
            mushroomScript = collision.gameObject.GetComponent<Mushroom>();
            if (mushroomScript.edible == true)
            {
                //increase the score, remove the mushroom if collision + edible
                score++;
                Destroy(collision.gameObject);
                if (energy < maxEnergy)
                {
                    //increase energy and change colour if energy is less than max energy
                    energy += 0.1f;
                    GetComponent<Renderer>().material.color = Color.Lerp(emptyColour, fullColour, energy);
                }
                if (mushroomScript.edible == false)
                {
                    //decrease score, energy and colour if mushroom was not edible
                    score--;
                    energy -= 0.1f;
                    GetComponent<Renderer>().material.color = Color.Lerp(emptyColour, fullColour, energy);
                }
            }
            else return;
        }

        if (collision.transform.name.StartsWith("Enemy"))
        {
            enemyCollision = true;
            //enemy steals energy from player if a collision is made
            if (energy > minEnergy)
            {
                energy -= 0.1f;
                GetComponent<Renderer>().material.color = Color.Lerp(emptyColour, fullColour, energy);
            }
            else energy = minEnergy;
            //set the score to 0 if it would fall below 0
            if (score > 0)
            {
                score--;
            }
            else score = 0;
        }
        //set the game over screen to active if the player enters the blackhole
        if (collision.transform.name.StartsWith("Blackhole"))
        {
            scenes.GameOver();
        }
    }
    //move the player based on time and the camera forward rotation
    //use w/a/s/d to look around
    void Movement()
    {
        transform.forward = myCamera.transform.forward;
       
        if (gameTimer > 0)
        {
            transform.position = transform.position + myCamera.transform.forward * speed * Time.deltaTime;
        }
    }
}