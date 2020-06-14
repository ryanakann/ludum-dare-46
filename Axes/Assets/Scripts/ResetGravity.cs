using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGravity : MonoBehaviour {
    private void Awake() {
        Physics2D.gravity = Vector2.down * 9.81f;
    }
}