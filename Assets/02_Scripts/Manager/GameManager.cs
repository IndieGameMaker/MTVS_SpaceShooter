using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글턴을 접근하기 위한 변수
    // 인스턴스를 생성
    // 전역 접근 가능 (static)
    public static GameManager Instance = null;
    
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private float _createTime = 3.0f;

    // 필드(Field) - 외부에 공개하지 않는 변수
    private bool _isGameOver = false; // camel Case (단봉 낙타법) - 변수
    
    // 프로퍼티 - 외부 접근 가능한 일종의 변수 // Pascal Case (파스칼) - 클래스, 메서드 
    public bool IsGameOver
    {
        get { return _isGameOver;  }
        set
        {
            _isGameOver = value;
            if (_isGameOver == true)
            {
                // InvokeRepeating으로 호출한 메서드를 정지
                CancelInvoke(nameof(CreateMonster));
            }
        }
    }
    
    private void Awake()
    {
        // bool temp = GameManager.Instance.IsGameOver; // getter 영역 실행
        // GameManager.Instance.IsGameOver = true; // setter 영역 실행
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var spawnPointGroup = GameObject.Find("SpawnPointGroup").transform;
        spawnPointGroup.GetComponentsInChildren<Transform>(_points);
        
        // 몬스터 프리팹 로딩
        _monsterPrefab = Resources.Load<GameObject>("Monster");
        
        // 델리게이트함수.Invoke();
        // Invoke("함수명", 지연시간)
        // InvokeRepeating("함수명", 지연시간, 반복간격)
        
        InvokeRepeating(nameof(CreateMonster) , 1.0f, _createTime);
    }

    private void CreateMonster()
    {
        // 난수 발생
        int idx = Random.Range(1, _points.Count);

        // var monster = Instantiate(_monsterPrefab);
        var monster = MonsterPool.Instance.pool.Get();
        
        monster.name = $"Monster_{idx}";

        monster.transform.position = _points[idx].position;
        monster.transform.rotation = Quaternion.identity;
    }
}
