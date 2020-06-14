// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Sets The degree to which this object is affected by gravity.  NOTE: Game object must have a rigidbody 2D.")]
    public class SetGravity2dScale : ComponentAction<Rigidbody2D>
	{
		[RequiredField]
		[Tooltip("The gravity scale effect")]
		public FsmFloat gravityScale;

		[Tooltip("Set gravity every frame")]
		public FsmBool everyFrame;
		
		public override void OnEnter() 
		{
			gravityScale.Value = Mathf.Clamp(gravityScale.Value, 0f, float.PositiveInfinity);
		}

		public override void OnUpdate()
		{
			DoSetGravityScale();

			if (!everyFrame.Value)
				Finish();
		}
		
		void DoSetGravityScale()
		{	
			Debug.Log("Unga bunga");
			if (gravityScale.Value != Physics2D.gravity.magnitude) {
				Vector2 grav = Physics2D.gravity.normalized;
				if (grav.y > 0f) {
					grav *= -1f;
				}
				Physics2D.gravity = grav * gravityScale.Value;
			}
		}
	}
}