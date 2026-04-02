using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : IInputSource
{
    private InputSystem_Actions inputActions;
    private Vector2 movement;

    public PlayerInputHandler()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => movement = Vector2.zero;

        inputActions.Enable();
    }

    public Vector2 GetMovement() => movement;
}