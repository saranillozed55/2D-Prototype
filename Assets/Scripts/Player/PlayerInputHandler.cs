using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Move and Jump")]
    public Vector2 MoveValue { get; private set; }
    public event Action OnTryJump;
    public event Action OnJumpReleased;
    public event Action OnInteract;
    public event Action OnAttack;
    public event Action OnDash;
    public static event Action OnMenu;
    public event Action OnSave;
    public event Action OnLoad;

    private readonly HashSet<object> _locks = new();
    private void Start()
    {
        UIStackManager.Instance.RegisterInputHandler(this);
    }

    private void OnEnable()
    {
        if (InputManager.InputControl != null)
        {
            InputManager.InputControl.Player.Jump.performed += OnJumpPerformed;
            InputManager.InputControl.Player.Jump.canceled += OnJumpCanceled;
            InputManager.InputControl.Player.Interact.performed += OnInteractPerformed;
            InputManager.InputControl.Player.Pause.performed += OnMenuPerformed;
            InputManager.InputControl.Player.Dash.performed += OnDashPerformed;
            InputManager.InputControl.Player.Attack.performed += OnAttackPerformed;

            InputManager.InputControl.Player.Save.performed += OnSavePerformed;
            InputManager.InputControl.Player.Load.performed += OnLoadPerformed;


            InputManager.InputControl.UI.Back.performed += OnBackPerformed;
        }
    }
    private void OnDisable()
    {
        if (InputManager.InputControl != null)
        {
            InputManager.InputControl.Player.Jump.performed -= OnJumpPerformed;
            InputManager.InputControl.Player.Jump.canceled -= OnJumpCanceled;
            InputManager.InputControl.Player.Interact.performed -= OnInteractPerformed;
            InputManager.InputControl.Player.Pause.performed -= OnMenuPerformed;
            InputManager.InputControl.Player.Dash.performed -= OnDashPerformed;
            InputManager.InputControl.Player.Attack.performed -= OnAttackPerformed;

            InputManager.InputControl.Player.Save.performed -= OnSavePerformed;
            InputManager.InputControl.Player.Load.performed -= OnLoadPerformed;


            InputManager.InputControl.UI.Back.performed -= OnBackPerformed;

        }
    }

    private void Update()
    {
        MoveValue = InputManager.InputControl.Player.Move.ReadValue<Vector2>();
    }


    private void OnSavePerformed(InputAction.CallbackContext ctx) => OnSave?.Invoke();
    private void OnLoadPerformed(InputAction.CallbackContext ctx) => OnLoad?.Invoke();

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        OnAttack?.Invoke();
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        OnDash?.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        OnTryJump?.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        OnJumpReleased?.Invoke();
    }   

    private void OnInteractPerformed(InputAction.CallbackContext ctx) {
        OnInteract?.Invoke();
    }
    private void OnMenuPerformed(InputAction.CallbackContext ctx)
    {
        OnMenu?.Invoke();
    }

    private void OnBackPerformed(InputAction.CallbackContext ctx)
    {
        UIStackManager.Instance?.Pop();
    }

    public bool IsJumpHeld()
    {
        if (InputManager.InputControl == null) return false;
        return InputManager.InputControl.Player.Jump.IsPressed();
    }

    public void AddLock(object owner)
    {
        _locks.Add(owner);
        UpdateInputState();
    }

    public void RemoveLock(object owner)
    {
        _locks.Remove(owner);
        UpdateInputState();
    }
    public bool IsInputEnabled => _locks.Count == 0;
    private void UpdateInputState()
    {
        if(IsInputEnabled)
        {
            EnableAllInput();
        }
        else
        {
            DisableAllInput();
        }
    }

    private void EnableAllInput()
    {
        InputManager.InputControl.Player.Enable();
    }

    private void DisableAllInput()
    {
        InputManager.InputControl.Player.Disable();
    }
}
