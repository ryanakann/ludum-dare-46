//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public class PlayerPlatformerController {
}
//    #region VARIABLES
//    public float maxSpeed = 7;
//    public float jumpTakeOffSpeed = 7;

//    private SpriteRenderer spriteRenderer;
//    private Animator animator;

//    [Header("Input Buffering")]
//    public int bufferFrames = 5;
//    private Dictionary<PlayerState, int> stateBuffer; //0 means not buffered.
//    private Dictionary<PlayerAction, int> actionBuffer; // 0 means not buffered (no input).
//    #endregion

//    // Use this for initialization
//    void Awake () {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        animator = GetComponent<Animator>();
//        InitBuffers();
//    }

//    public void ComputeVelocity (float moveX, bool jumpDown, bool jumpUp) {
//        Vector2 move = Vector2.right * moveX; // y not currently used

//        //Reset action buffers
//        if (jumpDown) {
//            RenewBuffer(PlayerAction.Jump);
//        }

//        if (grounded) RenewBuffer(PlayerState.Grounded);

//        if (CheckBuffer(PlayerAction.Jump) && CheckBuffer(PlayerState.Grounded)) {
//            velocity.y = jumpTakeOffSpeed;
//            animator.SetTrigger("jump");
//            ResetBuffer(PlayerAction.Jump);
//            ResetBuffer(PlayerState.Grounded);
//        } else if (jumpUp) {
//            if (velocity.y > 0) {
//                velocity.y = velocity.y * 0.5f;
//            }
//        }

//        if (move.x < -0.01f) {
//            spriteRenderer.flipX = true;
//        } else if (move.x > 0.01f) {
//            spriteRenderer.flipX = false;
//        }

//        animator.SetBool("grounded", grounded);
//        animator.SetFloat("moveX", Mathf.Abs(velocity.x) / maxSpeed);

//        targetVelocity = move * maxSpeed;

//        DecrementBuffers();
//    }

//    #region BUFFERS
//    private void InitBuffers () {
//        stateBuffer = new Dictionary<PlayerState, int>();
//        for (int i = 0; i < sizeof(PlayerState); i++) {
//            stateBuffer.Add((PlayerState)i, 0);
//        }

//        actionBuffer = new Dictionary<PlayerAction, int>();
//        for (int i = 0; i < sizeof(PlayerAction); i++) {
//            actionBuffer.Add((PlayerAction)i, 0);
//        }
//    }

//    public void DecrementBuffers () {
//        for (int i = 0; i < sizeof(PlayerState); i++) {
//            PlayerState action = (PlayerState)i;
//            if (stateBuffer.ContainsKey(action) && stateBuffer[action] > 0) {
//                stateBuffer[action]--;
//            }
//        }
//    }
    
//    public void ResetBuffer (PlayerState key) {
//        if (stateBuffer.ContainsKey(key)) {
//            stateBuffer[key] = 0;
//        }
//    }

//    public void RenewBuffer (PlayerState key) {
//        if (stateBuffer.ContainsKey(key)) {
//            stateBuffer[key] = bufferFrames;
//        }
//    }

//    public bool CheckBuffer (PlayerState key) {
//        if (stateBuffer.ContainsKey(key)) {
//            return stateBuffer[key] > 0;
//        }
//        return false;
//    }
//    public void ResetBuffer (PlayerAction key) {
//        if (actionBuffer.ContainsKey(key)) {
//            actionBuffer[key] = 0;
//        }
//    }

//    public void RenewBuffer (PlayerAction key) {
//        if (actionBuffer.ContainsKey(key)) {
//            actionBuffer[key] = bufferFrames;
//        }
//    }

//    public bool CheckBuffer (PlayerAction key) {
//        if (actionBuffer.ContainsKey(key)) {
//            return actionBuffer[key] > 0;
//        }
//        return false;
//    }
//    #endregion
//}

//#region ENUMS
//public enum PlayerState {
//    // Static states
//    Grounded,
//    Swimming,
//    Stunned,
//}

//public enum PlayerAction {
//    Jump,
//    Attack,
//    Menu,
//}
//#endregion