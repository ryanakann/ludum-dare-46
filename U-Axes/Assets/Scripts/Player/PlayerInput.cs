using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Buffers;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerInput : MonoBehaviour {
    private CharacterController2D controller;

    [HideInInspector] public BufferedButton jump;
    [HideInInspector] public BufferedButton crouch;

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