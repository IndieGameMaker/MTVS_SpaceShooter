//#define INPUTACTION_REFERENCE
#define WRAPPER_CLASS
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
#if INPUTACTION_REFERENCE    
    [Header("InputActionReference")]
    [SerializeField] private InputActionReference moveAction;

    private void OnEnable()
    {
        // 액션의 페이즈에 따라 다른 로직을 수행할 수 있도록 이벤트 구독
        moveAction.action.started += OnMove;
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        
        // 액션을 활성화하여 입력을 받을 수 있도록 설정
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.started -= OnMove;
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;

        moveAction.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        // 페이즈에 따라 다른 로직을 수행할 수 있음
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                Debug.Log($"OnMove Started: {ctx.started}");
                break;
            case InputActionPhase.Performed:
                Debug.Log($"OnMove performed: {ctx.ReadValue<Vector2>()}");
                break;
            case InputActionPhase.Canceled:
                Debug.Log($"OnMove canceled: {ctx.canceled}");
                break;
        }
    }
#endif
    
#if WRAPPER_CLASS
    // 이벤트버스 패턴을 활용한 래퍼 클래스 사용 예시
    
    [SerializeField] private InputEventSO _inputEventSO;
    private InputSystem_Actions _inputSystemActions;

    private void Awake()
    {
        // 래퍼 클래스 인스턴스 생성 및 액션 활성화
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.Player.Enable();
    }

    private void OnEnable()
    {
        // 액션의 페이즈에 따라 다른 로직을 수행할 수 있도록 이벤트 구독
        _inputSystemActions.Player.Move.performed += OnMove;
        _inputSystemActions.Player.Move.canceled += OnMove;
        
        _inputSystemActions.Player.Look.performed += OnLook;
        _inputSystemActions.Player.Attack.started += OnAttack;
        
        _inputSystemActions.Player.Attack.canceled += OnAttack;
    }

    private void OnDisable()
    {
        _inputSystemActions.Player.Move.performed -= OnMove;
        _inputSystemActions.Player.Move.canceled -= OnMove;
        _inputSystemActions.Player.Look.performed -= OnLook;
        _inputSystemActions.Player.Attack.started -= OnAttack;
        _inputSystemActions.Player.Attack.canceled -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        // Debug.Log 로 값 확인 후 이벤트 채널에 입력값 전달하는 로직 구현
        _inputEventSO.RaiseMove(ctx.ReadValue<Vector2>());
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        _inputEventSO.RaiseLook(ctx.ReadValue<Vector2>());
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        _inputEventSO.RaiseAttack(ctx.started);
    }
#endif
}
