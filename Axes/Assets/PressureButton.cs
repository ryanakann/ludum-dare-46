using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour {    
    [SerializeField]
    private bool pressed;
    [SerializeField]
    [Tooltip("0 = pressed when fully down. 1 = always pressed. Recommend (0.3-0.7).")]
    private float thresholdLerp = 0.7f;

    [Header("Events")]
    public UnityEvent OnButtonDown;
    public UnityEvent OnButtonUp;

    private Transform top;
    private float thresholdDistance;
    private float currentDistance;

    private void Awake () {
        top = transform.Find("Button_Top");
        pressed = false;
        currentDistance = top.localPosition.y;
        thresholdDistance = currentDistance * thresholdLerp;
    }

    private void Update () {
        currentDistance = top.localPosition.y;
        print($"Distance: {currentDistance} - Threshold: {thresholdDistance}");
        if (!pressed && currentDistance < thresholdDistance - Mathf.Epsilon) {
            pressed = true;
            OnButtonDown.Invoke();
        }  else if (pressed &&  currentDistance > thresholdDistance + Mathf.Epsilon) {
            pressed = false;
            OnButtonUp.Invoke();
        }
    }
}