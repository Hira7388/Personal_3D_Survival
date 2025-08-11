using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("Condition Info")]
    [SerializeField] private float curValue;
    public float CurValue { get => curValue; }
    [SerializeField] private float startValue;
    public float StartValue { get => startValue; }
    [SerializeField] private float maxValue;
    public float MaxValue { get => maxValue; }
    [SerializeField] private float passiveValue;
    public float PassiveValue { get => passiveValue; }
    [SerializeField] private Image uiBar;


    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        // 현재 컨디션 + 추가 회복 컨디션이 최대 컨디션을 넘지 못하도록 제한
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Substrac(float value)
    {
        // 위와 비슷한 이유로 최소치를 0으로 제한
        curValue = Mathf.Max(curValue - value, 0);
    }
}
