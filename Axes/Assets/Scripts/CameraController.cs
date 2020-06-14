using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineConfiner))]
public class CameraController : MonoBehaviour {
    private CinemachineConfiner confiner;
    private CinemachineVirtualCamera cam;
    private new PolygonCollider2D collider;
    
    public float size = 7f;

    private void OnValidate () {
        Setup();
        
    }

    private void Start () {
        Setup();
    }

    private void Update () {
        cam.m_Lens.OrthographicSize = size;
    }

    private void Setup () {
        if (confiner == null) {
            confiner = GetComponent<CinemachineConfiner>();
        }

        if (cam == null) {
            cam = GetComponent<CinemachineVirtualCamera>();
        }
        cam.m_Lens.OrthographicSize = size;

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