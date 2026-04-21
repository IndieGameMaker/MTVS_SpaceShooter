using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 몬스터 캐릭터 컨트롤러 클래스
public class MonsterController : MonoBehaviour, IDamageable
{
    // 몬스터 상태 정의 (Enum)
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }

    // 몬스터의 상태를 저장하는 변수
    [SerializeField] private State _state;
    // 추적 사정거리
    [SerializeField] private float _traceDist = 10.0f;
    // 공격 사정거리
    [SerializeField] private float _attackDist = 2.0f;
    // 몬스터 사명여부
    [SerializeField] private bool _isDead = false;
    // 몬스터의 HP
    [SerializeField] private float _hp = 100.0f;

    // 컴포넌트 캐싱
    private Transform _monsterTr;
    private Transform _playerTr;
    private NavMeshAgent _agent;
    private Animator _animator;
    
    // 애니메이터 파라메터 해시값 추출
    private readonly int hashIsTrace = Animator.StringToHash("IsTrace");
    private readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDead = Animator.StringToHash("Dead");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDead = Animator.StringToHash("PlayerDead");

    // WaitForSeconde 캐싱
    private WaitForSeconds _ws;
    
    // 몬스터의 모든 컬라아더 저장할 컬렉션
    private List<Collider> _colliders = new List<Collider>(); // new ();

    private void OnEnable()
    {
        // 이벤트 구독 (Subscribe)
        PlayerController.OnPlayerDead += this.OnPlayerDead;
        
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());        
    }

    private void OnDisable()
    {
        // 이벤트 구독 해지 (Unsubscribe)
        PlayerController.OnPlayerDead -= this.OnPlayerDead;
    }

    private void Awake()
    {
        _ws = new WaitForSeconds(0.3f);
        _monsterTr = GetComponent<Transform>();
        _playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        // 컬라이더 컴포넌트 추출 후 리스트에 저장
        _monsterTr.GetComponentsInChildren<Collider>(_colliders);
        

    }

    // 코루틴 1 - 몬스터의 상태를 갱신 (0.3초)
    private IEnumerator CheckMonsterState()
    {
        // while (_isDead == false)
        while (!_isDead)
        {
            if (_state == State.DEAD) yield break;
            
            // 반복되는 로직
            // Player와 Monster 간의 거리 계산
            // float dist = Vector3.Distance(_monsterTr.position, _playerTr.position);
            // 벡터의 뺄셈 연산 (A - B) ==> A와 B간의 벡터
            float dist = (_monsterTr.position - _playerTr.position).sqrMagnitude;
            // 5  5*5 = 25

            if (dist <= _attackDist * _attackDist)
            {
                _state = State.ATTACK; // 공격 사정거리 이내인 경우
            }
            else if (dist <= _traceDist * _traceDist)
            {
                _state = State.TRACE; // 추적 사정거리 이내인 경우
            }
            else
            {
                _state = State.IDLE;
            }

            yield return _ws;
        }
    }

    // 코루틴 2 - 몬스터의 상태에 따라서 행동 (0.3초)
    private IEnumerator MonsterAction()
    {
        while (!_isDead)
        {
            switch (_state)
            {
                case State.IDLE:
                    // 아이들 로직
                    _agent.isStopped = true;
                    _animator.SetBool(hashIsTrace, false);
                    break;
                
                case State.TRACE:
                    // 추적 로직
                    _agent.SetDestination(_playerTr.position);
                    // _agent.destination = _playerTr.position;
                    _agent.isStopped = false;
                    // Walking Animation
                    _animator.SetBool(hashIsAttack, false); // 이미 공격상태로 전이된 상태일 경우  
                    _animator.SetBool(hashIsTrace, true);
                    
                    break;
                case State.ATTACK:
                    // 공격 로직
                    _agent.isStopped = true;
                    _animator.SetBool(hashIsAttack, true);
                    break;
                case State.DEAD:
                    _isDead = true;
                    _agent.isStopped = true;
                    _animator.SetTrigger(hashDead);
                    ToggleColliders(false);
                    
                    // 잠시 기다린 후 처리
                    yield return new WaitForSeconds(2.0f);
                    // 각종 수치 초기화
                    _hp = 100.0f;
                    _state = State.IDLE;
                    _isDead = false;
                    ToggleColliders(true); // 컬라이터 활성화
                    // 오브젝트 풀 반납
                    MonsterPool.Instance.pool.Release(this);
                    break;
            }

            yield return _ws;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (!_isDead && coll.collider.CompareTag("BULLET"))
        {
            // 오브젝트 풀로 반환
            BulletPool.Instance.Return(coll.gameObject.GetComponent<Bullet>());
        }
    }
    
    public void TakeDamage(int damage)
    {
        _animator.SetTrigger(hashHit);
        _hp -= (float)damage;

        if (_hp <= 0.0f)
        {
            _state = State.DEAD;
        }        
    }

    private void ToggleColliders(bool active)
    {
        // TODO: 오류 수정
        foreach (var coll in _colliders)
        {
            coll.enabled = active;
        }
    }

    public void OnPlayerDead()
    {
        // 댄스 애니메이션 처리
        _animator.SetTrigger(hashPlayerDead);
        // 네비메시 정지
        _agent.isStopped = true;
        // 코루틴 정지
        // StopCoroutine(MonsterAction());
        // StopCoroutine(CheckMonsterState());
        
        StopAllCoroutines();
    }
    // private void Update()
    // {
    //     // 주인공과 몬스터간의 거리를 계산
    //     // 몬스터의 상태를 갱신
    //     // 추적 사정거리 여부 확인
    //     // 공격 사정거리 여부 확인
    // }

}