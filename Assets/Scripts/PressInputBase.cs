using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PressInputBase : MonoBehaviour
{
    private InputAction pressAction;

    protected virtual void Awake()
    {
        pressAction = new InputAction("touch", binding: "<Pointer>/press");

        pressAction.started += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPressBegan(device.position.ReadValue());
            }
        };

        pressAction.performed += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPress(device.position.ReadValue());
            }
        };

        if (pressAction.WasReleasedThisFrame())
            OnPressReleased();

        pressAction.canceled += _ => OnPressCancel();
    }

    protected virtual void OnEnable()
    {
        pressAction.Enable();
    }

    protected virtual void OnDisable()
    {
        pressAction.Disable();
    }

    protected virtual void OnDestroy()
    {
        pressAction.Dispose();
    }

    protected virtual void OnPress(Vector2 position)
    {
    }

    protected virtual void OnPressBegan(Vector2 position)
    {
    }

    protected virtual void OnPressCancel()
    {
    }

    protected virtual void OnPressReleased()
    {
    }
}