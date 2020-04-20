using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoard : MonoBehaviour
{

    SpringJoint2D spring_joint;
    LineRenderer lr;
    Transform base_board, base_point, rope_point;

    float spring_threshold = 0.5f, spring_force = 5f;

    List<Rigidbody2D> carried_objects = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Awake()
    {
        spring_joint = GetComponent<SpringJoint2D>();
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        base_board = transform.parent.FindDeepChild("Base");
        rope_point = transform.FindDeepChild("RopePoint");
        base_point = transform.parent.FindDeepChild("BasePoint");
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPositions(new Vector3[] { rope_point.position, base_point.position});

        if (Vector2.Dot(spring_joint.reactionForce, transform.position - base_board.position) > spring_threshold)
        {
            foreach (Rigidbody2D r in carried_objects)
            {
                print(r);
                r.AddForce(transform.up * spring_force, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D coll_rb = collision.GetComponent<Rigidbody2D>();
        if (coll_rb && !carried_objects.Contains(coll_rb)) 
        {
            carried_objects.Add(coll_rb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D coll_rb = collision.GetComponent<Rigidbody2D>();
        if (coll_rb && carried_objects.Contains(coll_rb))
        {
            carried_objects.Remove(coll_rb);
        }
    }
}
