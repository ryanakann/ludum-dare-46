using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxAutoOffset : MonoBehaviour {
    public float cyclesPerSecond;
    public Vector2 direction;
    private Material mat;

    [HideInInspector]
    public Vector2 offset;


    private void Awake () {
        mat = GetComponent<MeshRenderer>().material;
        direction.Normalize();
    }

    private void Update () {
        offset = direction * Time.time * cyclesPerSecond;
    }
}