using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxVelocity = 0.1f;
    public float maxSpeed = 0.1f;
    float energy;
    private Vector3 velocity;
    public Player player;
    public GameObject ground;
    public GameObject blackhole;
    Vector3 bounds = new Vector3();
    private void Start()
    {
        velocity = Vector3.zero;
        bounds = ground.GetComponent<Renderer>().bounds.size / 2;
    }

    void Update()
    {
        SeekPlayer();
        if (player.energy > player.maxEnergy / 2)
        {
            SeekPlayer();
            StealEnergy();
        }
        if (transform.position.x < -bounds.x || transform.position.x > bounds.x)
        {
            ResetPosition();
        }
        if (transform.position.z < -bounds.z || transform.position.z > bounds.z)
        {
            ResetPosition();
        }
    }
    void SeekPlayer()
    {
        Vector3 target = player.transform.position - transform.position;

        //normalize it to get direction
        target = target.normalized;

        //now make a new raycast hit
        //and draw a line from the AI out some distance in the ‘forward direction

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 20.0f))
        {

            //check that its not hitting itself
            //then add the normalised hit direction to your direction plus some repulsion force -in my case // 400f

            if (hit.transform != transform)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);

                target += hit.normal * 2.5f;
            }

        }

        //now make two more raycasts out to the left and right to make the cornering more accurate and reducing collisions more

        Vector3 leftR = transform.position;
        Vector3 rightR = transform.position;

        leftR.x -= 2;
        rightR.x += 2;

        if (Physics.Raycast(leftR, transform.forward, out hit, 20.0f))
        {
            if (hit.transform != transform)
            {
                Debug.DrawLine(leftR, hit.point, Color.red);
                target += hit.normal * 2.5f;
            }

        }
        if (Physics.Raycast(rightR, transform.forward, out hit, 20.0f))
        {
            if (hit.transform != transform)
            {
                Debug.DrawLine(rightR, hit.point, Color.red);

                target += hit.normal * 2.5f;
            }
        }

        // then set the look rotation toward this new target based on the collisions

        Quaternion lookAtTarget = Quaternion.LookRotation(target);

        //then slerp the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtTarget, Time.deltaTime * 10.0f);

        //finally add some propulsion to move the object forward based on this rotation
        //mine is a little more complicated than below but you hopefully get the idea…

        transform.position += transform.forward * 10.0f * Time.deltaTime;
    }
  
    void StealEnergy()
    {
        if (player.enemyCollision && player.energy > player.minEnergy)
        {
            energy += 0.01f;
            print("ENEMY ENERGY: " + energy);
            Vector4 colour = GetComponent<Renderer>().material.color = new Vector4(0, 0, energy, 1);
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.transform.name.StartsWith("Blackhole"))
        {
            ResetPosition();
        }
    }
    void ResetPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(-bounds.x, bounds.x), 0.0f, (Random.Range(-bounds.z, bounds.z)));
        transform.position = newPosition;
    }
}
