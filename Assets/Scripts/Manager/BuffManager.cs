using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{
    public List<ActiveBuff> activeBuffs = new List<ActiveBuff>();
    private PlayerController controller;
    private Inventory inventory;

    public event Action<List<ActiveBuff>> OnBuffsChanged;


    protected override void Initialize()
    {
        base.Initialize();
    }

    private void Start()
    {
        // 원래 밑에 두가지가 Init에 있었고
        controller = CharacterManager.Instance.Player.controller;
        inventory = CharacterManager.Instance.Player.inventory;
        // 이 밑에 델리게이트는 OnEnable에 있었는데 호출 순서상 캐릭터가 만들어지지 않고 위에서 컨트롤러를 먼저 받으려고해서 Null오류 발생
        inventory.OnUsedConsumeItem += OnItemUsed;
        // 아직 좀 더 나은 해법을 모르겠습니다.
    }

    private void OnDisable()
    {
        inventory.OnUsedConsumeItem -= OnItemUsed;
    }

    private void OnItemUsed(ItemData usedItem)
    {
        // 사용한 아이템에 버프가 존재하고, 해당 버프의 개수가 0개 초과 라면
        if (usedItem.buffs != null && usedItem.buffs.Length > 0)
        {
            foreach(var buff in usedItem.buffs)
            {
                AddBuff(buff);
            }
        }
    }

    public void AddBuff(BuffData buffData)
    {
        ActiveBuff existingBuff = activeBuffs.FirstOrDefault(b => b.Data.type == buffData.type);

        if (existingBuff != null)
        {
            // 이미 같은 타입의 버프가 있다면, 코루틴을 새로 시작할 필요 없이
            // 기존 버프의 남은 시간만 초기화(갱신)해줍니다.
            existingBuff.RemainingTime = buffData.duration;
        }
        else
        {
            // 새로운 버프라면, ActiveBuff 객체를 만들고 타이머 코루틴을 시작합니다.
            ActiveBuff newBuff = new ActiveBuff(buffData);
            StartCoroutine(BuffTimerCoroutine(newBuff));
        }
    }

    private IEnumerator BuffTimerCoroutine(ActiveBuff buff)
    {
        // 1. 리스트에 추가하고, 효과를 적용한 뒤 UI에 방송합니다.
        activeBuffs.Add(buff);
        ApplyBuffEffect(buff.Data);
        OnBuffsChanged?.Invoke(activeBuffs);

        // 2. 남은 시간이 0보다 클 동안 매 프레임 루프를 돕니다.
        while (buff.RemainingTime > 0)
        {
            buff.RemainingTime -= Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 3. 루프가 끝나면(시간이 다 되면), 버프를 제거합니다.
        // 리스트에서 먼저 제거해야 UI 갱신 시 버프가 남아있는 현상을 막을 수 있습니다.
        activeBuffs.Remove(buff);
        RemoveBuffEffect(buff.Data);
        OnBuffsChanged?.Invoke(activeBuffs);

        Debug.Log(buff.Data.type + " 버프가 코루틴에 의해 종료되었습니다.");
    }

    private void ApplyBuffEffect(BuffData buff)
    {
        if (buff.type == BuffType.Speed)
        {
            controller.ApplySpeedBuff(buff.value);
        }
        // 다른 버프 효과
    }

    private void RemoveBuffEffect(BuffData buff)
    {
        if (buff.type == BuffType.Speed)
        {
            controller.RemoveSpeedBuff();
        }
        // 다른 버프 효과
    }
}
