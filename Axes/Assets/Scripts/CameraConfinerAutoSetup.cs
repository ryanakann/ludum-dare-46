using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineConfiner))]
public class CameraConfinerAutoSetup : MonoBehaviour {
    private CinemachineConfiner confiner;
    private new PolygonCollider2D collider;
    private void OnValidate () {
        Setup();
    }

    private void Start () {
        Setup();
    }

    private void Setup () {
        if (confiner == null) {
            confiner = GetComponent<CinemachineConfiner>();
        }

        if (collider == null) {
            foreach (var col in FindObjectsOfType<PolygonCollider2D>()) {
                if (col.transform.name == "CameraBounds") {
                    collider = col;
                    confiner.m_BoundingShape2D = collider;
                    break;
                }
            }
        }
    }
}