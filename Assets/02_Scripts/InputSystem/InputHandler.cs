using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("InputActionReference")]
    [SerializeField] private InputActionReference _moveAction;

    private void OnEnable()
    {
        _moveAction.action.started += OnMove;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log($"OnMove Started: {ctx.started}");
    }
}
