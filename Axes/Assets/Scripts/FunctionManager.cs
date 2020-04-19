using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {
    //Independent
    PlayerXPosition,
    PlayerYPosition,
    PlayerXInput,
    PlayerYInput,
    PlayerRotation,
    PlayerAvgScale,
    PlayerDistanceToTarget,
    PlayerDistanceTravelled,
    PlayerJumpCount,
    PlayerGrounded,

    LevelTimeElapsed,

    GameVolume,
    GameQuality,
    GameTimeElapsed,


    //Dependent
    PlayerScale,
    PlayerMass,
    PlayerFriction,
    PlayerBounciness,
    PlayerSpeed,
    PlayerJumpHeight,
    PlayerVisibility,
    PlayerInputDelay,

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
    public GameObject player;
    public GameObject target;
    public Vector2 xRange;
    public Vector2 yRange;
    public AnimationCurve function;

    private void Start () {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void LateUpdate () {
        HandleFunction(x, y);
    }

    private float GetVar (Axis x) {
        switch (x) {
            case Axis.PlayerXPosition:
                return player.transform.position.x;
            case Axis.PlayerYPosition:
                return player.transform.position.y;
            case Axis.PlayerXInput:
                return Input.GetAxis("Horizontal");
            case Axis.PlayerYInput:
                return Input.GetAxis("Vertical");
            case Axis.PlayerRotation:
                return player.transform.eulerAngles.z;
            case Axis.PlayerAvgScale:
                return (player.transform.localScale.x + player.transform.localScale.y) / 2f;
            case Axis.PlayerDistanceToTarget:
                return Vector3.Distance(player.transform.position, target.transform.position);
            case Axis.PlayerDistanceTravelled:
                //TODO
                break;
            case Axis.PlayerJumpCount:
                //TODO
                break;
            case Axis.PlayerGrounded:
                break;
            case Axis.LevelTimeElapsed:
                break;
            case Axis.GameVolume:
                break;
            case Axis.GameQuality:
                break;
            case Axis.GameTimeElapsed:
                return Time.time;
            case Axis.PlayerScale:
                return (player.transform.localScale.x + player.transform.localScale.y) / 2f;
            case Axis.PlayerMass:
                return (player.GetComponent<Rigidbody2D>() != null ? player.GetComponent<Rigidbody2D>().mass : 1f);
            case Axis.PlayerFriction:
                return (player.GetComponent<Rigidbody2D>() != null ? player.GetComponent<Rigidbody2D>().sharedMaterial.friction : 0.2f);
            case Axis.PlayerBounciness:
                return (player.GetComponent<Rigidbody2D>() != null ? player.GetComponent<Rigidbody2D>().sharedMaterial.bounciness : 0f);
            case Axis.PlayerSpeed:
                return (player.GetComponent<Rigidbody2D>() != null ? player.GetComponent<Rigidbody2D>().velocity.magnitude : 1f);
            case Axis.PlayerJumpHeight:
                //TODO
                print("Error: PlayerJumpHeight reading not yet implemented.");
                break;
            case Axis.PlayerVisibility:
                return (player.GetComponent<SpriteRenderer>() != null ? player.GetComponent<SpriteRenderer>().color.a : 1f);
            case Axis.PlayerInputDelay:
                //TODO
                print("Error: PlayerInputDelay reading not yet implemented.");
                break;
            case Axis.AIReactionTime:
                //TODO
                print("Error: AIReactionTime reading not yet implemented.");
                break;
            case Axis.CameraRotation:
                return Camera.main.transform.rotation.z;
            case Axis.CameraDeepFryValue:
                //TODO
                break;
            case Axis.CameraZoom:
                return Camera.main.orthographicSize;
            case Axis.GameTimeScale:
                return Time.timeScale;
            case Axis.GameFrameRate:
                //TODO
                print("Error: GameFrameRate reading not yet implemented.");
                break;
            case Axis.GameGravityRotation:
                Vector2 gravVec = Physics2D.gravity;
                return Mathf.Atan2(gravVec.y, gravVec.x);
            case Axis.GameGravityScale:
                return Physics2D.gravity.magnitude;
            case Axis.GameMusicPitch:
                //TODO
                print("Error: GameMusicPitch reading not yet implemented.");
                break;
            case Axis.GameVisibility:
                //TODO
                print("Error: GameVisiblilty reading not yet implemented.");
                break;
        }
        return 1f;
    }

    private void SetVar (Axis y, float val) {
        switch (y) {
            case Axis.PlayerXPosition:
                player.transform.position = new Vector3(val, player.transform.position.y, player.transform.position.z);
                break;
            case Axis.PlayerYPosition:
                player.transform.position = new Vector3(player.transform.position.x, val, player.transform.position.z);
                break;
            case Axis.PlayerXInput:
                print("Error: PlayerXInput is not writeable");
                break;
            case Axis.PlayerYInput:
                print("Error: PlayerXInput is not writeable");
                break;
            case Axis.PlayerRotation:
                player.transform.eulerAngles = Vector3.forward * val;
                break;
            case Axis.PlayerAvgScale:
                print("Error: PlayerAvgScale is not writeable");
                break;
            case Axis.PlayerDistanceToTarget:
                print("Error: PlayerDistanceToTarget is not writeable");
                break;
            case Axis.PlayerDistanceTravelled:
                print("Error: PlayerDistanceTravelled is not writeable");
                break;
            case Axis.PlayerJumpCount:
                print("Error: PlayerJumpCount is not writeable");
                break;
            case Axis.PlayerGrounded:
                print("Error: PlayerGrounded is not writeable");

                break;
            case Axis.LevelTimeElapsed:
                print("Error: LevelTimeElapsed is not writeable");
                break;
            case Axis.GameVolume:
                break;
            case Axis.GameQuality:
                break;
            case Axis.GameTimeElapsed:
                print("Error: LevelTimeElapsed is not writeable");
                break;
            case Axis.PlayerScale:
                player.transform.localScale = Vector3.one * val;
                break;
            case Axis.PlayerMass:
                if (player.GetComponent<Rigidbody2D>()) {
                    player.GetComponent<Rigidbody2D>().mass = val;
                }
                break;
            case Axis.PlayerFriction:
                if (player.GetComponent<Rigidbody2D>()) {
                    player.GetComponent<Rigidbody2D>().sharedMaterial.friction = val;
                }
                break;
            case Axis.PlayerBounciness:
                if (player.GetComponent<Rigidbody2D>()) {
                    player.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = val;
                }
                break;
            case Axis.PlayerSpeed:
                print("Error: PlayerSpeed writing not yet implemented.");
                break;
            case Axis.PlayerJumpHeight:
                print("Error: PlayerJumpHeight writing not yet implemented.");
                break;
            case Axis.PlayerVisibility:
                if (player.GetComponent<SpriteRenderer>()) {
                    Color c = player.GetComponent<SpriteRenderer>().color;
                    c.a = val;
                    player.GetComponent<SpriteRenderer>().color = c;
                }
                break;
            case Axis.PlayerInputDelay:
                print("Error: PlayerInputDelay writing not yet implemented.");
                break;
            case Axis.AIReactionTime:
                print("Error: AIReactionTime writing not yet implemented.");
                break;
            case Axis.CameraRotation:
                Camera.main.transform.eulerAngles = Vector3.forward * val;
                break;
            case Axis.CameraDeepFryValue:
                print("Error: CameraDeepFryValue writing not yet implemented.");
                break;
            case Axis.CameraZoom:
                Camera.main.orthographicSize = val;
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
