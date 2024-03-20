using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    // Movement and fire events for player
    public event Action<Vector2> MoveEvent;
    public event Action<bool> PrimaryFireEvent;
    public Vector2 AimPosition { get; private set; }
    private Controls controls;

    // Enables input controls 
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    // 
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // '?' only invokes if there's a listener, prevents errors if bool val is null
            PrimaryFireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            PrimaryFireEvent.Invoke(false);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }
}
