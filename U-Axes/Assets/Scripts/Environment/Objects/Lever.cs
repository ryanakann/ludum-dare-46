using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour {
    [SerializeField]
    private AnimationCurve tween;
    [SerializeField]
    private float transitionDuration = 0.5f;

    [SerializeField]
    private Direction initialDirection;
    private Direction currentDirection;

    private Vector3 leftRotation = new Vector3(0f, 0f, 90f);
    private Vector3 rightRotation = new Vector3(0f, 0f, 0f);

    public UnityEvent OnLeverLeft;
    public UnityEvent OnLeverRight;

    private Transform pivot;

    private bool transitioning;

    private void Awake () {
        pivot = transform.FindDeepChild("Lever_Pivot");

        currentDirection = initialDirection;
        if (currentDirection == Direction.Left) {
            pivot.eulerAngles = Vector3.forward * 90f;
        }

        transitioning = false;
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.SqrMagnitude(FindObjectOfType<CharacterController2D>().transform.position-transform.position) < 1f) {
            if (FacingRight()) {
                currentDirection = Direction.Left;        
                OnLeverLeft.Invoke();
            } else {
                currentDirection = Direction.Right;
                OnLeverRight.Invoke();
            }
            if (!transitioning) {
                transitioning = true;
                StartCoroutine(TransitionCR());
            }
        }
    }

    private IEnumerator TransitionCR () {
        float t = FacingRight() ? 0f : 1f;
        bool finished = false;
        while (!finished) {
            pivot.eulerAngles = Vector3.Lerp(leftRotation, rightRotation, tween.Evaluate(t));
            t += Time.deltaTime / transitionDuration * (FacingRight() ? 1 : -1);
            finished = (FacingRight() ? t >= 1f : t <= 0f);
            yield return new WaitForEndOfFrame();
        }
        pivot.eulerAngles = FacingRight() ? rightRotation : leftRotation;
        transitioning = false;
    }

    private bool FacingRight () {
        return currentDirection == Direction.Right;
    }

    public enum Direction {
        Left,
        Right,
    }

}