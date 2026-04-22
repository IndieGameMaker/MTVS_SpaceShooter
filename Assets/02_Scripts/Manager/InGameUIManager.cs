using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private HealthEventSO healthEventSO;

    private void OnEnable()
    {
        healthEventSO.Subscribe(OnHpChanged);
    }

    private void OnDisable()
    {
        healthEventSO.UnSubscribe(OnHpChanged);
    }

    // 주인공이 피격시 발행하는 이벤트를 수신받았을 때 호출할(연결할) 메서드
    private void OnHpChanged(int hp)
    {
        hpBar.fillAmount = (float)hp / 100f;
    }
}
