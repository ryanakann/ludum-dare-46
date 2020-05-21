using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {
    [Header("Transition")]
    public float transitionDuration = 1f;
    [Tooltip("Normalized to (0-1) on X and Y axes.")]
    public AnimationCurve tween;    

    [Header("Events")]
    public UnityEvent OnDoorOpen;
    public UnityEvent OnDoorClose;


    private Transform door;

    private bool open;
    private bool transitioning;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Awake () {
        door = transform.Find("Door_Key");
        open = false;
        transitioning = false;

        closedPosition = door.localPosition;
        openPosition = door.localPosition + Vector3.up * -2f * door.localPosition.y;
    }

    public void Open () {
        if (open || transitioning) return;
        transitioning = true;
        OnDoorOpen.Invoke();
        StartCoroutine(OpenCR());
    }

    private IEnumerator OpenCR () {
        float t = 0f;

        while (t < 1f) {
            // print($"t: {t} - tween: {tween.Evaluate(t)}");
            door.localPosition = Vector3.Lerp(closedPosition, openPosition, tween.Evaluate(t));
            t += Time.deltaTime / transitionDuration;
            yield return new WaitForEndOfFrame();
        }
        door.localPosition = openPosition;
        open = true;
        transitioning = false;
    }

    public void Close () {
        if (!open || transitioning) return;
        transitioning = true;
        OnDoorClose.Invoke();
        StartCoroutine(CloseCR());
    }

    private IEnumerator CloseCR () {
        float t = 0f;

        while (t < 1f) {
            door.localPosition = Vector3.Lerp(openPosition, closedPosition, tween.Evaluate(t));
            t += Time.deltaTime / transitionDuration;
            yield return new WaitForEndOfFrame();
        }
        door.localPosition = closedPosition;
        open = false;
        transitioning = false;
    }
}
