using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxVelocity = 3;
    public float maxSpeed = 0.1f;
    private Vector3 velocity;
    public GameObject target;

    private void Start()
    {
        velocity = Vector3.zero;
    }

    private void Update()
    {
        var desiredVelocity = target.transform.position - transform.position;
        desiredVelocity = desiredVelocity.normalized * maxVelocity;

        var steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, maxSpeed);

        velocity = Vector3.ClampMagnitude(velocity + steering, maxVelocity);
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
    }
}
