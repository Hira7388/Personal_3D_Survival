using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // 컨디션이 바뀔 때 호출하는 델리게이트
    public event Action<float, float> OnConditionChanged;

    private float _curValue;
    public float CurValue
    {
        // 여기서 값 제한을 진행하므로 Subtract와 Add에서는 지운다.
        get => _curValue;
        private set
        {
            float clampedValue = Mathf.Clamp(value, 0, maxValue);
            if (Mathf.Approximately(_curValue, clampedValue)) return;

            _curValue = clampedValue;
            OnConditionChanged?.Invoke(_curValue, maxValue); // 값이 바뀔 때마다 이벤트 방송!
        }
    }

    [SerializeField] private float startValue;
    public float StartValue { get => startValue; }
    [SerializeField] private float maxValue;
    public float MaxValue { get => maxValue; }
    [SerializeField] private float passiveValue;
    public float PassiveValue { get => passiveValue; }



    void Start()
    {
        CurValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (passiveValue != 0)
        {
            // Time.deltaTime을 곱해 초당 증감량으로 만듦
            CurValue += passiveValue * Time.deltaTime;
        }
    }

    public void Add(float value)
    {
        // 현재 컨디션 + 추가 회복 컨디션이 최대 컨디션을 넘지 못하도록 제한
        CurValue += value;
    }

    public void Subtract(float value)
    {
        // 위와 비슷한 이유로 최소치를 0으로 제한
        CurValue -= value;
    }
}
