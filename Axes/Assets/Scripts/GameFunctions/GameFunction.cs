using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {
    //Independent
    XPosition,
    XLocalPosition,
    YPosition,
    YLocalPosition,
    XInput,
    YInput,
    Rotation,
    Scale,
    Distance,
    DistanceX,
    DistanceY,
    DistanceTravelled,
    AngleBetween,
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

[System.Serializable]
public class GameFunction {
    [HideInInspector]
    public string name = "Function";
    public Axis x;
    public Axis y;
    public GameObject xTarget;
    public GameObject yTarget; 
    public Vector2 xRange;
    public Vector2 yRange;
    public AnimationCurve function;

    private Vector3 refVector;
    private float refFloat;
    private Vector3 targetVector;
    private float targetFloat;

    private const float smoothTime = 0.02f;

    private Vector3 SmoothVector (Vector3 src, Vector3 dst) {
        return Vector3.SmoothDamp(src, dst, ref refVector, smoothTime);
    }

    private float SmoothFloat (float src, float dst) {
        return Mathf.SmoothDamp(src, dst, ref refFloat, smoothTime);
    }

    private Color SmoothColor(Color src, Color dst) {
        Vector3 rgb = SmoothVector(new Vector3(src.r, src.g, src.b), new Vector3(dst.r, dst.g, dst.b));
        float a = SmoothFloat(src.a, dst.a);
        Color c = new Color(rgb.x, rgb.y, rgb.z, a);
        return c;
    }

    private float GetVar (Axis x) {
        if (xTarget == null) return 0f;
        switch (x) {
            case Axis.XPosition:
                return xTarget.transform.position.x;
            case Axis.YPosition:
                return xTarget.transform.position.y;
            case Axis.XLocalPosition:
                return xTarget.transform.localPosition.x;
            case Axis.YLocalPosition:
                return xTarget.transform.localPosition.y;
            case Axis.XInput:
                return Input.GetAxis("Horizontal");
            case Axis.YInput:
                return Input.GetAxis("Vertical");
            case Axis.Rotation:
                return (xTarget.GetComponent<Cinemachine.CinemachineVirtualCamera>() ? xTarget.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.Dutch : xTarget.transform.eulerAngles.z);
            case Axis.Scale:
                return  (Mathf.Abs(xTarget.transform.localScale.x) + Mathf.Abs(xTarget.transform.localScale.y)) / 2f;
            case Axis.Distance:
                return (xTarget.transform.position - yTarget.transform.position).magnitude;
            case Axis.DistanceX:
                return (xTarget.transform.position - yTarget.transform.position).x;
            case Axis.DistanceY:
                return (xTarget.transform.position - yTarget.transform.position).y;
            case Axis.AngleBetween:
                float value = (float)((System.Math.Atan2((xTarget.transform.position - yTarget.transform.position).y, (xTarget.transform.position - yTarget.transform.position).x) / Mathf.PI) * 180f);
                if (value < 0) value += 360f;
                return value;
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
                //print("Speed: " + xTarget.GetComponent<Rigidbody2D>().velocity.magnitude);
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
                targetVector = new Vector3(val, yTarget.transform.position.y, yTarget.transform.position.z);
                yTarget.transform.position = SmoothVector(yTarget.transform.position, targetVector);
                break;
            case Axis.XLocalPosition:
                targetVector = new Vector3(val, yTarget.transform.localPosition.y, yTarget.transform.localPosition.z);
                yTarget.transform.localPosition = SmoothVector(yTarget.transform.localPosition, targetVector);
                break;
            case Axis.YPosition:
                targetVector = new Vector3(yTarget.transform.position.x, val, yTarget.transform.position.z);
                yTarget.transform.position = SmoothVector(yTarget.transform.position, targetVector);
                break;
            case Axis.YLocalPosition:
                targetVector = new Vector3(yTarget.transform.localPosition.x, val, yTarget.transform.localPosition.z);
                yTarget.transform.localPosition = SmoothVector(yTarget.transform.localPosition, targetVector);
                break;
            case Axis.XInput:
                Debug.Log("Error: PlayerXInput is not writeable");
                break;
            case Axis.YInput:
                Debug.Log("Error: PlayerXInput is not writeable");
                break;
            case Axis.Rotation:
                if (yTarget.GetComponent<Cinemachine.CinemachineVirtualCamera>()) {
                    if (val > 180f) {
                        val -= 360f;
                    }
                    yTarget.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.Dutch = val;
                } else {
                    targetVector = Vector3.forward * val;
                    yTarget.transform.eulerAngles = SmoothVector(yTarget.transform.eulerAngles, targetVector);
                }
                break;
            case Axis.Scale:
                if (yTarget.GetComponent<Cinemachine.CinemachineVirtualCamera>()) {
                    yTarget.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.OrthographicSize = val;
                } else {
                    Vector3 ratio = yTarget.transform.localScale.normalized;
                    float multiplier = 1 / Mathf.Min(Mathf.Abs(ratio.x), Mathf.Abs(ratio.y));
                    ratio *= multiplier;
                    yTarget.transform.localScale = SmoothVector(yTarget.transform.localScale, ratio * val);
                }
                break;
            case Axis.Distance:
                Debug.Log("Error: Distance is not writeable");
                break;
            case Axis.DistanceTravelled:
                Debug.Log("Error: DistanceTravelled is not writeable");
                break;
            case Axis.JumpCount:
                Debug.Log("Error: JumpCount is not writeable");
                break;
            case Axis.Grounded:
                Debug.Log("Error: Grounded is not writeable");

                break;
            case Axis.TimeElapsed:
                Debug.Log("Error: TimeElapsed is not writeable");
                break;
            case Axis.GameVolume:
                Debug.Log("Error: TimeElapsed not yet implemented");
                break;
            case Axis.GameQuality:
                Debug.Log("Error: GameQuality not yet implemented");

                break;
            case Axis.GameTimeElapsed:
                Debug.Log("Error: GameTimeElapsed is not writeable");
                break;
            case Axis.Mass:
                if (yTarget.GetComponent<Rigidbody2D>()) {
                    yTarget.GetComponent<Rigidbody2D>().mass = SmoothFloat(yTarget.GetComponent<Rigidbody2D>().mass, val);
                }
                break;
            case Axis.Friction:
                if (yTarget.GetComponent<Rigidbody2D>()) {
                    yTarget.GetComponent<Rigidbody2D>().sharedMaterial.friction = SmoothFloat(yTarget.GetComponent<Rigidbody2D>().sharedMaterial.friction, val);

                }
                break;
            case Axis.Bounciness:
                if (yTarget.GetComponent<Rigidbody2D>()) {
                    yTarget.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = SmoothFloat(yTarget.GetComponent<Rigidbody2D>().sharedMaterial.bounciness, val);

                }
                break;
            case Axis.Speed:
                if (yTarget.GetComponent<CharacterController2D>()) {
                    yTarget.GetComponent<CharacterController2D>().speed = SmoothFloat(yTarget.GetComponent<CharacterController2D>().speed, val);
                }
                break;
            case Axis.JumpHeight:
                if (yTarget.GetComponent<CharacterController2D>()) {
                    yTarget.GetComponent<CharacterController2D>().jumpForce = SmoothFloat(yTarget.GetComponent<CharacterController2D>().jumpForce, val);
                }
                break;
            case Axis.Visibility:
                if (yTarget.GetComponent<SpriteRenderer>()) {
                    Color c = yTarget.GetComponent<SpriteRenderer>().color;
                    c.a = val;
                    yTarget.GetComponent<SpriteRenderer>().color = SmoothColor(yTarget.GetComponent<SpriteRenderer>().color, c);

                }
                break;
            case Axis.InputDelay:
                Debug.Log("Error: PlayerInputDelay writing not yet implemented.");
                break;
            case Axis.AIReactionTime:
                Debug.Log("Error: AIReactionTime writing not yet implemented.");
                break;
            case Axis.GameTimeScale:
                Time.timeScale = val;
                break;
            case Axis.GameFrameRate:
                Debug.Log("Error: GameFrameRate is not writeable");
                break;
            case Axis.GameGravityRotation:
                float gravMag = Physics2D.gravity.magnitude;
                Debug.Log("Gravity: " + Physics2D.gravity);
                targetVector = Quaternion.Euler(0, 0, val) * Vector2.down * gravMag;
                //targetVector = new Vector2(Mathf.Cos(val * Mathf.Deg2Rad), Mathf.Sin(val * Mathf.Deg2Rad)) * gravMag;
                Physics2D.gravity = SmoothVector(Physics2D.gravity, targetVector);
                break;
            case Axis.GameGravityScale:
                targetVector = (Vector3)(Physics2D.gravity.normalized) * val;
                Physics2D.gravity = Physics2D.gravity.normalized * val;
                break;
            case Axis.GameMusicPitch:
                Debug.Log("Error: GameMusicPitch writing not yet implemented.");
                break;
            case Axis.GameVisibility:
                Debug.Log("Error: GameVisibility writing not yet implemented.");
                break;
        }
    }

    public void HandleFunction () {
        if (xTarget == null) return;
        float xi = GetVar(x);
        float xNorm = Mathf.InverseLerp(xRange.x, xRange.y, xi);
        float yNorm = function.Evaluate(xNorm);
        float yi = Mathf.Lerp(yRange.x, yRange.y, yNorm);
        SetVar(y, yi);
    }

    public void UpdateName () {
        string name1 = xTarget ? xTarget.name + " " : "";
        string name2 = yTarget ? yTarget.name + " " : "";

        name = "[" + name1.ToUpper() + x.ToString() + "] to [" + name2.ToUpper() + y.ToString() + "]";
    }
}
