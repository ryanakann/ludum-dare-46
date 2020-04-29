using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameFunctionModuleMode {
    Float,
    Vector,
}
public class GameFunctionModule : MonoBehaviour {
    public Axis axis;
    public float x;
    
    [Tooltip("The GameObject to get/set a variable from. Leave blank for 'Game'")]
    public GameObject target;
    public GameFunctionModuleMode mode;
}