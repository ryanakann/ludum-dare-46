using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [Header("Interpolation")]
    
    [SerializeField]
    private bool scaleSpeedWithDistance = false;
    [SerializeField]
    private float waypointSpeed = 1f;

    [SerializeField]
    private AnimationCurve tween;

    private List<Transform> waypoints = new List<Transform>();
    private int lastWaypoint = 0, currentWaypoint = 1;
    private float distanceThreshold = 0.01f;

    Transform carrier;
    Transform platform;
    Dictionary<Transform, Transform> carriedObjects = new Dictionary<Transform, Transform>();

    float t;

    void Awake () {
        carrier = transform;
        platform = transform.parent.FindDeepChild("PlatformObject");
        foreach (Transform t in transform.parent.FindDeepChild("Waypoints")) {
            waypoints.Add(t);
        }

        if (waypoints.Count < 2) {
            enabled = false;
        }

        waypoints[0].position = platform.position;
    }

    void Update () {
        carrier.position = platform.position;
        if (Vector2.Distance(platform.position, waypoints[currentWaypoint].position) > distanceThreshold) {
            t += Time.deltaTime * waypointSpeed * 
                (scaleSpeedWithDistance ? Vector2.Distance(waypoints[currentWaypoint].position, waypoints[(currentWaypoint + 1) % waypoints.Count].position) : 1);
            platform.position = Vector2.Lerp(waypoints[lastWaypoint].position, waypoints[currentWaypoint].position, tween.Evaluate(t));
        } else {
            t = 0f;
            lastWaypoint = currentWaypoint;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!carriedObjects.ContainsKey(collision.transform) && collision.GetComponent<Rigidbody2D>()) {
            carriedObjects[collision.transform] = (collision.transform.parent) ? collision.transform.parent : collision.transform;
            collision.transform.parent = carrier;
            // print($"Added {collision.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (carriedObjects.ContainsKey(collision.transform)) {
            if (collision.transform.parent == carrier) {
                collision.transform.parent = (carriedObjects[collision.transform] == collision.transform) ? null : carriedObjects[collision.transform];
            }
            carriedObjects.Remove(collision.transform);
            // print($"Removed {collision.name}");
        }
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.cyan;

        foreach (Transform t in transform.parent.FindDeepChild("Waypoints")) {
            Gizmos.DrawSphere(t.position, 0.25f);
        }
    }
}
