using UnityEngine;

public class InputManager : GenericSingleton<InputManager>
{
    private static PlayerInput_Actions inputControl;
    public static PlayerInput_Actions InputControl
    {
        get
        {
            if (inputControl == null)
            {
                inputControl = new PlayerInput_Actions();
            }
            return inputControl;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable() => InputControl.Player.Enable();
    private void OnDisable() => InputControl.Disable();

    public static void SwitchMap(ActionMapType map)
    {
        InputControl.Player.Disable();
        InputControl.UI.Disable();

        switch (map) {
            case ActionMapType.Player:
                InputControl.Player.Enable();
                break;
            case ActionMapType.UI:
                InputControl.UI.Enable();
                break;
            case ActionMapType.None:
                break;
        }
    }
}
