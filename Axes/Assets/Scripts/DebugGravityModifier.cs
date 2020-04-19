using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGravityModifier : MonoBehaviour
{
    public Transform node;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.gravity = (node.position - transform.position).normalized * 9.81f;
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(node.position, 0.1f);
    }
}
