using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {
    //Independent
    XPosition,
    YPosition,
    XInput,
    YInput,
    Rotation,
    Scale,
    Distance,
    DistanceTravelled,
    JumpCount,
    Grounded,

    TimeElapsed,

    GameVolume,
    GameQuality,
    GameTimeElapsed,


    //Dependent
    Mass,
    Friction,
    Bounciness,
    Speed,
    JumpHeight,
    Visibility,
    InputDelay,

    AIReactionTime,

    CameraRotation,
    CameraDeepFryValue,
    CameraZoom,

    GameTimeScale,
    GameFrameRate,
    GameGravityRotation, //0-360. Counter clockwise. 0 = top
    GameGravityScale,
    GameMusicPitch,
    GameVisibility,
}

public class FunctionManager : MonoBehaviour {
    public Axis x;
    public Axis y;
    public GameObject xTarget;
    public GameObject yTarget;
    public Vector2 xRange;
    public Vector2 yRange;
    public AnimationCurve function;

    private void Start () {
        if (xTarget == null) {
            xTarget = GameObject.FindWithTag("Player");
        }
        if (yTarget == null) {
            yTarget = GameObject.FindWithTag("Player");
        }
    }

    private void LateUpdate () {
        HandleFunction(x, y);
    }

    private float GetVar (Axis x) {
        switch (x) {
            case Axis.XPosition:
                return xTarget.transform.position.x;
            case Axis.YPosition:
                return xTarget.transform.position.y;
            case Axis.XInput:
                return Input.GetAxis("Horizontal");
            case Axis.YInput:
                return Input.GetAxis("Vertical");
            case Axis.Rotation:
                return xTarget.transform.eulerAngles.z;
            case Axis.Scale:
                return  (xTarget.transform.localScale.x + xTarget.transform.localScale.y) / 2f;
            case Axis.Distance:
                return (xTarget.transform.position - yTarget.transform.position).magnitude;
            case Axis.DistanceTravelled:
                return (xTarget.GetComponent<Entity>() ? xTarget.GetComponent<Entity>().distanceTravelled : 0f);
            case Axis.JumpCount:
                return (xTarget.GetComponent<CharacterController2D>() ? xTarget.GetComponent<CharacterController2D>().jumpCount : 0);
            case Axis.Grounded:
                return (xTarget.GetComponent<Entity>() ? (xTarget.GetComponent<Entity>().grounded ? 1f : 0f) : 0f);
            case Axis.TimeElapsed:
                return (xTarget.GetComponent<Entity>() ? xTarget.GetComponent<Entity>().timeAlive : Time.time);
            case Axis.GameVolume:
                return 1f;
            case Axis.GameQuality:
                return 1f;
            case Axis.GameTimeElapsed:
                return Time.time;
            case Axis.Mass:
                return (xTarget.GetComponent<Rigidbody2D>() ? xTarget.GetComponent<Rigidbody2D>().mass : 1f);
            case Axis.Friction:
                return (xTarget.GetComponent<Rigidbody2D>() ? xTarget.GetComponent<Rigidbody2D>().sharedMaterial.friction : 0.2f);
            case Axis.Bounciness:
                return (xTarget.GetComponent<Rigidbody2D>() ? xTarget.GetComponent<Rigidbody2D>().sharedMaterial.bounciness : 0f);
            case Axis.Speed:
                return (xTarget.GetComponent<Rigidbody2D>() ? xTarget.GetComponent<Rigidbody2D>().velocity.magnitude : 0f);
            case Axis.JumpHeight:
                return (xTarget.GetComponent<CharacterController2D>() ? xTarget.GetComponent<CharacterController2D>().jumpForce : 1f);
            case Axis.Visibility:
                return (xTarget.GetComponent<SpriteRenderer>() ? xTarget.GetComponent<SpriteRenderer>().color.a : 1f);
            case Axis.InputDelay:
                return 0f;
            case Axis.AIReactionTime:
                return 0.2f;
            case Axis.GameTimeScale:
                return Time.timeScale;
            case Axis.GameFrameRate:
                return 1f / Time.deltaTime;
            case Axis.GameGravityRotation:
                return Mathf.Atan2(Physics2D.gravity.y, Physics2D.gravity.x);
            case Axis.GameGravityScale:
                return Physics.gravity.magnitude;
            case Axis.GameMusicPitch:
                return 1f;
            case Axis.GameVisibility:
                return 1f;
            default:
                return 0f;
        }
    }

