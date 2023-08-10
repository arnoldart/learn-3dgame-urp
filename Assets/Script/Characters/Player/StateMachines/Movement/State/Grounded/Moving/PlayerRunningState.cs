using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovementState
{
	public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
	{
	}

	public override void Enter()
	{
		base.Enter();

		speedModifier = 1f;
	}

	protected override void AddInputActionsCallbacks()
	{
		base.AddInputActionsCallbacks();

		stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
	}

	protected override void RemoveInputActionsCallbackS()
	{
		base.RemoveInputActionsCallbackS();

		stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
	}

	protected override void OnWalkToggleStarted(InputAction.CallbackContext ctx)
	{
		base.OnWalkToggleStarted(ctx);

		stateMachine.ChangeState(stateMachine.WalkingState);
	}
	private void OnMovementCanceled(InputAction.CallbackContext obj)
	{
		stateMachine.ChangeState(stateMachine.IdlingState);
	}

}
