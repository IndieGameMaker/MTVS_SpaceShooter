using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _initialSize = 10;
    
    // 큐 자료형 (선입선출 FIFO)   <==> 스택 자료형 (후입선출 LIFO)
    private Queue<T> _pool = new Queue<T>();

    public virtual void Awake()
    {
        // 초기 풀 채우기 (초기 생성)
        for (int i = 0; i < _initialSize; i++)
        {
            // 생성
            T obj = Instantiate(_prefab);
            // 큐에 넣기 전에 비활서화
            obj.gameObject.SetActive(false);
            // 큐에 적재
            _pool.Enqueue(obj);
        }
    }
}
