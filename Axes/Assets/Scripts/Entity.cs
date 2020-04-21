using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    [Header("Basic Entity Properties")]
    public float distanceTravelled;
    private Vector3 positionLF;

    public float timeAlive;
    
    public bool grounded;

    protected virtual void Awake () {
        distanceTravelled = 0f;
        positionLF = transform.position;

        timeAlive = 0f;
    }

    protected virtual void Update () {
        timeAlive += Time.deltaTime;
        distanceTravelled += (transform.position - positionLF).magnitude;
        positionLF = transform.position;
    }

    protected virtual void FixedUpdate () {
        bool wasGrounded = grounded;
        grounded = false;

        if (GetComponent<Collider2D>()) {
            grounded = GetComponent<Collider2D>().IsTouchingLayers();
        }
    }
}
