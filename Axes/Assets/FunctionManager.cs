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
    PlayerDistanceToObject,
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
    public GameObject target;
    public Vector2 xRange;
    public Vector2 yRange;
    public AnimationCurve function;
}
