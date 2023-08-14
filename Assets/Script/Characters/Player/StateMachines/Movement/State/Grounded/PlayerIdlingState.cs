using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
	public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
	{

	}

	//IState Method
	public override void Enter()
	{
		base.Enter();
		speedModifier = 0f;

		ResetVelocity();
	}

	public override void Update()
	{
		base.Update();

		if(movementInput == Vector2.zero)
		{
			return;
		}

		OnMove();
	}
}
