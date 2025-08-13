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

    public void AddBuff(BuffData buff)
    {
        ActiveBuff existingBuff = activeBuffs.FirstOrDefault(b => b.Data.type == buff.type);

        if (existingBuff != null)
        {
            // 같은 타입의 버프가 있다면 지속시간을 갱신 시켜준다.
            existingBuff.RemainingTime = buff.duration;
        }
        else
        {
            // 새로운 버프라면, 활성 버프로 새로 생성하고 코루틴을 실행한다.
            ActiveBuff newBuff = new ActiveBuff(buff);
            StartCoroutine(BuffTimerCoroutine(newBuff));
        }
    }

    IEnumerator BuffTimerCoroutine(ActiveBuff buff)
    {
        // 리스트에 버프 추가, 효과 발동, 이벤트 알림
        activeBuffs.Add(buff);
        ApplyBuffEffect(buff.Data);
        OnBuffsChanged?.Invoke(activeBuffs);

        // 지속 시간동안 계속 돈다.
        while (buff.RemainingTime > 0)
        {
            buff.RemainingTime -= Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 리스트 제거, 버프 효과 제거, 이벤트 알림.
        activeBuffs.Remove(buff);
        RemoveBuffEffect(buff.Data);
        OnBuffsChanged?.Invoke(activeBuffs);
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
