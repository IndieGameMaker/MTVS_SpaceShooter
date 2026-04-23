using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputEventSO", menuName = "Scriptable Objects/InputEventSO")]
public class InputEventSO : ScriptableObject
{
    // 리스너 정의
    private event Action<Vector2> onMove;
    private event Action<Vector2> onLook;
    
    /*
     * Lambda Expression (람다식) : 익명 함수, 간단한 문법으로 함수를 표현하는 방법
     * 람다식 문법 :
     *  - 파라메터가 있는 경우 :   (파라메터) => { 함수 로직 }   => goes to , Lambda
     *  - 파라메터가 없는 경우 :   () => { 함수 로직 }
     * Action<int> onDamage += 함수;
     * 함수;
     *
     * Action<int> onDamage = (damage) => { Debug.Log("피격데미지" + damage); }
     * onDamage?.Invoke(10); 
     */
    
    // Move 구독 해지 처리
    public void SubscribeMove(Action<Vector2> listener) => onMove += listener;
    public void UnsubscribeMove(Action<Vector2> listener) => onMove -= listener;
    // Raise 처리
    public void RaiseMove(Vector2 value) => onMove?.Invoke(value);
    
    // Look 구독 해지 처리
    public void SubscribeLook(Action<Vector2> listener) => onLook += listener;
    public void UnsubscribeLook(Action<Vector2> listener) => onLook -= listener;
    // Raise 처리
    public void RaiseLook(Vector2 value) => onLook?.Invoke(value);
}
