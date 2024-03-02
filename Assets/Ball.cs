using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Adjust these values in the inspector to fine-tune the hit strength and behavior
    public float hitStrengthMultiplier = 1.0f;
    public float maxHitStrength = 50.0f;

    private Rigidbody ballRigidbody;

    void Start()
    {
        // Ensure there's a Rigidbody attached to the ball
        ballRigidbody = GetComponent<Rigidbody>();
        if (!ballRigidbody)
        {
            Debug.LogError("Rigidbody is missing on the golf ball!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is the golf club
        if (collision.gameObject.tag == "GolfClub")
        {
            // Calculate the hit strength based on the club's speed and angle
            float hitStrength = CalculateHitStrength(collision);

            // Apply a force to the ball
            Vector3 forceDirection = collision.contacts[0].normal;
            ballRigidbody.AddForce(-forceDirection * hitStrength, ForceMode.Impulse);
        }
    }

    private float CalculateHitStrength(Collision collision)
    {
        // Here you can incorporate the club's speed and angle
        // For simplicity, I'm using the relative velocity of the collision
        float speed = collision.relativeVelocity.magnitude;

        // Apply a multiplier for more control over the physics
        float strength = speed * hitStrengthMultiplier;

        // Optionally cap the maximum strength of the hit
        return Mathf.Min(strength, maxHitStrength);
    }
}
