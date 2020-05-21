using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {
    [Header("Transition")]
    public float transitionDuration = 1f;
    [Tooltip("Normalize to (0-1) on X and Y axes.")]
    public AnimationCurve tween;    

    [Header("Events")]
    public int lockCount = 1;
    private int currentUnlockCount = 0;
    public LockMode lockMode;
    public UnityEvent OnDoorOpen;
    public UnityEvent OnDoorClose;

    private Transform door;

    private bool open;
    private bool transitioning;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Awake () {
        // OpenRequirements.
        door = transform.Find("Door_Key");
        open = false;
        transitioning = false;

        closedPosition = door.localPosition;
        openPosition = door.localPosition + Vector3.up * -2f * door.localPosition.y;

        currentUnlockCount = 0;
    }

    private void Update () {
        if (transitioning) return;
        if (open && IsLocked()) Close();
    }

    public bool IsOpen () {
        return open;
    }

    private bool IsLocked () {
        bool unlocked;
        switch(lockMode) {
            case LockMode.and:
                unlocked = currentUnlockCount == lockCount;
                break;
            case LockMode.nand:
                unlocked = currentUnlockCount < lockCount;
                break;
            case LockMode.nor:
                unlocked = currentUnlockCount == 0;
                break;
            case LockMode.or:
                unlocked = currentUnlockCount > 0;
                break;
            default:
                unlocked = false;
            break;
        }
        return !unlocked;
    }

    public void Open () {
        if (open) return;
        currentUnlockCount++;
        if (IsLocked()) return;
        open = true;
        if (transitioning) return;
        transitioning = true;
        OnDoorOpen.Invoke();
        StartCoroutine(TransitionCR());
    }

    public void Close () {
        if (!open) return;
        currentUnlockCount--;
        if (!IsLocked()) return;
        open = false;
        if (transitioning) return;
        transitioning = true;
        OnDoorClose.Invoke();
        StartCoroutine(TransitionCR());
    }

    private IEnumerator TransitionCR () {
        float t = open ? 0f : 1f;
        bool finished = false;
        while (!finished) {
            door.localPosition = Vector3.Lerp(closedPosition, openPosition, tween.Evaluate(t));
            t += Time.deltaTime / transitionDuration * (open ? 1 : -1);
            finished = (open ? t >= 1f : t <= 0f);
            yield return new WaitForEndOfFrame();
        }
        door.localPosition = open ? openPosition : closedPosition;
        transitioning = false;
    }

    public enum LockMode {
        and,
        or,
        nand,
        nor,
    }
}
