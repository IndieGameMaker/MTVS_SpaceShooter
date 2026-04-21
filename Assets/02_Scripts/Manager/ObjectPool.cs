using System.Collections.Generic;
using UnityEngine;

// T는 반드시 Component를 상속해야 함 (where 제약조건)
public class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T prefab;
    [SerializeField] private int initialSize = 10;

    private Queue<T> pool = new Queue<T>();

    public virtual void Awake()
    {
        // 초기 풀 채우기
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        // 풀이 비면 새로 생성
        return Instantiate(prefab);
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }    
}
