using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    [Header("Logic Components (From Player)")]
    [SerializeField] private Condition health;
    [SerializeField] private Condition hunger;
    [SerializeField] private Condition stamina;

    [Header("UI Components (From Canvas)")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image hungerBar;
    [SerializeField] private Image staminaBar;


    void Start()
    {
         // 각 Condition의 이벤트가 발생할 때마다, 연결된 UI 업데이트 함수를 실행하도록 '구독' 신청
        if(health != null) health.OnConditionChanged += (cur, max) => UpdateBar(healthBar, cur, max);
        if(hunger != null) hunger.OnConditionChanged += (cur, max) => UpdateBar(hungerBar, cur, max);
        if(stamina != null) stamina.OnConditionChanged += (cur, max) => UpdateBar(staminaBar, cur, max);
    }

    private void UpdateBar(Image bar, float curValue, float maxValue)
    {
        if (bar != null)
        {
            bar.fillAmount = (maxValue > 0) ? curValue / maxValue : 0;
        }
    }
}
