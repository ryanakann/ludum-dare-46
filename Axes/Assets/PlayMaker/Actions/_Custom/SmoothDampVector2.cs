// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace Axes.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameLogic)]
    [Tooltip("Tween a float variable using a custom easing function.")]
    public class SmoothDampVector2 : FsmStateAction
    {
        public FsmVector2 source;
        public FsmVector2 target;
        private UnityEngine.Vector2 velocity;
        public FsmFloat smoothTime;
        public FsmFloat maxSpeed;


        public override void OnUpdate () {
            target.Value = UnityEngine.Vector2.SmoothDamp(source.Value, target.Value, ref velocity, smoothTime.Value, maxSpeed.Value);
        }
    }

}
