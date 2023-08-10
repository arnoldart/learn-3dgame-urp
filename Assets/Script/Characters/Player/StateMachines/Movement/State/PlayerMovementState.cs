using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
	protected PlayerMovementStateMachine stateMachine;
	protected Vector2 movementInput;

	protected float baseSpeed = 5f;
	protected float speedModifier = 1f;

	protected Vector3 currentTargetRotation;
	protected Vector3 timeToReachTargetRotation;
	protected Vector3 dampedTargetRotationCurrentVelocity;
	protected Vector3 dampedTargetRotationPassedTime;

	protected bool shouldWalk;

	public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
	{
		stateMachine = playerMovementStateMachine;

		InitializeData();
	}

	private void InitializeData()
	{
		timeToReachTargetRotation.y = 0.14f;
	}

	public virtual void Enter()
	{
		Debug.Log("State : " + GetType().Name);

		AddInputActionsCallbacks();
	}

	public virtual void Exit()
	{
		RemoveInputActionsCallbackS();
	}

	public virtual void HandleInput()
	{
		ReadMovementInput();
	}

	public virtual void PhysicsUpdate()
	{
		Move();
	}

	public virtual void Update()
	{
		
	}

	private void Move()
	{
		if (movementInput == Vector2.zero || speedModifier == 0f)
		{
			return;
		}

		Vector3 movementDirection = GetMovementInputDirection();

		float targetRotationYAngle = Rotate(movementDirection);

		Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

		float movementSpeed = GetMovementSpeed();

		Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

		stateMachine.Player.rb.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
	}


	protected Vector3 GetMovementInputDirection()
	{
		return new Vector3(movementInput.x, 0f, movementInput.y);
	}

	private float Rotate(Vector3 direction)
	{
		float directionAngle = UpdateTargetRotation(direction);

		RotateTowardsTargetRotation();

		return directionAngle;
	}

	protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
	{
		float directionAngle = GetDirectionAngle(direction);

		if (shouldConsiderCameraRotation)
		{
			directionAngle = AddCameraRotationToAngle(directionAngle);
		}

		if (directionAngle != currentTargetRotation.y)
		{
			UpdateTargetRotationData(directionAngle);
		}

		return directionAngle;
	}

	private float GetDirectionAngle(Vector3 direction)
	{
		float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

		if (directionAngle < 0f)
		{
			directionAngle += 360f;
		}

		return directionAngle;
	}

	private float AddCameraRotationToAngle(float angle)
	{
		angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

		if (angle > 360f)
		{
			angle -= 360f;
		}

		return angle;
	}

	private void UpdateTargetRotationData(float targetAngle)
	{
		currentTargetRotation.y = targetAngle;

		dampedTargetRotationPassedTime.y = 0f;
	}

	protected void RotateTowardsTargetRotation()
	{
		float currentYAngle = stateMachine.Player.rb.rotation.eulerAngles.y;

		if (currentYAngle == currentTargetRotation.y)
		{
			return;
		}

		float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, ref dampedTargetRotationCurrentVelocity.y, timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);

		dampedTargetRotationPassedTime.y += Time.deltaTime;

		Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

		stateMachine.Player.rb.MoveRotation(targetRotation);
	}

	protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
	{
		return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
	}

	protected Vector3 GetPlayerHorizontalVelocity()
	{
		Vector3 playerHorizontalVelocity = stateMachine.Player.rb.velocity;

		playerHorizontalVelocity.y = 0f;

		return playerHorizontalVelocity;
	}

	//#region Main Methods

	//private float Rotate(Vector3 direction)
	//{
	//	float directionAngle = UpdateTargetRotation(direction);

	//	RotateTowardsTargetRotation();

	//	return directionAngle;
	//}

	//protected float UpdateTargetRotation(Vector3 direction, bool shoudConsiderCameraRotation = true)
	//{
	//	float directionAngle = GetDirectionAngle(direction);

	//	if (shoudConsiderCameraRotation)
	//	{
	//		directionAngle = AddCameraRotationToAngle(directionAngle);
	//	}

	//	directionAngle = AddCameraRotationToAngle(directionAngle);

	//	if (directionAngle != currentTargetRotation.y)
	//	{
	//		UpdateTargetRotationData(directionAngle);
	//	}

	//	return directionAngle;
	//}

	//private void UpdateTargetRotationData(float targetAngle)
	//{
	//	currentTargetRotation.y = targetAngle;
	//	dampedTargetRotationPassedTime.y = 0f;
	//}

	//protected void RotateTowardsTargetRotation()
	//{
	//	float currentYAngle = stateMachine.Player.rb.rotation.eulerAngles.y;

	//	if(currentYAngle == currentTargetRotation.y)
	//	{
	//		return;
	//	}

	//	float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, ref dampedTargetRotationCurrentVelocity.y, timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);

	//	dampedTargetRotationPassedTime.y += Time.deltaTime;

	//	Quaternion targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);

	//	stateMachine.Player.rb.MoveRotation(targetRotation);
	//}

	//private float AddCameraRotationToAngle(float angle)
	//{
	//	angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

	//	if (angle > 360f)
	//	{
	//		angle -= 360f;
	//	}

	//	return angle;
	//}

	//private static float GetDirectionAngle(Vector3 direction)
	//{
	//	float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

	//	if (directionAngle < 0f)
	//	{
	//		directionAngle += 360f;
	//	}

	//	return directionAngle;
	//}

	private void ReadMovementInput()
	{
		movementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
	}

	//private void Move()
	//{
	//	if(movementInput == Vector2.zero || speedModifier == 0f)
	//	{
	//		return;
	//	}

	//	Vector3 movementDirection = GetMovementInputDirection();

	//	float targetRotationYAngle = Rotate(movementDirection);

	//	Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

	//	float movementSpeed = GetMovementSpeed();

	//	Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

	//	stateMachine.Player.rb.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange); ;
	//}

	//protected Vector3 GetTargetRotationDirection(float targetAngle)  
	//{
	//	return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
	//}

	//protected Vector3 GetMovementInputDirection()
	//{
	//	return new Vector3(movementInput.x, 0f, movementInput.y);
	//}

	protected float GetMovementSpeed()
	{
		return baseSpeed * speedModifier;
	}

	//protected Vector3 GetPlayerHorizontalVelocity()
	//{
	//	Vector3 playerHorizontalVelocity = stateMachine.Player.rb.velocity;
	//	playerHorizontalVelocity.y = 0f;
	//	return playerHorizontalVelocity;
	//}

	protected void ResetVelocity()
	{
		stateMachine.Player.rb.velocity = Vector3.zero;
	}

	protected virtual void AddInputActionsCallbacks()
	{
		stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
	}

	protected virtual void RemoveInputActionsCallbackS()
	{
		stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
	}

	//#endregion

	//#region Input Method
	protected virtual void OnWalkToggleStarted(InputAction.CallbackContext ctx)
	{
		shouldWalk = !shouldWalk;
	}
	//#endregion
}
