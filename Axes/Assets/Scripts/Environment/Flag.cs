﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    private bool triggered = false;

    public SceneTransition sceneTransition;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!triggered && collision.CompareTag("Player")) {
            triggered = true;
            collision.GetComponent<CharacterController2D>().enabled = false;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            collision.GetComponent<Animator>().SetTrigger("jump");
            GetComponent<AudioSource>().Play();
            StartCoroutine(TwistAndShout(collision.GetComponent<SpriteRenderer>().material, collision.transform));
        }
    }

    IEnumerator TwistAndShout(Material mat, Transform trans) {
        float maxDuration = 1f;
        float t = 0f;
        Vector3 startScale = trans.localScale;

        Vector3 refPos = Vector3.zero;

        while (t < maxDuration) {
            trans.GetComponent<Rigidbody2D>().gravityScale = 0f;
            trans.localScale = Vector3.Lerp(startScale, new Vector3(1f, 1f, 0f) * Mathf.Epsilon + Vector3.forward, t / maxDuration);
            mat.SetFloat("_FadeAmount", Mathf.Lerp(-0.1f, 0.75f, t / maxDuration));
            mat.SetFloat("_TwistUvAmount", Mathf.Lerp(0f, Mathf.PI / 2f, t / maxDuration));

            trans.position = Vector3.SmoothDamp(trans.position, transform.position, ref refPos, 1f);

            yield return new WaitForEndOfFrame();

            t += Time.deltaTime;
        }
        mat.SetFloat("_FadeAmount", 1f);
        SceneLoader.LoadNextScene(sceneTransition);
    }
}
