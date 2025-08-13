using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Speed,
    Attack,
    Defense
}

[Serializable]
public class BuffData
{
    public BuffType type;   // 버프 효과
    public float value;     // 버프 수치
    public float duration;  // 버프 지속시간
}

// 활성화된 버프
public class ActiveBuff
{
    public BuffData Data { get; private set; }
    public float RemainingTime { get; set; }

    public ActiveBuff(BuffData data)
    {
        Data = data;
        RemainingTime = data.duration;
    }
}
