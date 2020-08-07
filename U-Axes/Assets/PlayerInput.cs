using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterController2D controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    private void FixedUpdate()
    {
        controller.Move(Input.GetAxis("Horizontal"), Input.GetKeyDown(KeyCode.Space), Input.GetKey(KeyCode.Space));
    }
}
