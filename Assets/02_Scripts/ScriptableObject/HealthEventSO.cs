using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthEventSO", menuName = "Scriptable Objects/HealthEventSO")]
public class HealthEventSO : ScriptableObject
{
    // 구독자를 저장할 리스너 (델리게이트)
    private event Action<int> observers;

    // 구독자를 추가 메서드
    public void Subscribe(Action<int> linstner)
    {
        observers += linstner;
    }

    // 구독자를 해지 메서드
    public void UnSubscribe(Action<int> linstner)
    {
        observers -= linstner;
    }

    // 이벤트 발행 요청 메서드
    public void Raise(int hp)
    {
        observers?.Invoke(hp);
    }
}
