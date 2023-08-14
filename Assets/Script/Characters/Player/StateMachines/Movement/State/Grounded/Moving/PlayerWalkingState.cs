using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerGroundedState
{
	public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
	{
	}

	public override void Enter()
	{
		base.Enter();

		speedModifier = 0.225f;

	}

	protected override void OnWalkToggleStarted(InputAction.CallbackContext ctx)
	{
		base.OnWalkToggleStarted(ctx);

		stateMachine.ChangeState(stateMachine.RunningState);
	}
}
