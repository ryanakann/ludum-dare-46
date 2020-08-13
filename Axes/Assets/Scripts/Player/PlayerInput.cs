using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Buffers;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerInput : MonoBehaviour {
    private CharacterController2D controller;

    [HideInInspector] public BufferedButton jump;

    private void Start () {
        jump = new BufferedButton("Jump");
        controller = GetComponent<CharacterController2D>();
    }

    private void Update () {
        jump.UpdateButton();

        controller.Move(Input.GetAxis("Horizontal"), jump.Down(), jump.Stay());
    }
}