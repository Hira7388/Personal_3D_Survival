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

    private void Update()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            ActiveBuff buff = activeBuffs[i];

            // 남은 시간을 줄여줍니다.
            buff.RemainingTime -= Time.deltaTime;

            // 남은 시간이 다 되면
            if (buff.RemainingTime <= 0)
            {
                // 버프 효과를 제거하고
                RemoveBuffEffect(buff.Data);
                // 리스트에서 삭제합니다.
                activeBuffs.RemoveAt(i);
                OnBuffsChanged?.Invoke(activeBuffs);
            }
        }
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
            // 이미 있다면, 남은 시간을 새로 들어온 버프의 지속시간으로 초기화(갱신)합니다.
            existingBuff.RemainingTime = buff.duration;
        }
        else
        {
            // 없다면, 새로운 ActiveBuff를 생성해서 리스트에 추가하고 효과를 적용합니다.
            ActiveBuff newBuff = new ActiveBuff(buff);
            activeBuffs.Add(newBuff);
            ApplyBuffEffect(newBuff.Data);
        }

        OnBuffsChanged?.Invoke(activeBuffs);
    }

    // 버프 UI 표시를 위해 제거
    //IEnumerator BuffProcess(BuffData buff)
    //{
    //    // activeBuffs에 추가
    //    activeBuffs.Add(buff);
    //    // 버프 효과 발동
    //    ApplyBuffEffect(buff);
    //    OnChangedBuff?.Invoke(activeBuffs); // 버프 목록이 변경되었다고 신호

    //    yield return new WaitForSeconds(buff.duration); // 지속 시간동안 대기

        
    //    // activeBuffs에 삭제
    //    activeBuffs.Remove(buff);
    //    // 버프 효과 종료
    //    RemoveBuffEffect(buff);
    //    OnChangedBuff?.Invoke(activeBuffs); // 버프 목록이 변경되었다고 신호
    //}

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
