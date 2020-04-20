using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float threshold = 100f;
    Collider2D coll;
    ParticleSystem debris;
    SpriteRenderer sr;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        debris = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Rigidbody2D>())
        {
            float force = Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity) * collision.collider.GetComponent<Rigidbody2D>().mass;
            if (force > threshold) {
                Break();
            }
        }
    }

    void Break()
    {
        coll.enabled = false;
        sr.enabled = false;
        debris.Play();
    }
}
