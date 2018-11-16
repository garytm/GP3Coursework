using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxVelocity = 3;
    public float maxSpeed = 0.1f;
    float energy;
    private Vector3 velocity;
    public GameObject target;
    Player player;
    private void Start()
    {
        velocity = Vector3.zero;
    }

    private void Update()
    {
        SeekPlayer();
    }
    void SeekPlayer()
    {
        var desiredVelocity = target.transform.position - transform.position;
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
            GetComponent<Renderer>().material.color = new Vector4(0, 0, 1, 1);
        }
    }
}
