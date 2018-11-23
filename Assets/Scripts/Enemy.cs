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
    Vector3 bounds = new Vector3();
    private void Start()
    {
        velocity = Vector3.zero;
        bounds = ground.GetComponent<Renderer>().bounds.size / 2;
    }

    void Update()
    {
        if (player.energy != player.minEnergy)
        {
            SeekPlayer();
        }
    }
    void SeekPlayer()
    {
            var desiredVelocity = player.transform.position - transform.position;
            desiredVelocity = desiredVelocity.normalized * maxVelocity;

            var steering = desiredVelocity - velocity;
            steering = Vector3.ClampMagnitude(steering, maxSpeed);

            velocity = Vector3.ClampMagnitude(velocity + steering, maxVelocity);
            transform.position += velocity * Time.deltaTime;
            transform.forward = velocity.normalized;
    }
    void StealEnergy()
    {
        if (player.enemyCollision && player.energy > player.minEnergy)
        {
            energy += 0.1f;
            Vector4 colour = GetComponent<Renderer>().material.color = new Vector4(0, 0, 1, 1);
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.transform.name.StartsWith("blackhole"))
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
