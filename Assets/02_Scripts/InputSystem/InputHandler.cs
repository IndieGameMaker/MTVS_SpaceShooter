// 전처리기 (PreProcessor)
//#define INPUTACTION_REF
//#define WRAPPER_CLASS

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    #if INPUTACTION_REF    
    [Header("InputActionReference")]
    [SerializeField]
    private InputActionReference _moveAction;

    private void OnEnable()
    {
        // 액션의 페이즈에 따라서 다른 로직을 수행
        _moveAction.action.started += OnMove;
        _moveAction.action.performed += OnMove;
        _moveAction.action.canceled += OnMove;
        
        // 액션을 활성화
        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.started -= OnMove;
        _moveAction.action.performed -= OnMove;
        _moveAction.action.canceled -= OnMove;
        
        // 액션 비활성화
        _moveAction.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        // 페이즈에 따라서 다른 로직으로 분기
        switch (ctx.action.phase)
        {
            case InputActionPhase.Started:
                Debug.Log($"OnMove Started: {ctx.started}");
                break;
            case InputActionPhase.Performed:
                Debug.Log($"OnMove Performed: {ctx.ReadValue<Vector2>()}");
                break;
            case InputActionPhase.Canceled:
                Debug.Log($"OnMove Canceled: {ctx.canceled}");
                break;
        }
    }
    #endif
    
    #if WRAPPER_CLASS
    private InputSystem_Actions _inputSystemActions;

    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.Enable();
    }
    #endif
}