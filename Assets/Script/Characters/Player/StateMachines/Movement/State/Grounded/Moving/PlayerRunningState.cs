using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerGroundedState
{
	public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
	{
	}

	public override void Enter()
	{
		base.Enter();

		speedModifier = 1f;
	}

	protected override void OnWalkToggleStarted(InputAction.CallbackContext ctx)
	{
		base.OnWalkToggleStarted(ctx);

		stateMachine.ChangeState(stateMachine.WalkingState);
	}

}
