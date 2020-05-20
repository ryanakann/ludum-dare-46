// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace Axes.PlayMaker.Actions
{
    public enum OverflowMode {
        Clamp,
        Repeat,
        PingPong,
    }

    [ActionCategory(ActionCategory.GameLogic)]
    [Tooltip("Tween a float variable using a custom easing function.")]
    public class Function : FsmStateAction
    {
        public FsmFloat input;
        public FsmVector2 inputBounds;
        public FsmAnimationCurve functionCurve;
        public OverflowMode overflowMode;
        public FsmFloat output;
        public FsmVector2 outputBounds;

        public override void OnUpdate () {
            float x = (input.Value - inputBounds.Value.x) / (inputBounds.Value.y - inputBounds.Value.x);
            
            float y = functionCurve.curve.Evaluate(x);
            float ret = y * (outputBounds.Value.y - outputBounds.Value.x) + outputBounds.Value.x;
            output.Value = ret;
        }
    }

}
