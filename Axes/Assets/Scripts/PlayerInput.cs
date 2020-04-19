using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerInput : MonoBehaviour {
    private CharacterController2D controller;

    private BufferedButton jump;
    private BufferedButton crouch;

    private void Start () {
        jump = new BufferedButton("Jump");
        crouch = new BufferedButton("Crouch");

        controller = GetComponent<CharacterController2D>();
    }

    private void Update () {
        jump.UpdateButton();
        crouch.UpdateButton();

        controller.Move(Input.GetAxis("Horizontal"), crouch.Down(), jump.Down(), jump.Stay());
    }
}

public class BufferedButton {
    private string buttonName;
    private int bufferSize;
    private int bufferDown;
    private int bufferStay;
    private int bufferUp;

    public BufferedButton (string buttonName, int bufferSize = 4) {
        this.buttonName = buttonName;
        this.bufferSize = bufferSize;
    }

    public void UpdateButton () {
        if (Input.GetButtonDown(buttonName)) {
            bufferDown = bufferSize;
        } else {
            if (bufferDown > 0) {
                bufferDown--;
            }
        }

        if (Input.GetButton(buttonName)) {
            bufferStay = bufferSize;
        } else {
            if (bufferStay > 0) {
                bufferStay--;
            }
        }

        if (Input.GetButtonUp(buttonName)) {
            bufferUp = bufferSize;
        } else {
            if (bufferUp > 0) {
                bufferUp--;
            }
        }
    }

    public bool Down () {
        return bufferDown > 0;
    }

    public bool Stay () {
        return bufferStay > 0;
    }

    public bool Up () {
        return bufferUp > 0;
    }
}