using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
	public Rigidbody rb { get; private set; }
	public PlayerInput Input { get; private set; }
	public Transform MainCameraTransform { get; private set; }
	private PlayerMovementStateMachine movementStateMachine;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		Input = GetComponent<PlayerInput>();
		MainCameraTransform = Camera.main.transform;
		movementStateMachine = new PlayerMovementStateMachine(this);
	}

	private void Start()
	{
		Input = GetComponent<PlayerInput>();
		movementStateMachine.ChangeState(movementStateMachine.IdlingState);
	}

	private void Update()
	{
		movementStateMachine.HandleInput();
		movementStateMachine.Update();
	}

	private void FixedUpdate()
	{
		movementStateMachine.PhysicsUpdate();
	}
}
