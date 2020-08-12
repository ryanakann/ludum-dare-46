using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoard : MonoBehaviour {
    SpringJoint2D springJoint;
    Transform top, mid, bot;

    public float springThreshold = 0.5f;
    public float springForce = 5f;
    public float launchCD = 0.25f;
    [SerializeField]
    private float curLaunchCD;

    float initialSpringLength;
    float currentSpringLength;
    Vector3 midsectionInitialScale;
    Vector3 springInitialPosition;

    Collider2D[] carriedObjects;
    BoxCollider2D launchArea;
    Rigidbody2D objReference;

    // Start is called before the first frame update
    void Awake() {
        top = transform.Find("Spring_Top");
        mid = transform.Find("Spring_Mid");
        bot = transform.Find("Spring_Bot");
        springJoint = top.GetComponent<SpringJoint2D>();
        initialSpringLength = (top.position-bot.position).magnitude;
        midsectionInitialScale = mid.localScale;
        springInitialPosition = top.position;
        launchArea = top.GetComponent<BoxCollider2D>();
        launchArea.enabled = false;
        curLaunchCD = 0f;
    }

    void Update() {
        //Scale the mid section springy bit based on how compressed the spring is
        currentSpringLength = (top.position-bot.position).magnitude;
        mid.localScale = new Vector3(midsectionInitialScale.x, 
                                     midsectionInitialScale.y * Mathf.InverseLerp(0, initialSpringLength, currentSpringLength), 
                                     midsectionInitialScale.z);

        if (Vector2.Dot(springJoint.reactionForce, top.position - springInitialPosition) > springThreshold) {
            if (curLaunchCD <= 0f) {
                print("KABOOM");
                curLaunchCD = launchCD;
                carriedObjects = Physics2D.OverlapBoxAll((Vector2)top.transform.position + launchArea.offset, launchArea.size, 0f);
                foreach (Collider2D obj in carriedObjects) {
                    if (null != (objReference = obj.GetComponent<Rigidbody2D>()) && !obj.name.Contains("Spring")) {
                        objReference.velocity = Vector2.zero;
                        objReference.AddForce(transform.up * springForce, ForceMode2D.Impulse);
                    }
                }
            }    
        } else {
            // print($"Force: {Vector2.Dot(springJoint.reactionForce, top.position - springInitialPosition)} - Threshold: {springThreshold}");
        }
        
        //Prevent double launches
        if (curLaunchCD < 0f) {
            curLaunchCD = 0f;
            springJoint.frequency = 1f;
        } else if (curLaunchCD > 0f) {
            curLaunchCD -= Time.deltaTime;  
            springJoint.frequency = 10f;
        } else {
            springJoint.frequency = 1f;
        }
    }

    void OnDrawGizmos () {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube((Vector2)top.transform.position + launchArea.offset, launchArea.size);
    }
}
