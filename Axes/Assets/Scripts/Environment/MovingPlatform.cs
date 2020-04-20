using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    List<Transform> waypoints = new List<Transform>();
    int last_waypoint = 0, current_waypoint = 1;
    float distance_threshold = 0.1f, timer = 0, waypoint_time = 5f;

    Transform carrier;
    Dictionary<Transform, Transform> carried_objects = new Dictionary<Transform, Transform>();

    // Start is called before the first frame update
    void Awake()
    {

        carrier = transform.parent.FindDeepChild("Carrier");
        foreach (Transform t in transform.parent.FindDeepChild("Waypoints")) 
        {
            waypoints.Add(t);
        }

        if (waypoints.Count < 2)
            enabled = false;


        waypoints[0].position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        carrier.position = transform.position;
        if (Vector2.Distance(transform.position, waypoints[current_waypoint].position) > distance_threshold)
        {
            transform.position = Vector2.Lerp(waypoints[last_waypoint].position, waypoints[current_waypoint].position, timer/waypoint_time);
            timer += Time.deltaTime;
        }
        else 
        {
            timer = 0;
            last_waypoint = current_waypoint;
            current_waypoint = (current_waypoint + 1) % waypoints.Count;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!carried_objects.ContainsKey(collision.transform) && collision.GetComponent<Rigidbody2D>())
        {
            carried_objects[collision.transform] = (collision.transform.parent) ? collision.transform.parent : collision.transform;
            collision.transform.parent = carrier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (carried_objects.ContainsKey(collision.transform))
        {
            if (collision.transform.parent == carrier)
            {
                collision.transform.parent = (carried_objects[collision.transform] == collision.transform) ? null: carried_objects[collision.transform];
            }
            carried_objects.Remove(collision.transform);
        }
    }
}
