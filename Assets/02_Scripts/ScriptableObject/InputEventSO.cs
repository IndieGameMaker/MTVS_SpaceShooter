using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputEventSO", menuName = "Scriptable Objects/InputEventSO")]
public class InputEventSO : ScriptableObject
{
    private event Action<Vector2> onMove;
    private event Action<Vector2> onLook;
    private event Action<bool> onAttack;

    /*
     * Lambda Expression 람다식 : 익명 함수, 간결한 문법으로 함수를 표현하는 방법
     * 람다식 문법 :
     *  - 파라메터가 있는 경우 : (매개변수) => { 함수 본문 }
     *  - 파라메터가 없는 경우 : () => { 함수 본문 }
     * 예시 :
     * Action<int> onDamage = (damage) => { Debug.Log($"플레이어 피격 {damage}!"); };
     * onDamage(10); // 출력: 플레이어 피격 10!
     */
    public void SubscribeMove(Action<Vector2> listener) => onMove += listener;
    public void UnsubscribeMove(Action<Vector2> listener) => onMove -= listener;
    public void RaiseMove(Vector2 value) => onMove?.Invoke(value);

    public void SubscribeLook(Action<Vector2> listener) => onLook += listener;
    public void UnsubscribeLook(Action<Vector2> listener) => onLook -= listener;
    public void RaiseLook(Vector2 value) => onLook?.Invoke(value);

    public void SubscribeAttack(Action<bool> listener) => onAttack += listener;
    public void UnsubscribeAttack(Action<bool> listener) => onAttack -= listener;
    public void RaiseAttack(bool isPressed) => onAttack?.Invoke(isPressed);
}