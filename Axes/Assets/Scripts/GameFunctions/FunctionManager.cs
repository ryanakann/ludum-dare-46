using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionManager : MonoBehaviour {
    public GameFunction[] functions;

    private void Update () {
        foreach (var function in functions) {
            function.HandleFunction();
        }
    }

    private void OnValidate () {
        foreach (var function in functions) {
            function.UpdateName();
        }
    }
}
