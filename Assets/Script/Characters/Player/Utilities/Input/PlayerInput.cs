using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public PlayerController InputActions { get; private set; }
	public PlayerController.PlayerActions PlayerActions { get; private set; }

	private void Awake()
	{
		InputActions = new PlayerController();
		PlayerActions = InputActions.Player;
	}

	private void OnEnable()
	{
		InputActions.Enable();
	}

	private void OnDisable()
	{
		InputActions.Disable();
	}
}
