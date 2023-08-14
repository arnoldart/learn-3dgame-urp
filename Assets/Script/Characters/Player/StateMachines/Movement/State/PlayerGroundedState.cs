using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{

    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }
    
    // Reuseable Method
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

    //End
    
    protected virtual void OnMove()
    {
        if(shouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }
    
    protected virtual void OnMovementCanceled(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }
}
