using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rBody;
    public GameObject ground;
    public float speed = 0.1f;
    public float energy;
    public float minEnergy = 0.0f;
    public float maxEnergy = 1.0f;
    int score;
    public bool enemyCollision;
    public Color emptyColour;
    public Color fullColour;
    FollowCamera myCamera;
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
    }

    void Update()
    {
        WorldWrapping();
        Movement();
    }

    void WorldWrapping()
    {
        if (transform.position.x > bounds.x || transform.position.x < -bounds.x)
        {
            transform.position = UpdatePosition(transform.position.x * -1f, transform.position.z);
        }
        if (transform.position.z > bounds.z || transform.position.z < -bounds.z)
        {
            transform.position = UpdatePosition(transform.position.x, transform.position.z * -1f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.StartsWith("collectable"))
        {
            Destroy(collision.gameObject);
            if (energy < maxEnergy)
            {
                energy += 0.1f;
                score++;
                GetComponent<Renderer>().material.color = Color.Lerp(emptyColour, fullColour, energy);
                Debug.Log("SCORE " + score);
                Debug.Log("ENERGY " + energy);
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

        if (collision.transform.name.StartsWith("blackhole"))
        {
            Destroy(transform.gameObject);
            Debug.Log("GAME OVER, MAN!");
        }
    }
    void Movement()
    {
        transform.forward = myCamera.transform.forward;
       
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + myCamera.transform.forward * speed * Time.deltaTime;
        }
    }
}