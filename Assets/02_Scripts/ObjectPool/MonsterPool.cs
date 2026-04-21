using System;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance;
    public PoolManager<MonsterController> pool;

    private void Awake()
    {
        Instance = this;
        var monster = Resources.Load<MonsterController>("Monster");
        pool = new PoolManager<MonsterController>(monster);
    }
}
