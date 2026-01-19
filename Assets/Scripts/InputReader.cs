using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action OnFireEvent;
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<bool> OnSpeedEvent;

    private Controls _controls;
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Enable();

    }
    private void OnDisable()
    {
        if (_controls != null)
            _controls.Disable();

    }
    public void OnFire(InputAction.CallbackContext context)
    {
        bool isFire = context.performed ? true : false;
        if (isFire)
            OnFireEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        OnLookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSpeed(InputAction.CallbackContext context)
    {
        bool ispressed = context.performed ? true : false;
        OnSpeedEvent?.Invoke(ispressed);
    }
}
