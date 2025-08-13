using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{ 
    public List<BuffData> activeBuffs = new List<BuffData>();
    private PlayerController controller;
    private Inventory inventory;


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
        Debug.Log(buff.type + " 버프 추가 요청 받음! 값: " + buff.value + ", 시간: " + buff.duration);
        StartCoroutine(BuffProcess(buff));
    }

    IEnumerator BuffProcess(BuffData buff)
    {
        // activeBuffs에 추가
        activeBuffs.Add(buff);
        // 버프 효과 발동
        ApplyBuffEffect(buff);


        yield return new WaitForSeconds(buff.duration); // 지속 시간동안 대기

        
        // activeBuffs에 삭제
        activeBuffs.Remove(buff);
        // 버프 효과 종료
        RemoveBuffEffect(buff);
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
