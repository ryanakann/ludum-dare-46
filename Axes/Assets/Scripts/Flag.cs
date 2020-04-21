using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    public SceneTransition sceneTransition;
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.CompareTag("Player")) {
            SceneLoader.LoadNextScene(sceneTransition);
        }
    }
}
