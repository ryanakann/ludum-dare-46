// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace Axes.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameLogic)]
    [Tooltip("Tween a float variable using a custom easing function.")]
    public class SmoothDampFloat : FsmStateAction
    {
        public FsmFloat source;
        public FsmFloat target;
        private float velocity;
        public FsmFloat smoothTime;
        public FsmFloat maxSpeed;


        public override void OnUpdate () {
            target.Value = UnityEngine.Mathf.SmoothDamp(source.Value, target.Value, ref velocity, smoothTime.Value, maxSpeed.Value);
        }
    }

}
