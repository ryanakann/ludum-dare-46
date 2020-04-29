using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    [Header("Basic Entity Properties")]
    public float scale = 1f;
    private Vector3 baseScale;
    private Vector3 scaleRef;

    public float distanceTravelled;
    private Vector3 positionLF;

    public float timeAlive;
    
    public bool grounded;

    protected virtual void Awake () {
        distanceTravelled = 0f;
        positionLF = transform.position;

        timeAlive = 0f;

        baseScale = transform.localScale;
    }

    protected virtual void Update () {
        timeAlive += Time.deltaTime;
        distanceTravelled += (transform.position - positionLF).magnitude;
        positionLF = transform.position;

        //Scale proportionally to the base scale
        transform.localScale = Vector3.SmoothDamp(transform.localScale, baseScale * scale, ref scaleRef, 0.02f);        
    }

    protected virtual void FixedUpdate () {
        bool wasGrounded = grounded;
        grounded = false;

        if (GetComponent<Collider2D>()) {
            grounded = GetComponent<Collider2D>().IsTouchingLayers();
        }
    }
}
