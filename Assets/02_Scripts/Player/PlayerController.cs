using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float v;
    private float h;
    private float r;

    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private float _rotateSpeed = 200.0f;
    
    private Animator _animator;
    
    // 애니메이션 파라메터 해시값 추출
    private readonly int _hashSpeed = Animator.StringToHash("Speed");
    private readonly int _hashStrafe = Animator.StringToHash("Strafe");

    private float _initHp = 100.0f;
    private float _currHp = 100.0f;
    
    // 이벤트 채널 스크립터블 오브젝트
    [SerializeField] private HealthEventSO healthEventSO;
    [SerializeField] private InputEventSO _inputEventSO;
    
    // 델리게이트 (Delegate) : 대리자 , 함수를 저장하기 위한 데이터를 정의
    // int hp = 100;
    // public void Sum (int a, int b) { return a + b;};
    // 델리게이트 변수명 = Sum;
    // 델리게이트 SumDelegate = Sum;
    
    
    // 옵저버 패턴(Observer Pattern)
    // 1. 델리게이트 선언
    // public delegate 함수원형;
    // public delegate void PlayerDieHandler();
    // 델리게이트 정의
    // public static event PlayerDieHandler OnPlayerDead;
    
    // 2. Action : NET 미리 정의된 내장 델리게이트
    public static event Action OnPlayerDead;
    
    // public static event Action<T1, T2, ..., T16> ActionMethod; 
    
    
/*
    // OnPlayerDead = PlayerDead;
    OnPlayerDead();
    OnPlayerDead?.Invoke();
 */   
    #region 유니티 콜백 메서드

    private void OnEnable()
    {
        _inputEventSO.SubscribeMove(OnMoveInput);
    }

    private void OnDisable()
    {
        _inputEventSO.UnsubscribeMove(OnMoveInput);
    }


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        InputHandler();
        Movement();
        Animate();
    }

    #region 애니메이션 설정

    private void Animate()
    {
        _animator.SetFloat(_hashSpeed, v);
        _animator.SetFloat(_hashStrafe, h);
    }
    #endregion

    private void OnTriggerEnter(Collider coll)
    {
        if (_currHp > 0.0f && coll.CompareTag("PUNCH"))
        {
            // Debug.Log(coll.gameObject.name);
            PlayerDamaged(10.0f);
        }
    }

    private void PlayerDamaged(float damage)
    {
        _currHp -= damage;
        // _hpBar.fillAmount = _currHp / _initHp;
        
        // 이벤트 채널에게 데미지 이벤트 호출 요청
        healthEventSO.Raise((int)_currHp);
        
        if (_currHp <= 0.0f)
        {
            // 주인공 사망 처리
            Debug.Log("플레이어 사망");
            // PlayerDead();
            
            // 이벤트 발행 (Event Raise)
            OnPlayerDead?.Invoke();
            
            // GameManager의 IsGameOver 변경
            GameManager.Instance.IsGameOver = true;
            
            // GameObject.Find("GameManager").GetComponent<GameManager>().IsGameOver = true;
        }
    }

    private void PlayerDead()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        foreach (var monster in monsters)
        {
            // monster.SendMessage("OnPlayerDead", SendMessageOptions.DontRequireReceiver);
            monster.GetComponent<MonsterController>().OnPlayerDead();
        }
    }

    #endregion

    #region 입력처리

    private void OnMoveInput(Vector2 input)
    {
        v = input.y;
        h = input.x;
    }
    
    private void InputHandler()
    {
        // v = Input.GetAxis("Vertical"); // -1.0f ~ 0.0f ~ +1.0f
        // h = Input.GetAxis("Horizontal"); // -1.0f ~ 0.0f ~ +1.0f
        r = Input.GetAxis("Mouse X"); // -   /   +
    }
    #endregion

    #region 이동 처리

    private void Movement()
    {
        // 좌표 += 방향 * 속도 * 변위 * 시간보정값
        // transform.position += Vector3.forward * (5.0f * v * Time.deltaTime);
        // transform.position += Vector3.right * (5.0f * h * Time.deltaTime);
        
        // 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Debug.Log($"정규화 이전 벡터 {moveDir.magnitude}");
        // Debug.Log($"정규화 이후 벡터 {moveDir.normalized.magnitude}");
        // 이동 처리
        transform.Translate(moveDir.normalized * (_speed * Time.deltaTime));
        // 회전 처리
        transform.Rotate(Vector3.up * _rotateSpeed * r * Time.deltaTime);
    }
    /* 정규화 벡터, 단위 벡터 (Unit Vector)
     * Vector3.forward = Vector3(0, 0, 1)
     * Vector3.right   = Vector3(1, 0, 0)
     * Vector3.up      = Vector3(0, 1, 0)
     *
     * Vector3.one     = Vector3(1, 1, 1)
     * Vector3.zero    = Vector3(0, 0, 0)
     */
    
    
    
    #endregion
}
