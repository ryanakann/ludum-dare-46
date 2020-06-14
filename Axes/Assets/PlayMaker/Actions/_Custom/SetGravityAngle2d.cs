// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Sets the gravity vector, or individual axis.")]
	public class SetGravityAngle2d : FsmStateAction
	{
		public FsmFloat angle;

		[Tooltip("Check if angle is supplied in degrees")]
		public FsmBool deg2Rad;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			angle = new FsmFloat { UseVariable = true };
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoSetGravity();
			
			if (!everyFrame)
				Finish();		
		}
		
		public override void OnUpdate()
		{
			DoSetGravity();
		}
		
		void DoSetGravity()
		{
			float x = Mathf.Cos(angle.Value * (deg2Rad.Value ? Mathf.Deg2Rad : 1) - Mathf.PI / 2f);
			float y = Mathf.Sin(angle.Value * (deg2Rad.Value ? Mathf.Deg2Rad : 1) - Mathf.PI / 2f);

			Physics2D.gravity = new Vector2(x, y) * Physics2D.gravity.magnitude;
		}
	}
}