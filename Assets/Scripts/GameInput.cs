using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause
    }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler onPauseAction;
    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputAction.Player.Enable();

        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;

    }

    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        onPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Alternate:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
        .OnComplete(callback =>
        {
            callback.Dispose();
            playerInputAction.Player.Enable();
            onActionRebound();

            playerInputAction.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        })
        .Start();
    }
}
