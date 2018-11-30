using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Rigidbody rBody;
    public GameObject ground;
    public float speed = 0.1f;
    public float energy;
    public float minEnergy = 0.0f;
    public float maxEnergy = 1.0f;
    public float gameTimer = 0.0f;
    public Text playerName;
    static public int score;
    public bool enemyCollision;
    public Color emptyColour;
    public Color fullColour;
    FollowCamera myCamera;
    MainMenu scenes;
    Mushroom mushroomScript;
    Vector3 bounds = new Vector3();
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
        WorldWrapping();
        Movement();
        gameTimer += Time.deltaTime;
    }

    void WorldWrapping()
    {
        if (transform.position.x > bounds.x || transform.position.x < -bounds.x)
        {
            transform.position = UpdatePosition(transform.position.x * -1.0f, transform.position.z);
        }
        if (transform.position.z > bounds.z || transform.position.z < -bounds.z)
        {
            transform.position = UpdatePosition(transform.position.x, transform.position.z * -1.0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.StartsWith("collectable"))
        {
            mushroomScript = collision.gameObject.GetComponent<Mushroom>();
            if (mushroomScript.edible == true)
            {
                score++;
                if (energy < maxEnergy)
                {
                    energy += 0.1f;
                    GetComponent<Renderer>().material.color = Color.Lerp(emptyColour, fullColour, energy);
                    Debug.Log("SCORE " + score);
                    Debug.Log("ENERGY " + energy);
                    Destroy(collision.gameObject);
                }
            }
            else return;
        }

        if (collision.transform.name.StartsWith("Enemy"))
        {
            enemyCollision = true;

            if (energy > minEnergy)
            {
                energy -= 0.1f;
                GetComponent<Renderer>().material.color = Color.Lerp(emptyColour, fullColour, energy);
            }
            else energy = minEnergy;

            if (score > 0)
            {
                score--;
            }
            else score = 0;

            Debug.Log("SCORE " + score);
            Debug.Log("ENERGY " + energy);
        }

        if (collision.transform.name.StartsWith("Blackhole"))
        {
            //Destroy(transform.gameObject);
            Debug.Log("GAME OVER, MAN!");
            scenes.GameOver();
        }
    }
    void Movement()
    {
        transform.forward = myCamera.transform.forward;
       
        if (gameTimer > 0)
        {
            transform.position = transform.position + myCamera.transform.forward * speed * Time.deltaTime;
        }
    }
}