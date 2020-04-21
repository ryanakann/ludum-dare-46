using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<CharacterController2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            SceneLoader.Instance.RestartScene();
        }
    }
}
