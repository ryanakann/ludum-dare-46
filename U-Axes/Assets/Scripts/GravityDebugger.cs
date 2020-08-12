using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDebugger : MonoBehaviour
{
    [SerializeField]
    private bool active = false;
    [SerializeField]
    private Transform handle;

    Vector3 gravity;

    private void Update()
    {
        if (!active) return;
        gravity = (handle.position - transform.position) * 9.81f;
        Physics2D.gravity = gravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(handle.position, 0.1f);
    }
}
