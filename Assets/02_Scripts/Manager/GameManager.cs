using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글턴을 접근하기 위한 변수
    // 인스턴스를 생성
    // 전역 접근 가능 (static)
    public static GameManager Instance = null;
    
    [SerializeField] private List<Transform> points = new List<Transform>();

    private void Awake()
    {
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
        spawnPointGroup.GetComponentsInChildren<Transform>(points);
    }
}
