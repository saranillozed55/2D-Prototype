using UnityEngine;
using UnityEngine.InputSystem;

public class InputSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        playerInput.onControlsChanged += HandleControlsChanged;
    }

    private void HandleControlsChanged(PlayerInput input)
    {
        if(input.currentControlScheme == "GamePad")
        {
            Debug.Log("Switched to Controller");
        }
        else
        {
            Debug.Log("Switched to Keyboard");
        }
    }
}