    private void SetVar (Axis y, float val) {
        switch (y) {
            case Axis.XPosition:
                xTarget.transform.position = new Vector3(val, xTarget.transform.position.y, xTarget.transform.position.z);
                break;
            case Axis.YPosition:
                xTarget.transform.position = new Vector3(xTarget.transform.position.x, val, xTarget.transform.position.z);
                break;
            case Axis.XInput:
                print("Error: PlayerXInput is not writeable");
                break;
            case Axis.YInput:
                print("Error: PlayerXInput is not writeable");
                break;
            case Axis.Rotation:
                xTarget.transform.eulerAngles = Vector3.forward * val;
                break;
            case Axis.Scale:
                print("Error: Scale is not writeable");
                break;
            case Axis.Distance:
                print("Error: Distance is not writeable");
                break;
            case Axis.DistanceTravelled:
                print("Error: DistanceTravelled is not writeable");
                break;
            case Axis.JumpCount:
                print("Error: JumpCount is not writeable");
                break;
            case Axis.Grounded:
                print("Error: Grounded is not writeable");

                break;
            case Axis.TimeElapsed:
                print("Error: TimeElapsed is not writeable");
                break;
            case Axis.GameVolume:
                print("Error: TimeElapsed not yet implemented");
                break;
            case Axis.GameQuality:
                print("Error: GameQuality not yet implemented");

                break;
            case Axis.GameTimeElapsed:
                print("Error: GameTimeElapsed is not writeable");
                break;
            case Axis.Mass:
                if (xTarget.GetComponent<Rigidbody2D>()) {
                    xTarget.GetComponent<Rigidbody2D>().mass = val;
                }
                break;
            case Axis.Friction:
                if (xTarget.GetComponent<Rigidbody2D>()) {
                    xTarget.GetComponent<Rigidbody2D>().sharedMaterial.friction = val;
                }
                break;
            case Axis.Bounciness:
                if (xTarget.GetComponent<Rigidbody2D>()) {
                    xTarget.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = val;
                }
                break;
            case Axis.Speed:
                if (xTarget.GetComponent<CharacterController2D>()) {
                    xTarget.GetComponent<CharacterController2D>().speed = val;
                }
                break;
            case Axis.JumpHeight:
                if (xTarget.GetComponent<CharacterController2D>()) {
                    xTarget.GetComponent<CharacterController2D>().jumpForce = val;
                }
                break;
            case Axis.Visibility:
                if (xTarget.GetComponent<SpriteRenderer>()) {
                    Color c = xTarget.GetComponent<SpriteRenderer>().color;
                    c.a = val;
                    xTarget.GetComponent<SpriteRenderer>().color = c;
                }
                break;
            case Axis.InputDelay:
                print("Error: PlayerInputDelay writing not yet implemented.");
                break;
            case Axis.AIReactionTime:
                print("Error: AIReactionTime writing not yet implemented.");
                break;
            case Axis.GameTimeScale:
                Time.timeScale = val;
                break;
            case Axis.GameFrameRate:
                print("Error: GameFrameRate is not writeable");
                break;
            case Axis.GameGravityRotation:
                float gravMag = Physics2D.gravity.magnitude;
                Physics2D.gravity = new Vector2(Mathf.Cos(val * Mathf.Deg2Rad), Mathf.Sin(val * Mathf.Deg2Rad)) * gravMag;
                break;
            case Axis.GameGravityScale:
                Physics2D.gravity = Physics2D.gravity.normalized * val;
                break;
            case Axis.GameMusicPitch:
                print("Error: GameMusicPitch writing not yet implemented.");
                break;
            case Axis.GameVisibility:
                print("Error: GameVisibility writing not yet implemented.");
                break;
        }
    }

    private void HandleFunction (Axis x, Axis y) {
        float xi = GetVar(x);
        float xNorm = Mathf.InverseLerp(xRange.x, xRange.y, xi);
        float yNorm = function.Evaluate(xNorm);
        float yi = Mathf.Lerp(yRange.x, yRange.y, yNorm);
        SetVar(y, yi);
    }
}
